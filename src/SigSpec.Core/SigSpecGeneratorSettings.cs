using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema.Generation;

namespace SigSpec.Core
{
    public class SigSpecGeneratorSettings : JsonSchemaGeneratorSettings
    {
        public SigSpecGeneratorSettings()
        {
            this.SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
