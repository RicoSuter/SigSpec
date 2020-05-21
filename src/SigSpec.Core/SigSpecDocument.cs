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

        /// <summary>Gets or sets the SigSpec specification version being used.</summary>
        [JsonProperty(PropertyName = "sigspec", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SigSpec { get; set; } = "1.0.0";

        /// <summary>Gets or sets the metadata about the API.</summary>
        [JsonProperty(PropertyName = "info", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public SigSpecInfo Info { get; set; } = new SigSpecInfo();

        /// <summary>Gets the exposed SignalR hubs.</summary>
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
