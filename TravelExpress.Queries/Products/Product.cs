namespace TravelExpress.Queries.Products
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;

    public abstract class ProductQueryComponent
    {
        public abstract Task<GenericPage<ProductIndex>> ProductsAsync(int offset, int limit);
        public abstract Task<Product> ProductAsync(Guid productId);
        public abstract Task<ProductExcursionIndex[]> ProductExcursionsAsync(DateTime date);
        public abstract Task<ExcursionDetail> ExcursionItemAsync(Guid excursionId);
    }

    public class ProductIndex
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
    }

    public sealed class Product : ProductIndex
    {
        public ProductAditionalService[] AditionalServices { get; set; }
        public ProductPriceCategory[] PriceCategories { get; set; }
        public ExcursionDate[] ExcursionDates { get; set; }
    }

    public class ProductAditionalService
    {
        public int ProductAditionalServiceId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ProductPriceCategory
    {
        public int ProductPriceCategoryId { get; set; }
        public string Description { get; set; }
        public ProductPriceDetail[] PriceDetails { get; set; }
    }

    public class ProductPriceDetail
    {
        public int ProductPriceDetailId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ProductExcursionIndex
    {
        public string Description { get; set; }
        public ExcursionDate[] ExcursionDates { get; set; }
    }

    public class ExcursionDate
    {
        public Guid ExcursionId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ExcursionDetail : ExcursionDate
    {
        public string Description { get; set; }
        public AditionalService[] AditionalServices { get; set; }
        public PriceCategory[] PriceCategories { get; set; }
    }

    public class AditionalService
    {
        public int AditionalServiceId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PriceCategory
    {
        public int PriceCategoryId { get; set; }
        public string Description { get; set; }
        public PriceDetail[] PriceDetails { get; set; }
    }

    public class PriceDetail
    {
        public int PriceDetailId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
