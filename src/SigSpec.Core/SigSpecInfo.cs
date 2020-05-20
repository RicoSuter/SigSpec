using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;

namespace SigSpec.Core
{
    /// <summary>The web service description.</summary>
    public class SigSpecInfo : JsonExtensionObject
    {
        /// <summary>Gets or sets the title.</summary>
        [JsonProperty(PropertyName = "title", Required = Required.Always,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Title { get; set; } = "SigSpec specification";

        /// <summary>Gets or sets the description.</summary>
        [JsonProperty(PropertyName = "description", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Description { get; set; }

        /// <summary>Gets or sets the terms of service.</summary>
        [JsonProperty(PropertyName = "termsOfService", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string TermsOfService { get; set; }

        /// <summary>Gets or sets the API version.</summary>
        [JsonProperty(PropertyName = "version", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Version { get; set; } = "1.0.0";
    }
}
