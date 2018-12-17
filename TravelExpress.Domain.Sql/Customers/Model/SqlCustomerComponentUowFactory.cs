namespace TravelExpress.Domain.Sql.Customers.Model
{
    using EnaBricks.DataBricks;
    using EnaBricks.Generics;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Customers.Model;
    using TravelExpress.Domain.Sql.Customers.Shared;

    public sealed class SqlCustomerComponentUowFactory : CustomerComponentUowFactory
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _existsCustomerCmdTxt = @"
            IF EXISTS(SELECT [CustomerId] FROM [management].[Customer] WITH(NOLOCK) WHERE [CustomerId] = @customerId)
            BEGIN
	            SELECT 1
            END
            ELSE
            BEGIN
	            SELECT 0
            END
        ";

        public override async Task<Option<ICustomerComponentUow>> ExistingCustomerUowAsync(Guid customerId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    }
                };

            bool exists = Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(_existsCustomerCmdTxt, false, parameters));

            if (!exists)
                return Option<ICustomerComponentUow>.None();

            return Option<ICustomerComponentUow>.Some(new CustomerComponentUow(
                        new CustomerComponent(
                            customerId,
                            new SqlCustomersStorage(),
                           new SqlCustomerHistorization(),
                           new CstMexDateComponent()
                    )));
        }

        public override ICustomerComponentUow NewCustomerUow()
        {
            return new CustomerComponentUow(
                        new CustomerComponent(
                            new SqlCustomersStorage(),
                           new SqlCustomerHistorization(),
                           new CstMexDateComponent()
                    ));
        }
    }
}
