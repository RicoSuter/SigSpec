using HelloSignalR;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SigSpec.CodeGeneration;
using SigSpec.CodeGeneration.CSharp;

namespace SigSpec
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("SigSpec for SignalR Core");
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            var document = await generator.GenerateForHubsAsync(new Dictionary<string, Type>
            {
                { "chat", typeof(ChatHub) }
            });

            var json = document.ToJson();
            Console.WriteLine("\nGenerated SigSpec document:");
            Console.WriteLine(json);
            Console.ReadKey();

            var codeGeneratorSettings = new SigSpecToCSharpGeneratorSettings();
            var codeGenerator = new SigSpecToCSharpGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateClients(document);

            Console.WriteLine("\n\nGenerated SigSpec CSharp clients:");
            Console.WriteLine(file);
            Console.ReadKey();
        }
    }
}
