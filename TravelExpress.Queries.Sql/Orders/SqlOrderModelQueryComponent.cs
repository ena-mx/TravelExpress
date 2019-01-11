namespace TravelExpress.Queries.Sql.Orders
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Orders;

    public sealed class SqlOrderModelQueryComponent : OrderModelQueryComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdTxtOrderDetail = "[orders].[sp_OrderDetail]";

        public override async Task<OrderModel> OrderAsync(Guid orderId)
        {
            using (SqlDeserializerComponent<OrderModel> component = new SqlDeserializerComponent<OrderModel>(
               _connectionString,
               _cmdTxtOrderDetail,
               new SqlParameter[]
               {
                    new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = orderId
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }
    }
}
