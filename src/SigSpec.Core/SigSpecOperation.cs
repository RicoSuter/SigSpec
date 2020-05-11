using Newtonsoft.Json;
using NJsonSchema;
using System.Collections.Generic;
using System;

namespace SigSpec.Core
{
    public class SigSpecOperation
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, SigSpecParameter> Parameters { get; } = new Dictionary<string, SigSpecParameter>();

        [JsonProperty("returntype", NullValueHandling = NullValueHandling.Ignore)]
        public JsonSchema ReturnType { get; set; }

        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SigSpecOperationType Type { get; set; }
    }
}