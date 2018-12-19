using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.Models
{
    public class OperationModel
    {
        private readonly string _name;
        private readonly SigSpecOperation _operation;
        private readonly TypeResolverBase _resolver;
        private readonly ReturnTypeModel _returnType;

        public OperationModel(string name, SigSpecOperation operation, TypeResolverBase resolver)
        {
            _name = name;
            _operation = operation;
            _resolver = resolver;
            _returnType = operation.ReturnType == null ? null : new ReturnTypeModel("", _operation.ReturnType, _resolver);
        }

        public string Name => _name;

        public string MethodName => ConversionUtilities.ConvertToLowerCamelCase(_name, true);

        public IEnumerable<ParameterModel> Parameters => _operation.Parameters.Select(o => new ParameterModel(o.Key, o.Value, _resolver));

        public ReturnTypeModel ReturnType => _returnType;

        public bool IsObservable => _operation.Type == OperationType.Observable;
        public bool HasReturnType => _returnType != null;
    }
}
