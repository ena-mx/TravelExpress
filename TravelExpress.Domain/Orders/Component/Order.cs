namespace TravelExpress.Domain.Orders.Component
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.State;
    using TravelExpress.Domain.UserHistorization;

    public sealed class Order : IOrder
    {
        private readonly Guid _orderId;
        private readonly Guid _billId;
        private readonly Guid _customerId;

        internal OrderState OrderState { get; set; }

        public Guid OrderId => _orderId;
        public Guid BillId => _billId;
        public Guid CustomerId => _customerId;

        public Order(
            Guid customerId,
            OrderHistorization orderHistorization, 
            OrderStorage orderStorage,
            IDateComponent dateComponent,
            UserHistorizationComponent userHistorizationComponent)
        {
            if (orderHistorization == null)
            {
                throw new ArgumentNullException(nameof(orderHistorization));
            }

            if (orderStorage == null)
            {
                throw new ArgumentNullException(nameof(orderStorage));
            }

            if (dateComponent == null)
            {
                throw new ArgumentNullException(nameof(dateComponent));
            }

            if (userHistorizationComponent == null)
            {
                throw new ArgumentNullException(nameof(userHistorizationComponent));
            }

            _orderId = Guid.NewGuid();
            _billId = Guid.NewGuid();
            _customerId = customerId;
            OrderState = new OrderOpenState(this, orderHistorization, orderStorage, dateComponent, userHistorizationComponent);
        }

        public Order(
            Guid orderId,
            Guid billId,
            Guid customerId,
            OrderHistorization orderHistorization,
            OrderStorage orderStorage,
            IDateComponent dateComponent,
            UserHistorizationComponent userHistorizationComponent)
        {
            if (orderHistorization == null)
            {
                throw new ArgumentNullException(nameof(orderHistorization));
            }

            if (orderStorage == null)
            {
                throw new ArgumentNullException(nameof(orderStorage));
            }

            if (dateComponent == null)
            {
                throw new ArgumentNullException(nameof(dateComponent));
            }

            if (userHistorizationComponent == null)
            {
                throw new ArgumentNullException(nameof(userHistorizationComponent));
            }

            _orderId = orderId;
            _billId = billId;
            _customerId = customerId;
            OrderState = new OrderConfirmedState(this, orderHistorization, orderStorage, dateComponent, userHistorizationComponent);
        }

        public Order(
            Guid orderId,
            OrderPayedState orderState)
        {
            _orderId = orderId;
            _billId = Guid.Empty;
            _customerId = Guid.Empty;
            OrderState = orderState ?? throw new ArgumentNullException(nameof(orderState));
        }

        public Order(
            Guid orderId,
            OrderCancelledState orderState)
        {
            _orderId = orderId;
            _billId = Guid.Empty;
            _customerId = Guid.Empty;
            OrderState = orderState ?? throw new ArgumentNullException(nameof(orderState));
        }

        public async Task<WorkflowResult> CreateAsync(
            Guid excursionId,
            KeyValuePair<int, int>[] aditionalServices,
            KeyValuePair<int, int>[] orderDetails,
            Guid userId) => await OrderState.CreateAsync(excursionId, aditionalServices, orderDetails, userId);

        public async Task<WorkflowResult> AddPaymentAsync(decimal amount, Guid userId) => await OrderState.AddPaymentAsync(amount, userId);
        public async Task<WorkflowResult> CancelAsync(Guid userId) => await OrderState.CancelAsync(userId);
    }
}