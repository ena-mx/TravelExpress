namespace TravelExpress.Queries.Orders
{
    using System;

    public class OrderBillingPaymentModel
    {
        public int BillPaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
