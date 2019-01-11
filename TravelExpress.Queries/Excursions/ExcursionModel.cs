using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TravelExpress.Queries.Common;
using TravelExpress.Queries.Customers;
using TravelExpress.Queries.Products;

namespace TravelExpress.Queries.Excursions
{
    public abstract class ExcursionQueryComponent
    {
        public abstract Task<ExcursionDataModel> FindAsync(Guid excursionId);
        public abstract Task<GenericPage<ExcursionIndexModel>> PageAsync(int offset, int limit, string searchValue);

    }

    public class ExcursionIndexModel
    {
        public Guid ExcursionId { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ExcursionDataModel : ExcursionIndexModel
    {
        public OrderModel[] ActiveOrders { get; set; }
        public OrderModel[] PayedOrders { get; set; }
        public ProductAditionalService[] AditionalServices { get; set; }
        public ProductPriceCategory[] PriceCategories { get; set; }
    }
}
