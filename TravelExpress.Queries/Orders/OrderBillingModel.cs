namespace TravelExpress.Queries.Orders
{
    using System;

    public class OrderBillingModel
    {
        public Guid BillId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDebit { get; set; }
        public int BillStatusId { get; set; }
        public string BillStatusDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime PaidDate { get; set; }
        public OrderBillingPaymentModel[] BillingPayments { get; set; }
        public OrderHistorizationModel[] Historization { get; set; }
    }
}
