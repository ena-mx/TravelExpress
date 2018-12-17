namespace TravelExpress.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    public sealed class ProductComponent : IProductComponent
    {
        #region Private members

        private readonly Guid _productId; 

        #endregion Private members

        #region Constructor

        public ProductComponent(ProductStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            _productId = Guid.NewGuid();
            State = new NewProductState(_productId, this, storage);
        }

        public ProductComponent(Guid productId, ProductStorage storage)
        {
            _productId = productId;
            State = new ValidProductState(_productId, this, storage);
        }

        #endregion Constructor

        #region Properties

        internal ProductState State { get; set; }

        public Guid ProductId => _productId;

        #endregion Properties

        #region Protocol

        public async Task<WorkflowResult> AddAditionalServiceAsync(string description, decimal unitPrice)
         => await State.AddAditionalServiceAsync(description, unitPrice);

        public async Task<WorkflowResult> AddPriceCategoryAsync(string description)
            => await State.AddPriceCategoryAsync(description);

        public async Task<WorkflowResult> AddPriceDetailAsync(int priceCategoryId, string description, decimal unitPrice)
            => await State.AddPriceDetailAsync(priceCategoryId, description, unitPrice);

        public async Task<WorkflowResult> DeleteAditionalServiceAsync(int aditionalServiceId)
            => await State.DeleteAditionalServiceAsync(aditionalServiceId);

        public async Task<WorkflowResult> DeleteAsync()
            => await State.DeleteAsync();

        public async Task<WorkflowResult> DeletePriceCategoryAsync(int categoryId)
        => await State.DeletePriceCategoryAsync(categoryId);

        public async Task<WorkflowResult> DeletePriceDetailAsync(int priceDetailId)
            => await State.DeletePriceDetailAsync(priceDetailId);

        public async Task<WorkflowResult> SaveAsync(string description)
            => await State.SaveAsync(description);

        public async Task<WorkflowResult> UpdateDescriptionAsync(string description)
            => await State.UpdateDescriptionAsync(description);

        public async Task<KeyValuePair<WorkflowResult, Guid>> AddExcursionAsync(DateTime beginDate, DateTime endDate, Guid userId)
            => await State.AddExcursionAsync(beginDate, endDate, userId);

        #endregion Protocol
    }
}
