namespace TravelExpress.Domain.Sql.Orders.Shared
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Orders;
    using TravelExpress.Enums.Orders;

    public class SqlOrderHistorization : OrderHistorization
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _addBillingHistorizationCmdTxt = @"
            INSERT INTO [historization].[BillHistorization]
            ([BillId], [BillStatusId], [UserId], [Date])
            VALUES
            (@billId, @billActivityType, @userId, @date)

            IF @billActivityType > 1
            BEGIN
	            UPDATE [billing].[Bill]
	            SET [BillStatusId] = @billActivityType
	            WHERE BillId = @billId
            END
        ";
        private static readonly string _addOrderHistorizationCmdTxt = @"
            INSERT INTO [historization].[OrderHistorization]
			([OrderId], [OrderStatusId], [UserId], [Date])
			VALUES
			(@orderId, @orderStatus, @userId, @date)

			IF @orderStatus > 1
			BEGIN
				UPDATE [orders].[Order]
				SET [OrderStatusId] = @orderStatus
				WHERE OrderId = @orderId
			END
        ";

        public override async Task AddBillingHistorizationAsync(Guid billId, BillActivityType billActivityType, DateTime serverDate, Guid userId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
            {
                new SqlParameter("@billId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = billId
                },
                new SqlParameter("@billActivityType", System.Data.SqlDbType.Int)
                {
                    Value = (int)billActivityType
                },
                new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = serverDate
                },
                new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = userId
                }
            };
            await commandHelper.ExecuteNonQueryAsync(_addBillingHistorizationCmdTxt, false, parameters);
        }

        public override async Task AddOrderHistorizationAsync(Guid orderId, OrderStatus orderStatus, DateTime serverDate, Guid userId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@orderStatus", System.Data.SqlDbType.Int)
                {
                    Value = (int)orderStatus
                },
                new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = serverDate
                },
                new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = userId
                }
            };
            await commandHelper.ExecuteNonQueryAsync(_addOrderHistorizationCmdTxt, false, parameters);
        }
    }
}
