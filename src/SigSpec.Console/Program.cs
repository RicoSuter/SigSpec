using HelloSignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SigSpec
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("SigSpec for SignalR Core");

            var serializerSettings = new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            });

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
            File.WriteAllText("signalr.spec", json);
            Console.ReadKey();

            var codeGeneratorSettings = new SigSpecToTypeScriptGeneratorSettings();
            var codeGenerator = new SigSpecToTypeScriptGenerator(codeGeneratorSettings);
            var content = File.ReadAllText("signalr.spec");
            var deserializedDocument = JsonConvert.DeserializeObject<SigSpecDocument>(content, serializerSettings.Value);
            var file = codeGenerator.GenerateFile(deserializedDocument);

            Console.WriteLine("\n\nGenerated SigSpec TypeScript code:");
            Console.WriteLine(file);
            Console.ReadKey();
        }
    }
}
