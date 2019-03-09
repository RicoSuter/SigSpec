using System.Collections.Generic;

namespace SigSpec.CodeGeneration.JavaScript.Models
{
    public class FileModel
    {
        public FileModel(IEnumerable<string> hubs)
        {
            this.Hubs = hubs;
        }

        public IEnumerable<string> Hubs { get; }
    }
}
