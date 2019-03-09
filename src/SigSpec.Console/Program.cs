using HelloSignalR;
using SigSpec.CodeGeneration.JavaScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoldGroup.Progress;

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
                { "progress", typeof(ProgressHub) }
            });

            //var json = document.ToJson();
            //Console.WriteLine("\nGenerated SigSpec document:");
            //Console.WriteLine(json);
            //Console.ReadKey();

            var codeGeneratorSettings = new SigSpecToJavaScriptGeneratorSettings();
            var codeGenerator = new SigSpecToJavaScriptGenerator(codeGeneratorSettings);
            var file = codeGenerator.GenerateFile(document);

            Console.WriteLine("Generated SigSpec JavaScript code:\n\n");
            Console.WriteLine(file);
            Console.ReadKey();
        }
    }
}
