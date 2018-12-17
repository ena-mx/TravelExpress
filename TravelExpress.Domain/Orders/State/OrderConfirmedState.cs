namespace TravelExpress.Domain.Orders.State
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.Component;
    using Enums.Orders;

    public sealed class OrderConfirmedState : OrderState
    {
        #region Private Members

        #region Workflow Results

        private WorkflowResult _successResult = new WorkflowResult();
        private WorkflowResult _orderHasPaymentsError = new WorkflowResult(Resources.Resource.OrderHasPaymentsError);

        #endregion Workflow Results

        #region Collaborators

        private readonly Order _order;
        private readonly OrderHistorization _orderHistorization;
        private readonly OrderStorage _orderStorage;
        private readonly IDateComponent _dateComponent;

        #endregion Collaborators

        #endregion Private Members

        #region Constructor

        public OrderConfirmedState(
            Order order,
            OrderHistorization orderHistorization,
            OrderStorage orderStorage,
            IDateComponent dateComponent)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _orderHistorization = orderHistorization ?? throw new ArgumentNullException(nameof(orderHistorization));
            _orderStorage = orderStorage;
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        #endregion Constructor

        #region Protocol

        public override async Task<WorkflowResult> AddPaymentAsync(decimal amount, Guid userId)
        {
            await _orderStorage.AddPaymentAsync(_order.OrderId, _order.BillId, amount, _dateComponent.ServerDate);
            await _orderHistorization.AddBillingHistorizationAsync(_order.BillId, BillActivityType.PaymentAdded, _dateComponent.ServerDate, userId);
            if (await _orderStorage.IsPaidAsync(_order.OrderId))
            {
                _order.OrderState = new OrderPayedState();
                await _orderHistorization.AddBillingHistorizationAsync(_order.BillId, BillActivityType.Payed, _dateComponent.ServerDate, userId);
                await _orderHistorization.AddOrderHistorizationAsync(_order.OrderId, OrderStatus.Payed, _dateComponent.ServerDate, userId);
            }
            return _successResult;
        }

        public override async Task<WorkflowResult> CancelAsync(Guid userId)
        {
            if (!await _orderStorage.HasPaymentsAsync(_order.OrderId))
                return _orderHasPaymentsError;
            await _orderStorage.CancelOrderAsync(_order.OrderId);
            await _orderHistorization.AddBillingHistorizationAsync(_order.BillId, BillActivityType.Cancelled, _dateComponent.ServerDate, userId);
            await _orderHistorization.AddOrderHistorizationAsync(_order.OrderId, OrderStatus.Cancelled, _dateComponent.ServerDate, userId);
            _order.OrderState = new OrderCancelledState();
            return _successResult;
        }

        #endregion Protocol
    }
}