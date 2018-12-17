using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelExpenses.SharedFramework;

namespace TravelExpress.Domain.Orders.Component
{
    public interface IOrder
    {
        Guid BillId { get; }
        Guid CustomerId { get; }
        Guid OrderId { get; }
        Task<WorkflowResult> AddPaymentAsync(decimal amount, Guid userId);
        Task<WorkflowResult> CancelAsync(Guid userId);
        Task<WorkflowResult> CreateAsync(Guid excursionId, KeyValuePair<int, int>[] aditionalServices, KeyValuePair<int, int>[] orderDetails, Guid userId);
    }
}