using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using SigSpec.CodeGeneration.Models;
using SigSpec.Core;

namespace SigSpec.CodeGeneration.JavaScript.Models
{
    public class OperationModel
    {
        private readonly SigSpecOperation _operation;

        public OperationModel(string name, SigSpecOperation operation, TypeResolverBase resolver)
        {
            this._operation = operation;

            this.Name = name;
            this.ReturnType = operation.ReturnType != null ? new ReturnTypeModel(this._operation.ReturnType, resolver) : null;
            this.Parameters = operation.Parameters.Select(o => new ParameterModel(o.Key, o.Value, resolver));
        }

        public string Name { get; }
        public string NameCamelCase => this.Name.ToCamelCase();
        public string NameConstCase => this.Name.ToConstCase();

        public string MethodName => ConversionUtilities.ConvertToLowerCamelCase(this.Name, true);

        public IEnumerable<ParameterModel> Parameters { get; }

        public ReturnTypeModel ReturnType { get; }

        public bool IsObservable => this._operation.Type == SigSpecOperationType.Observable;

        public bool HasReturnType => this.ReturnType != null;
    }
}
