using NJsonSchema.CodeGeneration;
using NJsonSchema.CodeGeneration.TypeScript;
using SigSpec.CodeGeneration.Models;
using SigSpec.CodeGeneration.TypeScript.Models;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.TypeScript
{
    public class SigSpecToTypeScriptGenerator
    {
        private readonly SigSpecToTypeScriptGeneratorSettings _settings;

        public SigSpecToTypeScriptGenerator(SigSpecToTypeScriptGeneratorSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<CodeArtifact> GenerateArtifacts(SigSpecDocument document)
        {
            var resolver = new TypeScriptTypeResolver(_settings.TypeScriptGeneratorSettings);
            resolver.RegisterSchemaDefinitions(document.Definitions);

            var artifacts = new List<CodeArtifact>();
            foreach (var hub in document.Hubs)
            {
                var hubModel = new HubModel(hub.Key, hub.Value, resolver);
                var template = _settings.TypeScriptGeneratorSettings.TemplateFactory.CreateTemplate("TypeScript", "Hub", hubModel);
                artifacts.Add(new CodeArtifact(hubModel.Name, CodeArtifactType.Class, CodeArtifactLanguage.TypeScript, CodeArtifactCategory.Client, template));
            }

            if (_settings.GenerateDtoTypes)
            {
                var generator = new TypeScriptGenerator(document, _settings.TypeScriptGeneratorSettings, resolver);
                var types = generator.GenerateTypes();

                return artifacts.Concat(types);
            }
            else
            {
                return artifacts;
            }
        }

        public string GenerateFile(SigSpecDocument document)
        {
            var artifacts = GenerateArtifacts(document);

            var fileModel = new FileModel(artifacts.Select(a => a.Code));
            var fileTemplate = _settings.TypeScriptGeneratorSettings.TemplateFactory.CreateTemplate("TypeScript", "File", fileModel);

            return fileTemplate.Render();
        }
    }
}
