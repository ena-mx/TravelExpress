namespace TravelExpress.Queries.Excursions
{
    using System;

    public class ExcursionIndexModel
    {
        public Guid ExcursionId { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ActiveOrdersCount { get; set; }
        public int PayedOrdersCount { get; set; }
    }
}