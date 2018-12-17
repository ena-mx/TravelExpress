﻿namespace TravelExpress.Domain.Orders.State
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Orders.Component;
    using TravelExpress.Enums.Orders;

    public sealed class OrderOpenState : OrderState
    {
        #region Private members

        #region Workflow Results

        private WorkflowResult _successResult = new WorkflowResult();
        private WorkflowResult _createOrderHeaderError = new WorkflowResult(Resources.Resource.CreateOrderHeaderError);
        private WorkflowResult _createOrderDetailsError = new WorkflowResult(Resources.Resource.CreateOrderDetailsError);
        private WorkflowResult _createOrderAddBillingError = new WorkflowResult(Resources.Resource.CreateOrderAddBillingError);

        #endregion Workflow Results

        #region Collaborators

        private readonly Order _order;
        private readonly OrderHistorization _orderHistorization;
        private readonly OrderStorage _orderStorage;
        private readonly IDateComponent _dateComponent;

        #endregion Collaborators 

        #endregion Private members

        #region Constructor

        public OrderOpenState(
            Order order,
            OrderHistorization orderHistorization,
            OrderStorage orderStorage,
            IDateComponent dateComponent)
        {
            _order = order;
            _orderHistorization = orderHistorization ?? throw new ArgumentNullException(nameof(orderHistorization));
            _orderStorage = orderStorage;
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        #endregion Constructor

        #region Protocol

        public override async Task<WorkflowResult> CreateAsync(
            Guid excursionId,
            KeyValuePair<int, int>[] aditionalServices,
            KeyValuePair<int, int>[] orderDetails,
            Guid userId)
        {
            if (await _orderStorage.CreateOrderHeaderAsync(
                _order.OrderId, _order.CustomerId, excursionId, _dateComponent.ServerDate))
                return _createOrderHeaderError;

            await _orderHistorization.AddOrderHistorizationAsync(
                _order.OrderId, OrderStatus.Confirmed, _dateComponent.ServerDate, userId);

            if (!await _orderStorage.AddOrderDetailsAsync(
                _order.OrderId,
                aditionalServices,
                orderDetails))
                return _createOrderDetailsError;

            if (!await _orderStorage.AddBillingAsync(_order.OrderId, _order.BillId, _order.CustomerId))
                return _createOrderAddBillingError;

            await _orderHistorization.AddBillingHistorizationAsync(_order.BillId, BillActivityType.BillCreated, _dateComponent.ServerDate, userId);

            _order.OrderState = new OrderConfirmedState(_order, _orderHistorization, _orderStorage, _dateComponent);

            return _successResult;
        }

        #endregion Protocol
    }
}