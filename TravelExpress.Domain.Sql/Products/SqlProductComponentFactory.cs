namespace TravelExpress.Domain.Sql.Products
{
    using EnaBricks.DataBricks;
    using EnaBricks.Generics;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Products;

    public sealed class SqlProductComponentFactory : IProductComponentFactory
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _existsProductCmdTxt = @"
            IF EXISTS(SELECT [ProductId] FROM [catalogs].[Product] WITH(NOLOCK) WHERE [ProductId] = @productId)
            BEGIN
	            SELECT 1
            END
            ELSE
            BEGIN
	            SELECT 0
            END
        ";
        public async Task<Option<IProductComponent>> CurrentAsync(Guid id)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = id
                    }
                };

            bool exists = Convert.ToBoolean(await commandHelper.ExecuteNonQueryAsync(_existsProductCmdTxt, false, parameters));

            if (!exists)
                return Option<IProductComponent>.None();
            
            return Option<IProductComponent>.Some(
                    new ProductComponentDecorator(
                        new ProductComponent(id, new SqlProductStorage())
                    )
                );
        }

        public IProductComponent New()
        {
            return new ProductComponentDecorator(
                    new ProductComponent(new SqlProductStorage())
                );
        }
    }
}