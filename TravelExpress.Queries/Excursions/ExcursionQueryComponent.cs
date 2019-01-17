namespace TravelExpress.Queries.Excursions
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;

    public abstract class ExcursionQueryComponent
    {
        public abstract Task<ExcursionDataModel> FindAsync(Guid excursionId);
        public abstract Task<GenericPage<ExcursionIndexModel>> PageAsync(int offset, int limit, string searchValue);

    }
}
