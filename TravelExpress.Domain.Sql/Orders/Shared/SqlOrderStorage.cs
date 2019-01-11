namespace TravelExpress.Domain.Sql.Orders.Shared
{
    using EnaBricks.DataBricks;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Text;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Orders;

    public sealed class SqlOrderStorage : OrderStorage
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private static readonly string _addBillingCmdTxt = @"
            DECLARE @totalAmount MONEY
			SELECT @totalAmount = (SUM([OrderDetail].[UnitPrice] * [OrderDetail].[Quantity]))
			FROM [orders].[OrderDetail] WITH(NOLOCK)
			WHERE [OrderDetail].[OrderId] = @orderId

			INSERT INTO [billing].[Bill]
			([BillId], [CustomerId], [OrderId], [TotalAmount], [TotalPaid], [TotalDebit], [BillStatusId], [CreatedDate], [PaidDate])
			SELECT 
				@billId,
				@customerId,
				@orderId,
				@totalAmount,
				0, 
				@totalAmount,
				1,
				@serverDate,
				NULL
			FROM [orders].[Order] WITH(NOLOCK)
			WHERE [Order].[OrderId] = @orderId
        ";

        private static readonly string _addOrderDetailsCmdTxt = @"
            DECLARE @aditionalServicesXml XML = CONVERT(XML, @aditionalServices)
            DECLARE @orderDetailsXml XML = CONVERT(XML, @orderDetails)

            INSERT INTO [orders].[OrderDetail]
            ([OrderId], [OrderDetailTypeId], [Description], [UnitPrice], [Quantity])
            SELECT @orderId, 1, [PriceCategory].[Description] + ' ' + [PriceDetail].[Description], [PriceDetail].[UnitPrice], [XmlData].[Qty]
            FROM [travel].[PriceDetail]
			JOIN [travel].[PriceCategory] ON [PriceCategory].[PriceCategoryId] = [PriceDetail].[PriceCategoryId]
            JOIN(
	            SELECT 
		            t.c.query('./Id').value('.', 'INT') 'Id',
		            t.c.query('./Qty').value('.', 'INT') 'Qty'
	            FROM @orderDetailsXml.nodes('Root/Node') t(c)
            ) AS [XmlData] ON XmlData.[Id] = [PriceDetail].[PriceDetailId]


            INSERT INTO [orders].[OrderDetail]
            ([OrderId], [OrderDetailTypeId], [Description], [UnitPrice], [Quantity])
            SELECT 
	            @orderId, 2, [AditionalService].[Description], [AditionalService].[UnitPrice], [XmlData].[Qty]
            FROM [travel].[AditionalService]
            JOIN(
	            SELECT 
		            t.c.query('./Id').value('.', 'INT') 'Id',
		            t.c.query('./Qty').value('.', 'INT') 'Qty'
	            FROM @aditionalServicesXml.nodes('Root/Node') t(c)
            ) AS [XmlData] ON XmlData.[Id] = [AditionalService].[AditionalServiceId]
        ";

        private static readonly string _addPaymentCmdTxt = @"
            IF EXISTS(SELECT * FROM [billing].[Bill] WITH(NOLOCK) WHERE [BillId] = @billId AND [OrderId] = @orderId)
			BEGIN
				INSERT INTO [billing].[BillPayment]
				([BillId], [Amount], [Date])
				VALUES
				(@billId, @amount, @date)

				DECLARE @TotalPaid MONEY
				DECLARE @TotalDebit MONEY

				SELECT @TotalPaid = SUM([Amount])
				FROM [billing].[BillPayment] WITH(NOLOCK)
				WHERE [BillPayment].[BillId] = @billId

				SELECT @TotalDebit = ([TotalAmount] - @TotalPaid)
				FROM [billing].[Bill] WITH(NOLOCK)
				WHERE [Bill].[BillId] = @billId

				IF @TotalDebit < 0
				BEGIN
					SET @TotalDebit = 0
				END

				UPDATE [billing].[Bill]
				SET 
					[Bill].[TotalPaid] = @TotalPaid,
					[Bill].[TotalDebit] = @TotalDebit
				WHERE [Bill].[BillId] = @billId
			END
        ";

        private static readonly string _createOrderHeaderCmdTxt = @"
            INSERT INTO [orders].[Order]
			([OrderId], [CustomerId], [OrderStatusId], [ExcursionId], [CreatedDate])
			VALUES
			(@orderId, @customerId, 1, @excursionId, @serverDate)
        ";

        private static readonly string _hasPaymentsCmdTxt = @"
            IF EXISTS(
					SELECT * 
					FROM [billing].[Bill] WITH(NOLOCK) 
					JOIN [billing].[BillPayment] WITH(NOLOCK) ON [BillPayment].[BillId] = [Bill].[BillId]
					WHERE [Bill].[OrderId] = @orderId)
			BEGIN
				SELECT 1
			END
			ELSE
			BEGIN
				SELECT 0
			END
        ";

        private static readonly string _isPaidCmdTxt = @"
            IF EXISTS(
					SELECT * 
					FROM [billing].[Bill] WITH(NOLOCK)
					WHERE [Bill].[OrderId] = @orderId AND [Bill].[BillStatusId] = 3)
			BEGIN
				SELECT 1
			END
			ELSE
			BEGIN
				DECLARE @TotalPaid MONEY
				DECLARE @TotalAmount MONEY
				DECLARE @billId UNIQUEIDENTIFIER

				SELECT @billId = [BillId]
				FROM [billing].[Bill] WITH(NOLOCK)
				WHERE [Bill].[OrderId] = @orderId

				SELECT @TotalPaid = SUM([Amount])
				FROM [billing].[BillPayment] WITH(NOLOCK)
				WHERE [BillPayment].[BillId] = @billId

				SELECT @TotalAmount = [TotalAmount]
				FROM [billing].[Bill] WITH(NOLOCK)
				WHERE [Bill].[BillId] = @billId

				IF @TotalPaid >= @TotalAmount
				BEGIN
					SELECT 1
				END
				ELSE
				BEGIN
					SELECT 0
				END
			END
        ";

        public override async Task<bool> AddBillingAsync(Guid orderId, Guid billId, Guid customerId, DateTime serverDate)
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
                new SqlParameter("@customerId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = customerId
                },
                new SqlParameter("@serverDate", System.Data.SqlDbType.SmallDateTime)
                {
                    Value = serverDate
                },
            };
            return await commandHelper.ExecuteNonQueryAsync(_addBillingCmdTxt, false, parameters) > 0;
        }

        public override async Task<bool> AddOrderDetailsAsync(Guid orderId, KeyValuePair<int, int>[] aditionalServices, KeyValuePair<int, int>[] orderDetails)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Root>");

            for (int i = 0; i < aditionalServices.Length; i++)
            {
                sb.Append("<Node>");

                sb.Append("<Id>");
                sb.Append(aditionalServices[i].Key);
                sb.Append("</Id>");

                sb.Append("<Qty>");
                sb.Append(aditionalServices[i].Value);
                sb.Append("</Qty>");

                sb.Append("</Node>");
            }

            sb.Append("</Root>");

            string aditionalServicesXml = sb.ToString();

            sb.Clear();

            sb.Append("<Root>");

            for (int i = 0; i < orderDetails.Length; i++)
            {
                sb.Append("<Node>");

                sb.Append("<Id>");
                sb.Append(orderDetails[i].Key);
                sb.Append("</Id>");

                sb.Append("<Qty>");
                sb.Append(orderDetails[i].Value);
                sb.Append("</Qty>");

                sb.Append("</Node>");
            }

            sb.Append("</Root>");

            string orderDetailsXml = sb.ToString();

            sb = null;

            SqlParameter[] parameters = new SqlParameter[3]
            {
                new SqlParameter("@orderId", System.Data.SqlDbType.UniqueIdentifier)
                {
                    Value = orderId
                },
                new SqlParameter("@aditionalServices", System.Data.SqlDbType.VarChar)
                {
                    Value = aditionalServicesXml
                },
                new SqlParameter("@orderDetails", System.Data.SqlDbType.VarChar)
                {
                    Value = orderDetailsXml
                }
            };

            SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);

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
