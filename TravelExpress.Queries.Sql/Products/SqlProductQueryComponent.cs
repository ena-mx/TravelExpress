namespace TravelExpress.Queries.Sql.Products
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;
    using TravelExpress.Queries.Products;

    public sealed class SqlProductQueryComponent : ProductQueryComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdTxtProductPage = "[catalogs].[sp_ProductsPage]";
        private readonly string _cmdTxtProduct = "[catalogs].[sp_Product]";
        private readonly string _cmdTxtProductExcursions = "[catalogs].[sp_ProductExcursions]";
        private readonly string _cmdTxtExcursionItem = "[travel].[sp_Excursion]";
        private readonly string _rootName = "GenericPage";

        public override async Task<ExcursionDetail> ExcursionItemAsync(Guid excursionId)
        {
            using (SqlDeserializerComponent<ExcursionDetail> component = new SqlDeserializerComponent<ExcursionDetail>(
               _connectionString,
               _cmdTxtExcursionItem,
               new SqlParameter[]
               {
                    new SqlParameter("@excursionId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = excursionId
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }

        public override async Task<Product> ProductAsync(Guid productId)
        {
            using (SqlDeserializerComponent<Product> component = new SqlDeserializerComponent<Product>(
               _connectionString,
               _cmdTxtProduct,
               new SqlParameter[]
               {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }

        public override async Task<ProductExcursionIndex[]> ProductExcursionsAsync(DateTime date)
        {
            using (SqlDeserializerComponent<ProductExcursionIndex[]> component = new SqlDeserializerComponent<ProductExcursionIndex[]>(
               _connectionString,
               _cmdTxtProductExcursions,
               "ProductExcursionCollection",
               new SqlParameter[1]
               {
                    new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = date
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }

        public override async Task<GenericPage<ProductIndex>> ProductsAsync(int offset, int limit)
        {
            using (SqlDeserializerComponent<GenericPage<ProductIndex>> component = new SqlDeserializerComponent<GenericPage<ProductIndex>>(
              _connectionString,
              _cmdTxtProductPage,
              _rootName,
              new SqlParameter[]
              {
                    new SqlParameter("@offset", System.Data.SqlDbType.Int)
                    {
                        Value = offset
                    },
                    new SqlParameter("@limit", System.Data.SqlDbType.Int)
                    {
                        Value = limit
                    }
              }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }
    }
}
