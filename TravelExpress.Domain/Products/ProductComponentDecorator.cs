namespace TravelExpress.Domain.Products
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Transactions;
    using TravelExpenses.SharedFramework;

    public sealed class ProductComponentDecorator : IProductComponent
    {
        private readonly IProductComponent _decorated;
        public Guid ProductId => _decorated.ProductId;

        public ProductComponentDecorator(IProductComponent decorated)
        {
            _decorated = decorated;
        }

        public async Task<WorkflowResult> AddAditionalServiceAsync(string description, decimal unitPrice)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.AddAditionalServiceAsync(description, unitPrice);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> AddPriceCategoryAsync(string description)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.AddPriceCategoryAsync(description);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> AddPriceDetailAsync(int priceCategoryId, string description, decimal unitPrice)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.AddPriceDetailAsync(priceCategoryId, description, unitPrice);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> DeleteAditionalServiceAsync(int aditionalServiceId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.DeleteAditionalServiceAsync(aditionalServiceId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> DeleteAsync()
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.DeleteAsync();

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> DeletePriceCategoryAsync(int categoryId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.DeletePriceCategoryAsync(categoryId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> DeletePriceDetailAsync(int priceDetailId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.DeletePriceDetailAsync(priceDetailId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> SaveAsync(string description)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.SaveAsync(description);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<WorkflowResult> UpdateDescriptionAsync(string description)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.UpdateDescriptionAsync(description);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }

        public async Task<KeyValuePair<WorkflowResult, Guid>> AddExcursionAsync(DateTime beginDate, DateTime endDate, Guid userId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    KeyValuePair<WorkflowResult, Guid> workflowResult = await _decorated.AddExcursionAsync(beginDate, endDate, userId);

                    if (workflowResult.Key.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch
                {
                    Transaction.Current.Rollback();
                    throw;
                }
            }
        }
    }
}
