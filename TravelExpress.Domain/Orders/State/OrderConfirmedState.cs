namespace TravelExpress.Domain.Orders.State
{
    using System;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.Component;
    using Enums.Orders;
    using TravelExpress.Domain.UserHistorization;

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
        private readonly UserHistorizationComponent _userHistorizationComponent;

        #endregion Collaborators

        #endregion Private Members

        #region Constructor

        public OrderConfirmedState(
            Order order,
            OrderHistorization orderHistorization,
            OrderStorage orderStorage,
            IDateComponent dateComponent,
            UserHistorizationComponent userHistorizationComponent)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _orderHistorization = orderHistorization ?? throw new ArgumentNullException(nameof(orderHistorization));
            _orderStorage = orderStorage;
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
            _userHistorizationComponent = userHistorizationComponent ?? throw new ArgumentNullException(nameof(userHistorizationComponent));
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
                await _userHistorizationComponent.AddUserActivity(_order.OrderId, Enums.UserHistorization.UserActivityType.OrderPayed, _dateComponent.ServerDate, userId);
            }
            else
            {
                await _userHistorizationComponent.AddUserActivity(_order.OrderId, Enums.UserHistorization.UserActivityType.OrderPaymentAdded, _dateComponent.ServerDate, userId);
            }
            return _successResult;
        }

        public override async Task<WorkflowResult> CancelAsync(Guid userId)
        {
            if (await _orderStorage.HasPaymentsAsync(_order.OrderId))
                return _orderHasPaymentsError;
            await _orderHistorization.AddBillingHistorizationAsync(_order.BillId, BillActivityType.Cancelled, _dateComponent.ServerDate, userId);
            await _orderHistorization.AddOrderHistorizationAsync(_order.OrderId, OrderStatus.Cancelled, _dateComponent.ServerDate, userId);
            await _userHistorizationComponent.AddUserActivity(_order.OrderId, Enums.UserHistorization.UserActivityType.OrderCancelled, _dateComponent.ServerDate, userId);
            _order.OrderState = new OrderCancelledState();
            return _successResult;
        }

        #endregion Protocol
    }
}