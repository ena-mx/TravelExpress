namespace TravelExpress.Domain.Sql.Customers.Shared
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Customers.Shared;

    public sealed class SqlCustomersStorage : CustomerStorageComponent
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly string _findDuplicateEmailCmdText = @"
            IF EXISTS(SELECT * FROM [management].[CustomerInfo] WITH(NOLOCK) WHERE [CustomerId] <> @customerId AND [Email] = @email)
            BEGIN
	            SELECT 1
            END
            ELSE 
            BEGIN
	            SELECT 0
            END
        ";
        private readonly string _addStoreCmdText = @"
            INSERT INTO [management].[Customer]
            ([CustomerId])
            VALUES
            (@customerId)

            INSERT INTO [management].[CustomerInfo]
            ([CustomerId], [Name], [FamilyName1], [FamilyName2], [Telephone], [Cellphone], [Email], [BirthDate], [RegisterDate], [LastUpdateDate])
            VALUES
            (@customerId, @name, @familyName1, @familyName2, @telephone, @cellphone, @email, @birthDate, @lastUpdatedDate, @lastUpdatedDate)

            INSERT INTO [management].[CustomerOptions]
            (CustomerId, PhoneCallsEnabled, MailEnabled)
            VALUES
            (@customerId, @phoneCallsEnabled, @mailEnabled)
        ";
        private readonly string _updateCmdText = @"
            UPDATE [management].[CustomerInfo]
            SET 
	            [Name] = @name, 
	            [FamilyName1] = @familyName1, 
	            [FamilyName2] = @familyName2, 
	            [Telephone] = @telephone, 
	            [Cellphone] = @cellphone, 
	            [Email] = @email, 
	            [BirthDate] = @birthDate,
	            [LastUpdateDate] = @lastUpdatedDate
            WHERE [CustomerId] = @customerId

            UPDATE [management].[CustomerOptions]
            SET
	            PhoneCallsEnabled = @phoneCallsEnabled, 
	            MailEnabled = @mailEnabled
            WHERE [CustomerId] = @customerId
        ";

        private static readonly string _addObservationCmdTxt = @"
            INSERT INTO [management].[CustomerObservation]
            (CustomerId, UserId, [Description], RegisterDate)
            VALUES
            (@customerId, @userId, @description, @date)
        ";
        private static readonly string _addPhoneCallCmdTxt = @"
            INSERT INTO [management].[CustomerPhoneCall]
            (CustomerId, UserId, [Description], RegisterDate)
            VALUES
            (@customerId, @userId, @description, @date)
        ";

        public override async Task<bool> FindDuplicateEmailAsync(Guid customerId, string email)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[2]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@email", System.Data.SqlDbType.VarChar)
                    {
                        Value = email
                    }
                };
            return Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(_findDuplicateEmailCmdText, false, parameters));
        }

        public override async Task<bool> TryAddAsync(
            Guid customerId,
            string name,
            string familyName1,
            string familyName2,
            string telephone,
            string cellphone,
            string email,
            DateTime birthDate,
            DateTime lastUpdatedDate,
            bool phoneCallsEnabled,
            bool mailEnabled)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[11]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@name", System.Data.SqlDbType.VarChar)
                    {
                        Value = name
                    },
                    new SqlParameter("@familyName1", System.Data.SqlDbType.VarChar)
                    {
                        Value = familyName1
                    },
                    new SqlParameter("@familyName2", System.Data.SqlDbType.VarChar)
                    {
                        Value = familyName2
                    },
                    new SqlParameter("@telephone", System.Data.SqlDbType.VarChar)
                    {
                        Value = telephone
                    },
                    new SqlParameter("@cellphone", System.Data.SqlDbType.VarChar)
                    {
                        Value = cellphone
                    },
                    new SqlParameter("@email", System.Data.SqlDbType.VarChar)
                    {
                        Value = email
                    },
                    new SqlParameter("@birthDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = birthDate
                    },
                    new SqlParameter("@lastUpdatedDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = lastUpdatedDate
                    },
                    new SqlParameter("@phoneCallsEnabled", System.Data.SqlDbType.Bit)
                    {
                        Value = phoneCallsEnabled
                    },
                    new SqlParameter("@mailEnabled", System.Data.SqlDbType.Bit)
                    {
                        Value =mailEnabled
                    }
                };
            return await commandHelper.ExecuteNonQueryAsync(_addStoreCmdText, false, parameters) > 0;
        }

        public override async Task<bool> TryUpdateAsync(
            Guid customerId, 
            string name, 
            string familyName1, 
            string familyName2, 
            string telephone, 
            string cellphone, 
            string email, 
            DateTime birthDate,
            DateTime lastUpdatedDate, 
            bool phoneCallsEnabled, 
            bool mailEnabled)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[11]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@name", System.Data.SqlDbType.VarChar)
                    {
                        Value = name
                    },
                    new SqlParameter("@familyName1", System.Data.SqlDbType.VarChar)
                    {
                        Value = familyName1
                    },
                    new SqlParameter("@familyName2", System.Data.SqlDbType.VarChar)
                    {
                        Value = familyName2
                    },
                    new SqlParameter("@telephone", System.Data.SqlDbType.VarChar)
                    {
                        Value = telephone
                    },
                    new SqlParameter("@cellphone", System.Data.SqlDbType.VarChar)
                    {
                        Value = cellphone
                    },
                    new SqlParameter("@email", System.Data.SqlDbType.VarChar)
                    {
                        Value = email
                    },
                    new SqlParameter("@birthDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = birthDate
                    },
                    new SqlParameter("@lastUpdatedDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = lastUpdatedDate
                    },
                    new SqlParameter("@phoneCallsEnabled", System.Data.SqlDbType.Bit)
                    {
                        Value = phoneCallsEnabled
                    },
                    new SqlParameter("@mailEnabled", System.Data.SqlDbType.Bit)
                    {
                        Value =mailEnabled
                    }
                };
            return await commandHelper.ExecuteNonQueryAsync(_updateCmdText, false, parameters) > 0;
        }

        public override async Task<bool> TryAddObservationAsync(Guid customerId, string description, Guid userId, DateTime date)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    },
                    new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = userId
                    },
                    new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = date
                    },
                };
            return await commandHelper.ExecuteNonQueryAsync(_addObservationCmdTxt, false, parameters) > 0;
        }

        public override async Task<bool> TryAddPhoneCallAsync(Guid customerId, string description, Guid userId, DateTime date)
        {
            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);
            SqlParameter[] parameters = new SqlParameter[4]
                {
                    new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = customerId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    },
                    new SqlParameter("@userId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = userId
                    },
                    new SqlParameter("@date", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = date
                    },
                };
            return await commandHelper.ExecuteNonQueryAsync(_addPhoneCallCmdTxt, false, parameters) > 0;
        }
    }
}