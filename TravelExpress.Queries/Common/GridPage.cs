namespace TravelExpress.Queries.Common
{
    public sealed class GenericPage<T>
    {
        public PageInfo PageInfo { get; set; }
        public T[] Items { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return PageInfo != null && (PageInfo.PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageInfo != null && (PageInfo.PageNumber < PageInfo.TotalPages);
            }
        }
    }
}
