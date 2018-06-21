using System.IO;
using System.Reflection;
using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration
{
    public class DefaultTemplateFactory : NJsonSchema.CodeGeneration.DefaultTemplateFactory
    {
        public DefaultTemplateFactory(CodeGeneratorSettingsBase settings, Assembly[] assemblies) 
            : base(settings, assemblies)
        {
        }

        /// <summary>Tries to load an embedded Liquid template.</summary>
        /// <param name="language">The language.</param>
        /// <param name="template">The template name.</param>
        /// <returns>The template.</returns>
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            var assembly = Assembly.Load(new AssemblyName("SigSpec.CodeGeneration." + language));
            var resourceName = "SigSpec.CodeGeneration." + language + ".Templates." + template + ".liquid";

            var resource = assembly.GetManifestResourceStream(resourceName);
            if (resource != null)
            {
                using (var reader = new StreamReader(resource))
                    return reader.ReadToEnd();
            }

            return base.GetEmbeddedLiquidTemplate(language, template);
        }
    }
}
