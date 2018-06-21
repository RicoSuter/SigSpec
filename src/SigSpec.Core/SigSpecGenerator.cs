using Microsoft.AspNetCore.SignalR;
using NJsonSchema.Generation;
using NJsonSchema.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
            var resolver = new SigSpecSchemaResolver(document, _settings);
            var generator = new JsonSchemaGenerator(_settings);

            foreach (var h in hubs)
            {
                var type = h.Value;

                var hub = new SigSpecHub();
                hub.Name = type.Name.EndsWith("Hub") ? type.Name.Substring(0, type.Name.Length - 3) : type.Name;
                hub.Description = await type.GetXmlSummaryAsync();

                foreach (var method in GetOperationMethods(type))
                {
                    var operation = await GenerateOperationAsync(type, method, generator, resolver);
                    hub.Operations[method.Name] = operation;

                    var baseTypeGenericArguments = type.BaseType.GetGenericArguments();
                    if (baseTypeGenericArguments.Length == 1)
                    {
                        var callbackType = baseTypeGenericArguments[0];
                        foreach (var callbackMethod in GetOperationMethods(callbackType))
                        {
                            var callback = await GenerateOperationAsync(type, callbackMethod, generator, resolver);
                            hub.Callbacks[callbackMethod.Name] = callback;
                        }
                    }
                }

                document.Hubs[h.Key] = hub;
            }

            return document;
        }

        private IEnumerable<MethodInfo> GetOperationMethods(Type type)
        {
            return type.GetTypeInfo().GetRuntimeMethods().Where(m =>
                m.IsPublic && 
                m.IsSpecialName == false && 
                m.DeclaringType != typeof(Hub) && 
                m.DeclaringType != typeof(object));
        }

        private async Task<SigSpecOperation> GenerateOperationAsync(Type type, MethodInfo method, JsonSchemaGenerator generator, SigSpecSchemaResolver resolver)
        {
            var operation = new SigSpecOperation();
            operation.Description = await type.GetXmlSummaryAsync();

            foreach (var arg in method.GetParameters())
            {
                var parameter = await generator.GenerateWithReferenceAndNullabilityAsync<SigSpecParameter>(
                    arg.ParameterType, 
                    arg.GetCustomAttributes(), 
                    resolver, 
                    async (p, s) =>
                    {
                        p.Description = await arg.GetXmlDocumentationAsync();
                    });

                operation.Parameters[arg.Name] = parameter;
            }

            return operation;
        }
    }
}
