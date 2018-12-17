namespace TravelExpress.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    internal abstract class ProductState
    {
        protected readonly ProductStorage _storage;
        protected readonly Guid _productId;
        protected readonly ProductComponent _product;
        protected static readonly WorkflowResult _invalidOperationError = new WorkflowResult(Resources.Resource.InvalidOperation);

        protected static readonly WorkflowResult _successResult = new WorkflowResult();
        protected static readonly WorkflowResult _duplicateError = new WorkflowResult(Resources.Resource.ProductComponent_DuplicateProduct);
        protected static readonly WorkflowResult _storageError = new WorkflowResult(Resources.Resource.ProductComponent_ErrorSaveNewProduct);
        protected static readonly WorkflowResult _invalidDescriptionError = new WorkflowResult(Resources.Resource.ProductComponent_ErrorInvalidDescription);
        private static readonly KeyValuePair<WorkflowResult, Guid> _addExcursionInvalidOperationError = new KeyValuePair<WorkflowResult, Guid>(_invalidOperationError, Guid.Empty);

        protected ProductState(Guid productId, ProductComponent product, ProductStorage storage)
        {
            _productId = productId;
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }

        public virtual Task<WorkflowResult> UpdateDescriptionAsync(string description) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> SaveAsync(string description) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> DeleteAsync() => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> AddAditionalServiceAsync(string description, decimal unitPrice) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> AddPriceCategoryAsync(string description) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> AddPriceDetailAsync(int priceCategoryId, string description, decimal unitPrice) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> DeleteAditionalServiceAsync(int aditionalServiceId) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> DeletePriceCategoryAsync(int categoryId) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> DeletePriceDetailAsync(int priceDetailId) => Task.FromResult(_invalidOperationError);
        public virtual Task<KeyValuePair<WorkflowResult, Guid>> AddExcursionAsync(DateTime beginDate, DateTime endDate, Guid userId) => Task.FromResult(_addExcursionInvalidOperationError);
    }
}
