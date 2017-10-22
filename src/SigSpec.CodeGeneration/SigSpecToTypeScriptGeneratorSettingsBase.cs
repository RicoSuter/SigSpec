using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration
{
    public class SigSpecToTypeScriptGeneratorSettingsBase
    {
        public SigSpecToTypeScriptGeneratorSettingsBase(CodeGeneratorSettingsBase codeGeneratorSettings)
        {
            CodeGeneratorSettings = codeGeneratorSettings;
            CodeGeneratorSettings.UseLiquidTemplates = true;
            CodeGeneratorSettings.TemplateFactory = new DefaultTemplateFactory(codeGeneratorSettings);
        }

        protected CodeGeneratorSettingsBase CodeGeneratorSettings { get; }
    }
}
