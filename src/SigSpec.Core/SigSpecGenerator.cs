using Microsoft.AspNetCore.SignalR;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Channels;
using System.Threading.Tasks;
using Namotion.Reflection;

namespace SigSpec.Core
{
    public class SigSpecGenerator
    {
        private readonly SigSpecGeneratorSettings _settings;

        public SigSpecGenerator(SigSpecGeneratorSettings settings)
        {
            _settings = settings;
        }

        public async Task<SigSpecDocument> GenerateForHubsAsync(IReadOnlyDictionary<string, Type> hubs)
        {
            var document = new SigSpecDocument();
            return await GenerateForHubsAsync(hubs, document);
        }

        public async Task<SigSpecDocument> GenerateForHubsAsync(IReadOnlyDictionary<string, Type> hubs, SigSpecDocument template)
        {
            var document = template;
            var resolver = new SigSpecSchemaResolver(document, _settings);
            var generator = new JsonSchemaGenerator(_settings);

            foreach (var h in hubs)
            {
                var type = h.Value;

                var hub = new SigSpecHub();
                hub.Name = type.Name.EndsWith("Hub") ? type.Name.Substring(0, type.Name.Length - 3) : type.Name;
                hub.Description = type.GetXmlDocsSummary();

                foreach (var method in GetOperationMethods(type))
                {
                    var operation = await GenerateOperationAsync(type, method, generator, resolver, SigSpecOperationType.Sync);
                    hub.Operations[method.Name] = operation;
                }

                foreach (var method in GetChannelMethods(type))
                {
                    hub.Operations[method.Name] = await GenerateOperationAsync(type, method, generator, resolver, SigSpecOperationType.Observable);
                }

                var baseTypeGenericArguments = type.BaseType.GetGenericArguments();
                if (baseTypeGenericArguments.Length == 1)
                {
                    var callbackType = baseTypeGenericArguments[0];
                    foreach (var callbackMethod in GetOperationMethods(callbackType))
                    {
                        var callback = await GenerateOperationAsync(type, callbackMethod, generator, resolver, SigSpecOperationType.Sync);
                        hub.Callbacks[callbackMethod.Name] = callback;
                    }
                }

                document.Hubs[h.Key] = hub;
            }

            return document;
        }

        private static IEnumerable<string> _forbiddenOperations { get; } = typeof(Hub).GetRuntimeMethods().Concat(typeof(Hub<>).GetRuntimeMethods()).Select(x => x.Name).Distinct();
        private IEnumerable<MethodInfo> GetOperationMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
            {
                var returnsChannelReader = m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(ChannelReader<>);
                return
                    m.IsPublic &&
                    m.IsSpecialName == false &&
                    m.DeclaringType != typeof(Hub) &&
                    m.DeclaringType != typeof(Hub<>) &&
                    m.DeclaringType != typeof(object) &&
                    !_forbiddenOperations.Contains(m.Name) &&
                    returnsChannelReader == false;
            });
        }

        private IEnumerable<MethodInfo> GetChannelMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
            {
                var returnsChannelReader = m.ReturnType.IsGenericType && m.ReturnType.GetGenericTypeDefinition() == typeof(ChannelReader<>);
                return
                    m.IsPublic &&
                    m.IsSpecialName == false &&
                    m.DeclaringType != typeof(Hub) &&
                    m.DeclaringType != typeof(Hub<>) &&
                    m.DeclaringType != typeof(object) &&
                    !_forbiddenOperations.Contains(m.Name) &&
                    returnsChannelReader == true;
            });
        }

        private async Task<SigSpecOperation> GenerateOperationAsync(Type type, MethodInfo method, JsonSchemaGenerator generator, SigSpecSchemaResolver resolver, SigSpecOperationType operationType)
        {
            var operation = new SigSpecOperation
            {
                Description = method.GetXmlDocsSummary(),
                Type = operationType
            };

            foreach (var arg in method.GetParameters())
            {
                var parameter = generator.GenerateWithReferenceAndNullability<SigSpecParameter>(
                    arg.ParameterType.ToContextualType(), arg.ParameterType.ToContextualType().IsNullableType, resolver, (p, s) =>
                    {
                        p.Description = arg.GetXmlDocs();
                    });

                operation.Parameters[arg.Name] = parameter;
            }

            var returnType =
                operationType == SigSpecOperationType.Observable
                    ? method.ReturnType.GetGenericArguments().First()
                : method.ReturnType == typeof(Task)
                    ? null
                : method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task)
                    ? method.ReturnType.GetGenericArguments().First()
                    : method.ReturnType;

            operation.ReturnType = returnType == null ? null : generator.GenerateWithReferenceAndNullability<JsonSchema>(
                returnType.ToContextualType(), returnType.ToContextualType().IsNullableType, resolver, async (p, s) =>
                {
                    p.Description = method.ReturnType.GetXmlDocsSummary();
                });

            return operation;
        }
    }
}
