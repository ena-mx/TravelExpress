namespace TravelExpress.Domain.Orders.State
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;

    public abstract class OrderState
    {
        protected static readonly WorkflowResult _invalidOperationError = new WorkflowResult(Resources.Resource.InvalidOperation);

        public virtual Task<WorkflowResult> CreateAsync(
            Guid excursionId,
            KeyValuePair<int, int>[] aditionalServices,
            KeyValuePair<int, int>[] orderDetails,
            Guid userId) => Task.FromResult(_invalidOperationError);

        public virtual Task<WorkflowResult> AddPaymentAsync(decimal amount, Guid userId) => Task.FromResult(_invalidOperationError);
        public virtual Task<WorkflowResult> CancelAsync(Guid userId) => Task.FromResult(_invalidOperationError);
    }
}