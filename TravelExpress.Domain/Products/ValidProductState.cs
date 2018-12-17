namespace TravelExpress.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    internal sealed class ValidProductState : ProductState
    {
        private static readonly KeyValuePair<WorkflowResult, Guid> _invalidParameters = new KeyValuePair<WorkflowResult, Guid>(new WorkflowResult(Resources.Resource.IncorrectParameters), Guid.Empty);
        private static readonly KeyValuePair<WorkflowResult, Guid> _endDateShouldBeGreterThanBeginDateError = new KeyValuePair<WorkflowResult, Guid>(new WorkflowResult(Resources.Resource.EndDateShouldBeGreterThanBeginDateError), Guid.Empty);
        private static readonly KeyValuePair<WorkflowResult, Guid> _beginDateShouldBeGratherThanTodayError = new KeyValuePair<WorkflowResult, Guid>(new WorkflowResult(Resources.Resource.BeginDateShouldBeGraterThanToday), Guid.Empty);
        private static readonly WorkflowResult _duplicateExcursionError = new WorkflowResult(Resources.Resource.DuplicateExcursionError);
        private static readonly WorkflowResult _duplicateCategoryError = new WorkflowResult(Resources.Resource.DuplicateCategoryError);
        public ValidProductState(Guid productId, ProductComponent product, ProductStorage storage) : base(productId, product, storage)
        {
        }

        public override async Task<WorkflowResult> AddAditionalServiceAsync(string description, decimal unitPrice)
        {
            if (!await _storage.AddAditionalServiceAsync(_productId, description, unitPrice))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> AddPriceCategoryAsync(string description)
        {
            if (await _storage.IsDuplicatePriceCategory(_productId, description))
                return _duplicateCategoryError;
            if (!await _storage.AddPriceCategoryAsync(_productId, description))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> AddPriceDetailAsync(int priceCategoryId, string description, decimal unitPrice)
        {
            if (!await _storage.AddPriceDetailAsync(_productId, priceCategoryId, description, unitPrice))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> DeleteAditionalServiceAsync(int aditionalServiceId)
        {
            if (!await _storage.DeleteAditionalServiceAsync(_productId, aditionalServiceId))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> DeleteAsync()
        {
            if (!await _storage.DeleteProductAsync(_productId))
                return _storageError;
            _product.State = new DeletedProductState(_productId, _product, _storage);
            return _successResult;
        }

        public override async Task<WorkflowResult> DeletePriceCategoryAsync(int categoryId)
        {
            if (!await _storage.DeletePriceCategoryAsync(_productId, categoryId))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> DeletePriceDetailAsync(int priceDetailId)
        {
            if (!await _storage.DeletePriceDetailAsync(_productId, priceDetailId))
                return _storageError;
            return _successResult;
        }

        public override async Task<WorkflowResult> UpdateDescriptionAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return _invalidDescriptionError;
            if (await _storage.IsDuplicateAsync(_productId, description))
                return _duplicateError;
            if (!await _storage.UpdateProductDescriptionAsync(_productId, description))
                return _storageError;
            return _successResult;
        }

        public override async Task<KeyValuePair<WorkflowResult, Guid>> AddExcursionAsync(DateTime beginDate, DateTime endDate, Guid userId)
        {
            if (beginDate == DateTime.MinValue || endDate == DateTime.MinValue || userId == Guid.Empty)
                return _invalidParameters;
            if (beginDate <= DateTime.Today)
                return _beginDateShouldBeGratherThanTodayError;
            if (beginDate >= endDate)
                return _endDateShouldBeGreterThanBeginDateError;
            if (await _storage.IsDuplicateExcursionAsync(_productId, beginDate.Date, endDate.Date))
                return new KeyValuePair<WorkflowResult, Guid>(_duplicateExcursionError, Guid.Empty);
            Guid excursionId = Guid.NewGuid();
            if (!await _storage.AddExcursionAsync(_productId, excursionId, beginDate.Date, endDate.Date))
                return new KeyValuePair<WorkflowResult, Guid>(_storageError, Guid.Empty);
            return new KeyValuePair<WorkflowResult, Guid>(_successResult, excursionId);
        }
    }
}
