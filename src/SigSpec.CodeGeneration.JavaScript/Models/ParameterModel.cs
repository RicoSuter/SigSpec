using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;

namespace SigSpec.CodeGeneration.JavaScript.Models
{
    public class ParameterModel
    {
        public ParameterModel(string name, SigSpecParameter parameter, TypeResolverBase resolver)
        {
            this.Name = name;
            this.Type = resolver.Resolve(parameter.ActualTypeSchema, parameter.IsNullable(SchemaType.JsonSchema), this.Name);
        }

        public string Name { get; }
        public string NameCamelCase => this.Name.ToCamelCase();

        public string Type { get; }
    }
}
