using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Namotion.Reflection;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using SigSpec.Core.Attributes;

namespace SigSpec.Core
{
    public class SigSpecGenerator
    {
        private readonly SigSpecGeneratorSettings _settings;

        public SigSpecGenerator(SigSpecGeneratorSettings settings)
        {
            _settings = settings;
        }

        public SigSpecDocument GenerateForHubs(params Type[] hubs)
        {
            var document = new SigSpecDocument();
            return GenerateForHubs(hubs, document);
        }

        public SigSpecDocument GenerateForHubs(IReadOnlyCollection<Type> hubs, SigSpecDocument template)
        {
            var document = template;
            var resolver = new SigSpecSchemaResolver(document, _settings);
            var generator = new JsonSchemaGenerator(_settings);

            foreach (var type in hubs)
            {
                var hub = new SigSpecHub();
                hub.Name = type.Name.EndsWith("Hub") ? type.Name.Substring(0, type.Name.Length - 3) : type.Name;
                hub.Description = type.GetXmlDocsSummary();

                foreach (var method in GetHubMethods(type))
                {
                    var operation = GenerateOperation(method, generator, resolver, SigSpecOperationType.Sync);
                    hub.Operations[method.Name] = operation;
                }

                foreach (var method in GetChannelMethods(type))
                {
                    hub.Operations[method.Name] = GenerateOperation(method, generator, resolver, SigSpecOperationType.Observable);
                }

                var baseTypeGenericArguments = type.BaseType.GetGenericArguments();
                if (baseTypeGenericArguments.Length == 1)
                {
                    var callbackType = baseTypeGenericArguments[0];
                    foreach (var callbackMethod in GetCallbackMethods(callbackType))
                    {
                        var callback = GenerateOperation(callbackMethod, generator, resolver, SigSpecOperationType.Sync);
                        hub.Callbacks[callbackMethod.Name] = callback;
                    }
                }

                document.Hubs[nameof(type)] = hub;
            }
            return document;
        }

        private IEnumerable<MethodInfo> GetHubMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
            {
                var returnsChannelReader = m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(ChannelReader<>);
                return
                    m.GetCustomAttribute<SigSpecRegisterAttribute>() != null &&
                    !returnsChannelReader;
            });
        }

        private IEnumerable<MethodInfo> GetCallbackMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
            {
                var returnsChannelReader = m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(ChannelReader<>);
                return !returnsChannelReader;
            });
        }

        private IEnumerable<MethodInfo> GetChannelMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
            {
                var returnsChannelReader = m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(ChannelReader<>);
                return m.GetCustomAttribute<SigSpecRegisterAttribute>() != null && returnsChannelReader;
            });
        }

        private Type GetReturnType(SigSpecOperationType operationType, MethodInfo method)
        {
            if (operationType == SigSpecOperationType.Observable)
            {
                return method.ReturnType.GetGenericArguments().First();
            }
            if (method.ReturnType == typeof(Task))
            {
                return null;
            }
            if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
            {
                return method.ReturnType.GetGenericArguments().First();
            }
            return method.ReturnType;
        }

        private SigSpecOperation GenerateOperation(MethodInfo method, JsonSchemaGenerator generator, SigSpecSchemaResolver resolver, SigSpecOperationType operationType)
        {
            var operation = new SigSpecOperation
            {
                    Description = method.GetXmlDocsSummary(),
                    Type = operationType
            };
                        
            var ignoreParams = new List<Type> { typeof(CancellationToken), typeof(InvocationContext), typeof(ILogger) };
            foreach (var arg in method.GetParameters().Where(param => !ignoreParams.Contains(param.ParameterType)))
            {
                var parameter = generator.GenerateWithReferenceAndNullability<SigSpecParameter>(
                    arg.ParameterType.ToContextualType(), arg.ParameterType.ToContextualType().IsNullableType, resolver, (p, s) =>
                    {
                            p.Description = arg.GetXmlDocs();
                    });

                operation.Parameters[arg.Name] = parameter;
            }

            var returnType = GetReturnType(operationType, method);

            if (operation.ReturnType != null)
            {
                generator.GenerateWithReferenceAndNullability<JsonSchema>(
                returnType.ToContextualType(), returnType.ToContextualType().IsNullableType, resolver, (p, s) => 
                    p.Description = method.ReturnType.GetXmlDocsSummary());
            }
            return operation;
        }
    }
}
