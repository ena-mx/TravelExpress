namespace TravelExpress.Domain.Sql.Orders.Shared
{
    using EnaBricks.DataBricks;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Orders;

    public sealed class SqlOrderStorage : OrderStorage
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _addBillingCmdTxt = @"";
        private static readonly string _addOrderDetailsCmdTxt = @"";
        private static readonly string _addPaymentCmdTxt = @"";
        private static readonly string _cancelOrderCmdTxt = @"";
        private static readonly string _createOrderHeaderCmdTxt = @"";
        private static readonly string _hasPaymentsCmdTxt = @"";
        private static readonly string _isPaidCmdTxt = @"";


        public override async Task<bool> AddBillingAsync(Guid orderId, Guid billId, Guid customerId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[3]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@billId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = billId
                },
                new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = customerId
                }
            };
            return await commandHelper.ExecuteNonQueryAsync(_addBillingCmdTxt, false, parameters) > 0;
        }

        public override async Task<bool> AddOrderDetailsAsync(Guid orderId, KeyValuePair<int, int>[] aditionalServices, KeyValuePair<int, int>[] orderDetails)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[3]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@aditionalServices", System.Data.SqlDbType.VarChar)
                {
                    Value = aditionalServices
                },
                new SqlParameter("@orderDetails", System.Data.SqlDbType.VarChar)
                {
                    Value = orderDetails
                }
            };
            return await commandHelper.ExecuteNonQueryAsync(_addOrderDetailsCmdTxt, false, parameters) > 0;
        }

        public override async Task AddPaymentAsync(Guid orderId, Guid billId, decimal amount, DateTime serverDate)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@billId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = billId
                },
                new SqlParameter("@amount", System.Data.SqlDbType.Money)
                {
                    Value = amount
                },
                new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = serverDate
                }
            };
            await commandHelper.ExecuteNonQueryAsync(_addPaymentCmdTxt, false, parameters);
        }

        public override async Task CancelOrderAsync(Guid orderId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                }
            };
            await commandHelper.ExecuteNonQueryAsync(_cancelOrderCmdTxt, false, parameters);
        }

        public override async Task<bool> CreateOrderHeaderAsync(Guid orderId, Guid customerId, Guid excursionId, DateTime serverDate)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = customerId
                },
                new SqlParameter("@excursionId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = excursionId
                },
                new SqlParameter("@serverDate", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = serverDate
                }
            };
            return await commandHelper.ExecuteNonQueryAsync(_createOrderHeaderCmdTxt, false, parameters) > 0;
        }

        public override async Task<bool> HasPaymentsAsync(Guid orderId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                }
            };
            return Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(_hasPaymentsCmdTxt, false, parameters));
        }

        public override async Task<bool> IsPaidAsync(Guid orderId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[1]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                }
            };
            return Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(_isPaidCmdTxt, false, parameters));
        }
    }
}
