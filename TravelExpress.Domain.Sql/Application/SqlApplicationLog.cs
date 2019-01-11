namespace TravelExpress.Domain.Sql.Application
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpenses.SharedFramework;
    using TravelExpress.Domain.Application;

    public sealed class SqlApplicationLog : ApplicationLog
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _cmdText = @"
            INSERT INTO [AppInsights].[ErrorLog]
            ([Exception], [InnerException], [StackTrace], [Date])
            VALUES
            (@Exception, @InnerException, @StackTrace, @ServerDate)

            SELECT @@IDENTITY
        ";
        private readonly IDateComponent _dateComponent;

        public SqlApplicationLog(IDateComponent dateComponent)
        {
            _dateComponent = dateComponent ?? throw new ArgumentNullException(nameof(dateComponent));
        }
        public override async Task<int> LogExceptionAsync(Exception exception)
        {
            if (exception == null)
                return -1;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4]
                {
                new SqlParameter("@Exception", System.Data.SqlDbType.VarChar)
                {
                    Value = exception.Message ?? string.Empty
                },
                new SqlParameter("@InnerException", System.Data.SqlDbType.VarChar)
                {
                    Value = exception.InnerException?.Message ?? string.Empty
                },
                new SqlParameter("@StackTrace", System.Data.SqlDbType.VarChar)
                {
                    Value = exception.StackTrace ?? string.Empty
                },
                new SqlParameter("@ServerDate", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = _dateComponent.ServerDate
                },
                };

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(_cmdText, conn))
                    {
                        command.Parameters.AddRange(parameters);

                        await conn.OpenAsync();

                        int errorId = await command.ExecuteNonQueryAsync();

                        return errorId;
                    }
                }


                //SqlCommandHelper helper = new SqlCommandHelper(_connectionString);

                //return Convert.ToInt32(await helper.ExecuteScalarAsync(_cmdText, false, parameters));
            }
            catch(Exception ex)
            {
                return -2;
            }
        }
    }
}
