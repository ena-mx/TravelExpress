namespace TravelExpress.Queries.Analytics
{
    using System;
    using System.Threading.Tasks;

    public abstract class AnalyticsQueryComponent
    {
        public abstract Task<AnalyticsModel> AnalyticsModelAsync(DateTime dateUserActivity, DateTime dateActiveExcursions);
    }

    public class AnalyticsModel
    {
        public int ActiveExcursions { get; set; }
        public int OrderDelayPaymentsQuantity { get; set; }
        public ExcursionSalesIndicator[] ExcursionSalesIndicators { get; set; }
        public ExcursionSalesIndicator[] ExcursionPendingBillingIndicators { get; set; }
        public UserActivityModel[] UserActivities { get; set; }
        public LatestOrderModel[] LatestOrders { get; set; }
        public LatestCustomers[] LatestCustomers { get; set; }
    }

    public class LatestOrderModel
    {
        public Guid OrderId { get; set; }
        public string ExcursionDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExcursionDate { get; set; }
    }

    public class LatestCustomers
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ExcursionSalesIndicator
    {
        public Guid ExcursionId { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
    }
    public class UserActivityModel
    {
        public int UserActivityModuleId { get; set; }
        public string UserActivityModuleDescription { get; set; }
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}
