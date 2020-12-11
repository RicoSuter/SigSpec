using System.Reflection;
using NJsonSchema.CodeGeneration.CSharp;

namespace SigSpec.CodeGeneration.CSharp
{
    public class SigSpecToCSharpGeneratorSettings : SigSpecToCSharpGeneratorSettingsBase
    {
        public SigSpecToCSharpGeneratorSettings()
            : base(new CSharpGeneratorSettings())
        {
            CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(CSharpGeneratorSettings, new[]
            {
                typeof(CSharpGeneratorSettings).GetTypeInfo().Assembly,
                typeof(SigSpecToCSharpGeneratorSettingsBase).GetTypeInfo().Assembly
            });
            //TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
            //CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(TypeScriptGeneratorSettings, new[]
            //{
            //    typeof(TypeScriptGeneratorSettings).GetTypeInfo().Assembly,
            //    typeof(SigSpecToTypeScriptGeneratorSettingsBase).GetTypeInfo().Assembly,
            //});

        }

        public CSharpGeneratorSettings CSharpGeneratorSettings => (CSharpGeneratorSettings)CodeGeneratorSettings;
    }
}
