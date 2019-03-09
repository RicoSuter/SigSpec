using System.Reflection;
using NJsonSchema.CodeGeneration.TypeScript;

namespace SigSpec.CodeGeneration.JavaScript
{
    public class SigSpecToJavaScriptGeneratorSettings : SigSpecToTypeScriptGeneratorSettingsBase
    {
        public SigSpecToJavaScriptGeneratorSettings()
            : base(new TypeScriptGeneratorSettings())
        {
            this.GenerateDtoTypes = false;
            this.TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
            this.CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(this.TypeScriptGeneratorSettings, new[]
            {
                typeof(TypeScriptGeneratorSettings).GetTypeInfo().Assembly,
                typeof(SigSpecToTypeScriptGeneratorSettingsBase).GetTypeInfo().Assembly,
            });
        }

        public TypeScriptGeneratorSettings TypeScriptGeneratorSettings => (TypeScriptGeneratorSettings)this.CodeGeneratorSettings;
    }
}
