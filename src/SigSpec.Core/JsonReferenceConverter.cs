using Newtonsoft.Json;
using NJsonSchema;
using System;

namespace SigSpec.Core
{
    public class JsonReferenceConverter : JsonConverter
    {
        // TODO: Move to NJsonSchema

        [ThreadStatic]
        private static bool _canWrite = true;

        public override bool CanWrite => _canWrite;

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JsonSchemaReferenceUtilities.UpdateSchemaReferencePaths(value, false);
            try
            {
                _canWrite = false;
                var json = JsonConvert.SerializeObject(value, Formatting.Indented);
                writer.WriteRaw(JsonSchemaReferenceUtilities.ConvertPropertyReferences(json));
            }
            finally
            {
                _canWrite = true;
            }
        }
    }
}
