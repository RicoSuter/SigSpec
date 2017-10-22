using NJsonSchema.CodeGeneration.TypeScript;

namespace SigSpec.CodeGeneration.TypeScript
{
    public class SigSpecToTypeScriptGeneratorSettings : SigSpecToTypeScriptGeneratorSettingsBase
    {
        public SigSpecToTypeScriptGeneratorSettings()
            : base(new TypeScriptGeneratorSettings())
        {
            TypeScriptGeneratorSettings.TypeStyle = TypeScriptTypeStyle.Interface;
        }

        public TypeScriptGeneratorSettings TypeScriptGeneratorSettings => (TypeScriptGeneratorSettings)CodeGeneratorSettings;
    }
}
