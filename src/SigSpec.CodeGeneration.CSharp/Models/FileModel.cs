using System.Collections.Generic;

namespace SigSpec.CodeGeneration.CSharp.Models
{
    public class FileModel
    {
        public FileModel(IEnumerable<string> hubs, string @namespace)
        {
            Hubs = hubs;
            Namespace = @namespace;
        }
        public string Namespace { get; set; }

        public IEnumerable<string> Hubs { get; }
    }
}
