using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.Core;
using System.Collections.Generic;
using System.Linq;

namespace SigSpec.CodeGeneration.Models
{
    public class OperationModel
    {
        private readonly SigSpecOperation _operation;

        public OperationModel(string name, SigSpecOperation operation, TypeResolverBase resolver)
        {
            _operation = operation;

            Name = name;
            ReturnType = operation.ReturnType != null ? new ReturnTypeModel(_operation.ReturnType, resolver) : null;
            Parameters = operation.Parameters.Select(o => new ParameterModel(o.Key, o.Value, resolver));
        }

        public string Name { get; }

        public string MethodName => ConversionUtilities.ConvertToLowerCamelCase(Name, true);

        public IEnumerable<ParameterModel> Parameters { get; }

        public ReturnTypeModel ReturnType { get; }

        public bool IsObservable => _operation.Type == SigSpecOperationType.Observable;

        public bool HasReturnType => ReturnType != null;
    }
}
