using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;
using System;

namespace SigSpec.Core
{
    public class SigSpecChannel
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, SigSpecParameter> Parameters { get; } = new Dictionary<string, SigSpecParameter>();

        [JsonProperty("returntype")]
        public JsonSchema4 ReturnType { get; set; }
    }
}
