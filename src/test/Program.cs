using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Fluid.Filters;
using Microsoft.AspNetCore.SignalR;
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
            string[] pdbs = Directory.GetFiles(dllDir, "*.pdb");



            //Assembly ServerFlatTop = Assembly.LoadFile("D:\\workingdir\\newgen\\code\\git\\demo-comansa\\RaycoWylie.Server.FlatTopTC\\bin\\Release\\net6.0\\RaycoWylie.Server.FlatTopTC.dll");
            /*
            AssemblyLoadContext loadContext = new AssemblyLoadContext(null); //System.Runtime.Loader.AssemblyLoadContext.GetLoadContext(ServerFlatTop);
            //Assembly ServerFlatTop = loadContext.LoadFromAssemblyPath(dllFile);

            
            foreach(string dll in dlls)
                loadContext.LoadFromAssemblyPath(dll);

           *foreach (string dll in Directory.GetFiles(".", "*.dll"))
            {
                if(loadContext.Assemblies.Count(a => loadContext.Assemblies.First().GetName().Name == Path.GetFileNameWithoutExtension(dll)) == 0 && dll != null)
                    loadContext.LoadFromAssemblyPath(Path.GetFullPath(dll));
            }(/

            Assembly ServerFlatTop = loadContext.Assemblies.First(a => a.FullName.StartsWith(dllname));


            /*
            AppDomain doms = AppDomain.CreateDomain("bob");

            foreach(string dll in dlls)
                doms.Load(File.ReadAllBytes(dll));


            Assembly ServerFlatTop = doms.GetAssemblies().First(a => a.FullName.StartsWith(dllname));
            */


            PluginLoadContext context = new PluginLoadContext(dllFile);

            Assembly ServerFlatTop = context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(dllFile)));




            Type[] types = ServerFlatTop.GetTypes();


            var settings = new SigSpecGeneratorSettings();
            var generator = new SigSpecGenerator(settings);

            Type[] hubss = types.Where(t => t.IsAssignableFrom(typeof(Hub))).ToArray();
            Type[] hubs = types.Where(t => t.IsAssignableTo(typeof(Hub))).ToArray();

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