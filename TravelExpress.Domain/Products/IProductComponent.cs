using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelExpenses.SharedFramework;

namespace TravelExpress.Domain.Products
{
    public interface IProductComponent
    {
        Guid ProductId { get; }
        Task<WorkflowResult> AddAditionalServiceAsync(string description, decimal unitPrice);
        Task<WorkflowResult> AddPriceCategoryAsync(string description);
        Task<WorkflowResult> AddPriceDetailAsync(int priceCategoryId, string description, decimal unitPrice);
        Task<WorkflowResult> DeleteAditionalServiceAsync(int aditionalServiceId);
        Task<WorkflowResult> DeleteAsync();
        Task<WorkflowResult> DeletePriceCategoryAsync(int categoryId);
        Task<WorkflowResult> DeletePriceDetailAsync(int priceDetailId);
        Task<WorkflowResult> SaveAsync(string description);
        Task<WorkflowResult> UpdateDescriptionAsync(string description);
        Task<KeyValuePair<WorkflowResult, Guid>> AddExcursionAsync(DateTime beginDate, DateTime endDate, Guid userId);
    }
}