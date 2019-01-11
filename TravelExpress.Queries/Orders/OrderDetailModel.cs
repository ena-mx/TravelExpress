namespace TravelExpress.Queries.Orders
{
    public class OrderDetailModel
    {
        public int OrderDetailId { get; set; }
        public int OrderDetailTypeId { get; set; }
        public string OrderDetailTypeDescription { get; set; }
        public string OrderDetailDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}
