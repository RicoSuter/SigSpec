using System.Collections.Generic;

namespace SigSpec.CodeGeneration.TypeScript.Models
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
