using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;

namespace SigSpec.CodeGeneration.Models
{
    public class ParameterModel
    {
        public ParameterModel(string name, SigSpecParameter parameter, TypeResolverBase resolver)
        {
            Name = name;
            Type = resolver.Resolve(parameter.ActualTypeSchema, parameter.IsNullable(SchemaType.JsonSchema), Name);
        }

        public string Name { get; }

        public string Type { get; }
    }
}
