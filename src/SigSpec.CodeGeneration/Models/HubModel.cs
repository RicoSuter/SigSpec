using NJsonSchema.CodeGeneration;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.Models
{
    public class HubModel
    {
        private readonly string _path;
        private readonly SigSpecHub _hub;
        private readonly TypeResolverBase _resolver;

        public HubModel(string path, SigSpecHub hub, TypeResolverBase resolver)
        {
            _path = path;
            _hub = hub;
            _resolver = resolver;
        }

        public string Name => _hub.Name;

        public IEnumerable<OperationModel> Operations => _hub.Operations.Select(o => new OperationModel(o.Key, o.Value, _resolver));

        public IEnumerable<OperationModel> Callbacks => _hub.Callbacks.Select(o => new OperationModel(o.Key, o.Value, _resolver));
    }
}
