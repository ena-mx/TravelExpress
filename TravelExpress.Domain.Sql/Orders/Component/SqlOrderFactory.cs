namespace TravelExpress.Domain.Sql.Orders.Component
{
    using EnaBricks.DataBricks;
    using EnaBricks.Generics;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Application;
    using TravelExpress.Domain.Orders;
    using TravelExpress.Domain.Orders.Component;
    using TravelExpress.Domain.Orders.State;
    using TravelExpress.Enums.Orders;

    public sealed class SqlOrderFactory : OrderFactory
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _existsOrder = @"
            IF EXISTS(SELECT [OrderId] FROM [orders].[Order] WITH(NOLOCK) WHERE [OrderId] = @orderId)
            BEGIN
	            SELECT 1
            END
            ELSE
            BEGIN
	            SELECT 0
            END
        ";

        public static readonly string _orderInfoCmdText = @"
            SELECT [Order].[CustomerId], [Order].[OrderStatusId], [Bill].[BillId]
            FROM [orders].[Order] WITH(NOLOCK)
            JOIN [billing].[Bill] WITH(NOLOCK) ON [Bill].[OrderId] = [Order].[OrderId]
            WHERE [Order].[OrderId] = @orderId
        ";


        private ApplicationLog _applicationLog;
        private OrderHistorization _orderHistorization;
        private OrderStorage _orderStorage;
        private IDateComponent _dateComponent;

        public SqlOrderFactory(
            ApplicationLog applicationLog,
            OrderHistorization orderHistorization,
            OrderStorage orderStorage,
            IDateComponent dateComponent)
        {
            _applicationLog = applicationLog ?? throw new ArgumentNullException(nameof(applicationLog));
            _orderHistorization = orderHistorization ?? throw new ArgumentNullException(nameof(orderHistorization));
            _orderStorage = orderStorage ?? throw new ArgumentNullException(nameof(orderStorage));
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }

        public override IOrder NewOrder(Guid customerId)
        {
            return new OrderDecorator(
                        new Order(
                            customerId,
                            _orderHistorization,
                            _orderStorage,
                            _dateComponent),
                        _applicationLog);
        }

        public override async Task<Option<IOrder>> OrderAsync(Guid orderId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                }
            };

            bool exists = Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(_existsOrder, false, parameters));

            commandHelper = null;

            if (!exists)
                return Option<IOrder>.None();

            Guid billId = Guid.Empty;
            Guid customerId = Guid.Empty;
            OrderStatus orderStatusId = OrderStatus.Confirmed;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(_orderInfoCmdText, connection))
                {
                    command.Parameters.AddRange(parameters);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                customerId = reader.GetGuid(0);
                                orderStatusId = (OrderStatus)reader.GetInt32(1);
                                billId = reader.GetGuid(2);
                            }
                        }
                    }
                }
            }

            switch (orderStatusId)
            {
                case OrderStatus.Confirmed:
                    {
                        return Option<IOrder>.Some(new OrderDecorator(
                            new Order(
                                orderId,
                                billId,
                                customerId,
                                _orderHistorization,
                                _orderStorage,
                                _dateComponent),
                            _applicationLog));
                    }
                case OrderStatus.Payed:
                    {
                        return Option<IOrder>.Some(
                            new Order(
                                orderId,
                                new OrderPayedState()
                            ));
                    }
                case OrderStatus.Cancelled:
                    {
                        return Option<IOrder>.Some(
                            new Order(
                                orderId,
                                new OrderCancelledState()
                            ));
                    }
                default:
                    {
                        return Option<IOrder>.None();
                    }
            }
        }
    }
}
