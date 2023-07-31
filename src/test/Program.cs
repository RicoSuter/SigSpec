using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Fluid.Filters;
using Microsoft.AspNetCore.SignalR;
using Namotion.Reflection;
using SigSpec.CodeGeneration.CSharp;
using SigSpec.CodeGeneration.TypeScript;
using SigSpec.Core;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            
            string dllFile = "D:\\workingdir\\newgen\\code\\git\\demo-comansa\\RaycoWylie.Server.FlatTopTC\\bin\\Release\\net6.0\\RaycoWylie.Server.FlatTopTC.dll";
            string dllname = Path.GetFileNameWithoutExtension(dllFile);
            string dllDir = Path.GetDirectoryName(dllFile)!;
            string[] dlls = Directory.GetFiles(dllDir, "*.dll");

            foreach (string dll in dlls)
                AppDomain.CurrentDomain.Load(File.ReadAllBytes(dll));

            foreach (string dll in Directory.GetFiles(Path.Join(dllDir, "win-x64"), "*.dll"))
            {
                try
                {
                    AppDomain.CurrentDomain.Load(File.ReadAllBytes(dll));
                }
                catch (BadImageFormatException e)
                {
                    Console.WriteLine("Not loaded : " + dll);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Not loaded : " + dll);
                }
            }

            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Name);
                var asss = AppDomain.CurrentDomain.GetAssemblies();

                Assembly? bob = asss.FirstOrDefault(a => a.ToString() == eventArgs.Name);
                if (bob is not null)
                {
                    Console.WriteLine("Pre-Loaded : " + bob.GetName().ToString());
                    return bob;
                }

                string assemblyPath = Path.Combine(dllDir, new AssemblyName(eventArgs.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    Assembly ass = Assembly.LoadFrom(assemblyPath);
                    Console.WriteLine("LOADED : " + ass.GetName().ToString());
                    return ass;
                }
                assemblyPath = Path.Join( dllDir, "win-x64", new AssemblyName(eventArgs.Name).Name + ".dll"); 
                if(File.Exists(assemblyPath))
                {
                    Assembly ass = Assembly.LoadFrom(assemblyPath);
                    Console.WriteLine("LOADED : " + ass.GetName().ToString());
                    return ass;
                }
                
                string? pathVar = Environment.GetEnvironmentVariable("PATH");
                string[] paths = pathVar?.Split(';') ?? new string[0];
                foreach (string path in paths)
                {
                    assemblyPath = Path.Join( path, new AssemblyName(eventArgs.Name).Name + ".dll"); 
                    if(File.Exists(assemblyPath))
                    {
                        Assembly ass = Assembly.LoadFrom(assemblyPath);
                        Console.WriteLine("LOADED : " + ass.GetName().ToString());
                        return ass;
                    }
                }
                Console.WriteLine("Not found.");
                return null;
            };
            Assembly ServerFlatTop = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.StartsWith(dllname));
            
            
            
            Type[] types = ServerFlatTop.GetTypes();


            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            Type[] hubs = types.Where(t => t.BaseType?.FullName?.Contains("Microsoft.AspNetCore.SignalR.Hub") ?? false).ToArray();

            var document = generator.GenerateForHubsAsync(new Dictionary<string, Type>(hubs.Select(t => new KeyValuePair<string, Type>(t.Name, t)))).Result;

            var rSimplified = document.Definitions["RSimplified"];
            foreach (var key in rSimplified.Properties.Keys)
            {
                if (key != "stringID" && key != "properties" && key != "objectValue")
                    rSimplified.Properties.Remove(key);
            }

            var json = document.ToJson();
            File.WriteAllText(Path.Combine("", "specs.json"), json);
        }
    }

    class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
    }
}