using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.NewtonsoftJson.Generation;

namespace SigSpec.Core
{
    public class SigSpecGeneratorSettings : NewtonsoftJsonSchemaGeneratorSettings
    {
        public SigSpecGeneratorSettings()
        {
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
