using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Linq;

namespace SigSpec.Core
{
    public class SigSpecSchemaResolver : JsonSchemaResolver
    {
        private readonly SigSpecDocument _document;
        private readonly JsonSchemaGeneratorSettings _settings;

        public SigSpecSchemaResolver(SigSpecDocument document, SigSpecGeneratorSettings settings) 
            : base(document, settings)
        {
            _document = document;
            _settings = settings;
        }

        public override void AppendSchema(JsonSchema schema, string typeNameHint)
        {
            // TODO: JsonSchemaResolver should use new IDefinitionsObject interface and not JsonSchema4

            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (schema == RootObject)
                throw new ArgumentException("The root schema cannot be appended.");

            if (!_document.Definitions.Values.Contains(schema))
            {
                var typeName = _settings.TypeNameGenerator.Generate(schema, typeNameHint, _document.Definitions.Keys);
                if (!string.IsNullOrEmpty(typeName) && !_document.Definitions.ContainsKey(typeName))
                    _document.Definitions[typeName] = schema;
                else
                    _document.Definitions["ref_" + Guid.NewGuid().ToString().Replace("-", "_")] = schema;
            }
        }
    }
}
