namespace TravelExpress.Domain.Products
{
    using System;

    internal sealed class DeletedProductState : ProductState
    {
        public DeletedProductState(Guid productId, ProductComponent product, ProductStorage storage) : base(productId, product, storage)
        {
        }
    }
}
