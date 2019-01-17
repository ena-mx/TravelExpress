namespace TravelExpress.Queries.Excursions
{
    using TravelExpress.Queries.Customers;
    using TravelExpress.Queries.Products;

    public class ExcursionDataModel : ExcursionIndexModel
    {
        public OrderModel[] ActiveOrders { get; set; }
        public OrderModel[] PayedOrders { get; set; }
        public ProductAditionalService[] AditionalServices { get; set; }
        public ProductPriceCategory[] PriceCategories { get; set; }
    }
}
