using System.Linq;
using NJsonSchema;

namespace SigSpec.CodeGeneration.JavaScript.Models
{
    public static class Extensions
    {
        public static string ToConstCaseLower(this string @this) => string.Concat(@this.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        public static string ToConstCase(this string @this) => @this.ToConstCaseLower().ToUpper();
        public static string ToCamelCase(this string @this) => ConversionUtilities.ConvertToLowerCamelCase(@this, true);
    }
}
