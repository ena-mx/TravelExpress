namespace TravelExpress.Domain.Sql.Customers.Shared
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public sealed class SqlCustomerHistorization : Domain.Customers.Shared.CustomerHistorization
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _addCustomerActivityCmdText = @"
            INSERT INTO [historization].[CustomerHistorization]
            ([CustomerId], [UserId], [CustomerActivityTypeId], [Date])
            VALUES
            (@customerId, @userId, @customerActivityType, @date)
        ";

        public override async Task AddCustomerActivity(Guid customerId, Enums.Customers.CustomerActivityType customerActivityType, DateTime date, Guid userId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = userId
                    },
                    new SqlParameter("@customerActivityType", System.Data.SqlDbType.Int)
                    {
                        Value = (int)customerActivityType
                    },
                    new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = date
                    }
                };
            await commandHelper.ExecuteNonQueryAsync(_addCustomerActivityCmdText, false, parameters);
        }
    }
}
