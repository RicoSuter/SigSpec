using HelloSignalR;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SigSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SigSpec for SignalR Core");
            Run();
        }

        static void Run()
        {
            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            var document = generator.GenerateForHubs(new Dictionary<string, Type>
            {
                { "chat", typeof(ChatHub) }
            });

            var json = document.ToJson();

            Console.WriteLine("\nGenerated SigSpec document:");
            Console.WriteLine(json);
            Console.ReadKey();

            var codeGeneratorSettings = new SigSpecToTypeScriptGeneratorSettings();
            var codeGenerator = new SigSpecToTypeScriptGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateFile(document);

            Console.WriteLine("\n\nGenerated SigSpec TypeScript code:");
            Console.WriteLine(file);
            Console.ReadKey();
        }
    }
}
