using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using TravelExpenses.SharedFramework;
using TravelExpress.Domain.Application;

namespace TravelExpress.Domain.Orders.Component
{
    public sealed class OrderDecorator : IOrder
    {
        private readonly IOrder _decorated;
        private readonly ApplicationLog _applicationLog;

        public Guid BillId => _decorated.BillId;

        public Guid CustomerId => _decorated.CustomerId;

        public Guid OrderId => _decorated.OrderId;

        public OrderDecorator(IOrder order, ApplicationLog applicationLog)
        {
            _decorated = order ?? throw new ArgumentNullException(nameof(order));
            _applicationLog = applicationLog ?? throw new ArgumentNullException(nameof(applicationLog));
        }

        public async Task<WorkflowResult> AddPaymentAsync(decimal amount, Guid userId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.AddPaymentAsync(amount, userId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    int errorId = await _applicationLog.LogExceptionAsync(ex);
                    return new WorkflowResult(
                        new[] {
                            Resources.Resource.InternalServerErrorMessage,
                            string.Format(Resources.Resource.GeneratedErrorIdMessage, errorId)
                        });
                }
            }
        }

        public async Task<WorkflowResult> CancelAsync(Guid userId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    WorkflowResult workflowResult = await _decorated.CancelAsync(userId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    int errorId = await _applicationLog.LogExceptionAsync(ex);
                    return new WorkflowResult(
                        new[] {
                            Resources.Resource.InternalServerErrorMessage,
                            string.Format(Resources.Resource.GeneratedErrorIdMessage, errorId)
                        });
                }
            }
        }

        public async Task<WorkflowResult> CreateAsync(
            Guid excursionId,
            KeyValuePair<int, int>[] aditionalServices,
            KeyValuePair<int, int>[] orderDetails,
            Guid userId)
        {
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Exception appException = null;
                try
                {
                    WorkflowResult workflowResult = await _decorated.CreateAsync(
                        excursionId,
                        aditionalServices,
                        orderDetails,
                        userId);

                    if (workflowResult.Success)
                        transactionScope.Complete();
                    else
                        Transaction.Current.Rollback();
                    return workflowResult;
                }
                catch (Exception ex)
                {
                    Transaction.Current.Rollback();
                    if (ex != null)
                        appException = ex;
                }

                int errorId = 0;

                if (appException != null)
                {
                    errorId = await _applicationLog.LogExceptionAsync(appException);
                }

                if (errorId > 0)
                {
                    return new WorkflowResult(
                            new[] {
                                Resources.Resource.InternalServerErrorMessage,
                                string.Format(Resources.Resource.GeneratedErrorIdMessage, errorId)
                            });
                }

                return new WorkflowResult(
                            new[] {
                                Resources.Resource.InternalServerErrorMessage
                            });

            }
        }
    }
}