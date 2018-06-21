using System.Reflection;
using NJsonSchema.CodeGeneration.TypeScript;

namespace SigSpec.CodeGeneration.TypeScript
{
    public class SigSpecToTypeScriptGeneratorSettings : SigSpecToTypeScriptGeneratorSettingsBase
    {
        public SigSpecToTypeScriptGeneratorSettings()
            : base(new TypeScriptGeneratorSettings())
        {
            TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
            CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(TypeScriptGeneratorSettings, new[]
            {
                typeof(TypeScriptGeneratorSettings).GetTypeInfo().Assembly,
                typeof(SigSpecToTypeScriptGeneratorSettingsBase).GetTypeInfo().Assembly,
            });
        }

        public TypeScriptGeneratorSettings TypeScriptGeneratorSettings => (TypeScriptGeneratorSettings)CodeGeneratorSettings;
    }
}
