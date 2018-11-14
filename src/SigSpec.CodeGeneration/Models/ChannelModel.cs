using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.Models
{
    public class ChannelModel
    {
        private readonly string _name;
        private readonly SigSpecChannel _channel;
        private readonly TypeResolverBase _resolver;

        public ChannelModel(string name, SigSpecChannel channel, TypeResolverBase resolver)
        {
            _name = name;
            _channel = channel;
            _resolver = resolver;
        }

        public string Name => _name;

        public string MethodName => ConversionUtilities.ConvertToLowerCamelCase(_name, true);

        public IEnumerable<ParameterModel> Parameters => _channel.Parameters.Select(o => new ParameterModel(o.Key, o.Value, _resolver));

        public ReturnTypeModel ReturnType => new ReturnTypeModel("", _channel.ReturnType, _resolver);
    }
}
