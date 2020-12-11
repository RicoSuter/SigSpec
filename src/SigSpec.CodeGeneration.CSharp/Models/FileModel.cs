using System.Collections.Generic;

namespace SigSpec.CodeGeneration.CSharp.Models
{
    public class FileModel
    {
        public FileModel(IEnumerable<string> hubs)
        {
            Hubs = hubs;
        }

        public IEnumerable<string> Hubs { get; }
    }
}
