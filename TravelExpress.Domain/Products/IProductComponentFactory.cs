namespace TravelExpress.Domain.Products
{
    using EnaBricks.Generics;
    using System;
    using System.Threading.Tasks;

    public interface IProductComponentFactory
    {
        IProductComponent New();
        Task<Option<IProductComponent>> CurrentAsync(Guid id);
    }
}
