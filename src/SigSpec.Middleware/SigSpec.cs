using SigSpec.CodeGeneration.JavaScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace SigSpec.Middleware
{
    public static class SigSpec
    {
        public static void Generate<T>(string route, string outputPath)
            where T : class
        {
            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);
            var document = generator.GenerateForHubsAsync(new Dictionary<string, Type>
            {
                { "route".Trim('/'), typeof(T) }
            }).GetAwaiter().GetResult();

            var codeGeneratorSettings = new SigSpecToJavaScriptGeneratorSettings();
            var codeGenerator = new SigSpecToJavaScriptGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateFile(document);
            File.WriteAllText(outputPath, file); 
        }
    }
}
