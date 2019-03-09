using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using SigSpec.CodeGeneration.JavaScript.Models;
using SigSpec.CodeGeneration.JavaScript.Models;
using SigSpec.Core;

namespace SigSpec.CodeGeneration.JavaScript
{
    public class SigSpecToJavaScriptGenerator
    {
        private readonly SigSpecToJavaScriptGeneratorSettings _settings;

        public SigSpecToJavaScriptGenerator(SigSpecToJavaScriptGeneratorSettings settings)
        {
            this._settings = settings;
        }

        public CodeArtifactCollection GenerateArtifacts(SigSpecDocument document)
        {
            var resolver = new TypeScriptTypeResolver(this._settings.TypeScriptGeneratorSettings);
            resolver.RegisterSchemaDefinitions(document.Definitions);

            var artifacts = new List<CodeArtifact>();
            foreach (var hub in document.Hubs)
            {
                var hubModel = new HubModel(hub.Key, hub.Value, resolver);
                var template = this._settings.TypeScriptGeneratorSettings.TemplateFactory.CreateTemplate("JavaScript", "Hub", hubModel);
                artifacts.Add(new CodeArtifact(hubModel.Name, CodeArtifactType.Class, CodeArtifactLanguage.TypeScript, template));
            }

            if (this._settings.GenerateDtoTypes)
            {
                var generator = new TypeScriptGenerator(document, this._settings.TypeScriptGeneratorSettings, resolver);
                var types = generator.GenerateTypes();

                return new CodeArtifactCollection(artifacts.Concat(types.Artifacts), types.ExtensionCode);
            }
            else
            {
                var extensionCode = new TypeScriptExtensionCode(this._settings.TypeScriptGeneratorSettings.ExtensionCode, this._settings.TypeScriptGeneratorSettings.ExtendedClasses);
                return new CodeArtifactCollection(artifacts, extensionCode);
            }
        }

        public string GenerateFile(SigSpecDocument document)
        {
            var artifacts = this.GenerateArtifacts(document);

            var fileModel = new FileModel(artifacts.Artifacts.Select(a => a.Code));
            var fileTemplate = this._settings.TypeScriptGeneratorSettings.TemplateFactory.CreateTemplate("JavaScript", "File", fileModel);

            return fileTemplate.Render();
        }
    }
}
