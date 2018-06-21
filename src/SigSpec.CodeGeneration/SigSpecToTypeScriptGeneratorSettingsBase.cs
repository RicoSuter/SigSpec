using NJsonSchema.CodeGeneration;

namespace SigSpec.CodeGeneration
{
    public class SigSpecToTypeScriptGeneratorSettingsBase : ClientGeneratorBaseSettings
    {
        public SigSpecToTypeScriptGeneratorSettingsBase(CodeGeneratorSettingsBase codeGeneratorSettings)
        {
            CodeGeneratorSettings = codeGeneratorSettings;
        }

        protected CodeGeneratorSettingsBase CodeGeneratorSettings { get; }
    }
}
