using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;

namespace SigSpec.CodeGeneration.JavaScript.Models
{
    public class HubModel
    {
        private readonly SigSpecHub _hub;
        private readonly TypeResolverBase _resolver;

        public HubModel(string path, SigSpecHub hub, TypeResolverBase resolver)
        {
            this.Path = path;
            this._hub = hub;
            this._resolver = resolver;
        }

        public string Name => this._hub.Name;
        public string NameCamelCase => this._hub.Name.ToCamelCase();
        public string NameConstCase => this._hub.Name.ToConstCase();
        public string Path { get; }

        public IEnumerable<OperationModel> Operations => this._hub.Operations.Select(o => new OperationModel(o.Key, o.Value, this._resolver));

        public IEnumerable<OperationModel> Callbacks => this._hub.Callbacks.Select(o => new OperationModel(o.Key, o.Value, this._resolver));
        public IEnumerable<OperationModel> All => this.Callbacks.Concat(this.Operations);


    }
}
