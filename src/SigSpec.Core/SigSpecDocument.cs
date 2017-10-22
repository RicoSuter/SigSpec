using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;

namespace SigSpec.Core
{
    [JsonConverter(typeof(JsonReferenceConverter))]
    public class SigSpecDocument
    {
        [JsonProperty("hubs")]
        public IDictionary<string, SigSpecHub> Hubs { get; } = new Dictionary<string, SigSpecHub>();

        [JsonProperty("definitions")]
        public IDictionary<string, JsonSchema4> Definitions { get; } = new Dictionary<string, JsonSchema4>();

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
