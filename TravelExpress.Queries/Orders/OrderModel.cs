namespace TravelExpress.Queries.Orders
{
    using TravelExpress.Queries.Customers;

    public class OrderModel
    {
        public OrderHeaderModel OrderHeader { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public OrderDetailModel[] OrderDetails { get; set; }
        public OrderBillingModel BillingInfo { get; set; }
        public OrderHistorizationModel[] Historization { get; set; }
    }
}
