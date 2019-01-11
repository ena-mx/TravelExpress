namespace TravelExpress.Queries.Orders
{
    using System;

    public class OrderHeaderModel
    {
        public Guid OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public string StatusDescription { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public Guid ExcursionId { get; set; }
        public string ExcursionDescription { get; set; }
        public DateTime ExcursionBeginDate { get; set; }
        public DateTime ExcursionEndDate { get; set; }
    }
}
