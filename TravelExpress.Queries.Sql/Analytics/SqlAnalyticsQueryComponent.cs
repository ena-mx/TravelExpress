namespace TravelExpress.Queries.Sql.Analytics
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Analytics;

    public class SqlAnalyticsQueryComponent : AnalyticsQueryComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdTxt = "[analytics].[sp_AnalyticsModel]";

        public override async Task<AnalyticsModel> AnalyticsModelAsync(DateTime dateUserActivity, DateTime dateActiveExcursions)
        {
            using (SqlDeserializerComponent<AnalyticsModel> component = new SqlDeserializerComponent<AnalyticsModel>(
               _connectionString,
               _cmdTxt,
               new SqlParameter[2]
               {
                    new SqlParameter("@dateUserActivity", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = dateUserActivity
                    },
                    new SqlParameter("@dateActiveExcursions", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = dateActiveExcursions
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }
    }
}
