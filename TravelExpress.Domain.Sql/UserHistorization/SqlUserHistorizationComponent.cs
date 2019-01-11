namespace TravelExpress.Domain.Sql.UserHistorization
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.UserHistorization;
    using TravelExpress.Enums.UserHistorization;

    public sealed class SqlUserHistorizationComponent : UserHistorizationComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdText = @"
            INSERT INTO [historization].[UserHistorization]
            ([UserActivityTypeId], [UserId], [Date], [Id])
            VALUES
            (@userActivityTypeId, @userId, @date, @Id)
        ";

        public override async Task AddUserActivity(Guid Id, UserActivityType userActivityType, DateTime date, Guid userId)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
                {
                    new SqlParameter("@Id", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = Id
                    },
                    new SqlParameter("@userActivityTypeId", System.Data.SqlDbType.Int)
                    {
                        Value = (int)userActivityType
                    },
                    new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = date
                    },
                    new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = userId
                    },
                };
            await commandHelper.ExecuteNonQueryAsync(_cmdText, false, parameters);
        }
    }
}
