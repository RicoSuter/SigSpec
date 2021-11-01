using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelloSignalR;
using SigSpec.CodeGeneration.CSharp;
using SigSpec.Core;

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
            codeGeneratorSettings.GenerateInterface = true;
            var codeGenerator = new SigSpecToCSharpGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateClients(document);

            Console.WriteLine("\n\nGenerated SigSpec CSharp clients:");
            Console.WriteLine(file);
            Console.ReadKey();
        }
    }
}
