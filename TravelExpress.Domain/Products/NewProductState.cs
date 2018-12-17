namespace TravelExpress.Domain.Products
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    internal sealed class NewProductState : ProductState
    {
        public NewProductState(Guid productId, ProductComponent product, ProductStorage storage) : base(productId, product, storage)
        {
        }

        public override async Task<WorkflowResult> SaveAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return _invalidDescriptionError;
            if (await _storage.IsDuplicateAsync(_productId, description))
                return _duplicateError;
            if (!await _storage.SaveProductAsync(_productId, description))
                return _storageError;
            _product.State = new ValidProductState(_productId, _product, _storage);
            return _successResult;
        }
    }
}