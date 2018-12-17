namespace TravelExpress.Domain.Products
{
    using System;
    using System.Threading.Tasks;

    public abstract class ProductStorage
    {
        public abstract Task<bool> IsDuplicateAsync(Guid productId, string description);
        public abstract Task<bool> SaveProductAsync(Guid productId, string description);
        public abstract Task<bool> UpdateProductDescriptionAsync(Guid productId, string description);
        public abstract Task<bool> DeleteProductAsync(Guid productId);
        public abstract Task<bool> AddAditionalServiceAsync(Guid productId, string description, decimal unitPrice);
        public abstract Task<bool> IsDuplicatePriceCategory(Guid productId, string description);
        public abstract Task<bool> AddPriceCategoryAsync(Guid productId, string description);
        public abstract Task<bool> AddPriceDetailAsync(Guid productId, int priceCategoryId, string description, decimal unitPrice);
        public abstract Task<bool> DeleteAditionalServiceAsync(Guid productId, int aditionalServiceId);
        public abstract Task<bool> DeletePriceCategoryAsync(Guid productId, int categoryId);
        public abstract Task<bool> DeletePriceDetailAsync(Guid productId, int priceDetailId);
        public abstract Task<bool> AddExcursionAsync(Guid productId, Guid excursionId, DateTime beginDate, DateTime endDate);
        public abstract Task<bool> IsDuplicateExcursionAsync(Guid productId, DateTime beginDate, DateTime endDate);
    }
}
