namespace TravelExpress.Queries.Sql.Excursions
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;
    using TravelExpress.Queries.Excursions;

    public sealed class SqlExcursionQueryComponent : ExcursionQueryComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _cmdTxtExcursionDetail = "[travel].[sp_ExcursionDetail]";
        private readonly string _cmdTxtExcursionPage = "[travel].[sp_ExcursionPage]";
        private readonly string _rootName = "GenericPage";

        public override async Task<ExcursionDataModel> FindAsync(Guid excursionId)
        {
            using (SqlDeserializerComponent<ExcursionDataModel> component = new SqlDeserializerComponent<ExcursionDataModel>(
               _connectionString,
               _cmdTxtExcursionDetail,               
               new SqlParameter[]
               {
                    new SqlParameter("@excursionId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = excursionId
                    }
               }))
            {
                return await component.ExecuteStoreProcedureAndDeserialize();
            }
        }

        public override async Task<GenericPage<ExcursionIndexModel>> PageAsync(int offset, int limit, string searchValue)
        {
            using (SqlDeserializerComponent<GenericPage<ExcursionIndexModel>> component = new SqlDeserializerComponent<GenericPage<ExcursionIndexModel>>(
               _connectionString,
               _cmdTxtExcursionPage,
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
    }
}
