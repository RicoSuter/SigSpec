using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;

namespace SigSpec.Core
{
    public class SigSpecHub
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("operations")]
        public IDictionary<string, SigSpecOperation> Operations { get; } = new Dictionary<string, SigSpecOperation>();

        [JsonProperty("callbacks")]
        public IDictionary<string, SigSpecOperation> Callbacks { get; } = new Dictionary<string, SigSpecOperation>();
    }
}
