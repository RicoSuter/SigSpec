using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SigSpec.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SigSpecOperationType
    {
        Sync,

        Observable
    }
}