using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Converters;
using System;
using System.Collections.Generic;

namespace SigSpec.Core
{
    [JsonConverter(typeof(JsonReferenceConverter))]
    public class SigSpecDocument
    {
        private static Lazy<JsonSerializerSettings> _serializerSettings = new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        });

        [JsonProperty("hubs")]
        public IDictionary<string, SigSpecHub> Hubs { get; } = new Dictionary<string, SigSpecHub>();

        [JsonProperty("definitions")]
        public IDictionary<string, JsonSchema> Definitions { get; } = new Dictionary<string, JsonSchema>();

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, _serializerSettings.Value);
        }
    }
}
