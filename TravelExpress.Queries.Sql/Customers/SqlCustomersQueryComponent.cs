namespace TravelExpress.Queries.Sql.Customers
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;
    using TravelExpress.Queries.Customers;

    public sealed class SqlCustomersQueryComponent : CustomersQueryComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdTxtCustomersPage = "[management].[sp_CustomersPage]";
        private readonly string _cmdTxtCustomer = "[management].[sp_Customer]";
        private readonly string _rootName = "GenericPage";

        public override async Task<GenericPage<CustomerIndex>> CustomersAsync(int offset, int limit, string searchValue)
        {
            using (SqlDeserializerComponent<GenericPage<CustomerIndex>> component = new SqlDeserializerComponent<GenericPage<CustomerIndex>>(
               _connectionString,
               _cmdTxtCustomersPage,
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
                    },
                    new SqlParameter("@searchValue", System.Data.SqlDbType.VarChar)
                    {
                        Value = searchValue ?? string.Empty
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }

        public override async Task<Customer> CustomerAsync(Guid customerId)
        {
            using (SqlDeserializerComponent<Customer> component = new SqlDeserializerComponent<Customer>(
               _connectionString,
               _cmdTxtCustomer,
               new SqlParameter[]
               {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }
    }
}