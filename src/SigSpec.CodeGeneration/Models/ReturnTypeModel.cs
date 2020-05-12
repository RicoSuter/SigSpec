using NJsonSchema;
using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration.Models
{
    public class ReturnTypeModel
    {
        private readonly JsonSchema _parameter;
        private readonly TypeResolverBase _resolver;

        public ReturnTypeModel(JsonSchema parameter, TypeResolverBase resolver)
        {
            _parameter = parameter;
            _resolver = resolver;

            Type = _resolver.Resolve(_parameter.ActualTypeSchema, _parameter.IsNullable(SchemaType.JsonSchema), null);
        }

        public string Type { get; }
    }
}