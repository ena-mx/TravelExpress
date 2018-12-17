namespace TravelExpress.Domain.Sql.Products
{
    using EnaBricks.DataBricks;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using TravelExpress.Domain.Products;

    public sealed class SqlProductStorage : ProductStorage
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["TravelExpress"].ConnectionString;
        private readonly SqlCommandHelper commandHelper = new SqlCommandHelper(_connectionString);

        #region Command Texts

        private static readonly string _isDuplicateProductCmdTxt = @"
            IF EXISTS(SELECT * FROM [catalogs].[Product] WITH(NOLOCK) WHERE [ProductId] <> @productId AND [Description] = @description)
            BEGIN
	            SELECT 1
            END
            ELSE
            BEGIN
	            SELECT 0
            END
        ";

        private static readonly string _saveProductCmdTxt = @"
            INSERT INTO [catalogs].[Product]
            ([ProductId], [Description])
            VALUES
            (@productId, @description)
        ";

        private static readonly string _updateProductCmdTxt = @"
            UPDATE [catalogs].[Product]
            SET
	            [Description] = @description
            WHERE [ProductId] = @productId
        ";

        private static readonly string _deleteProductCmdTxt = @"
            DELETE FROM [catalogs].[Product] WHERE [ProductId] = @productId
        ";

        private static readonly string _addAditionalServiceCmdTxt = @"
            INSERT INTO [catalogs].[ProductAditionalService]
            ([ProductId], [Description], [UnitPrice])
            VALUES
            (@productId, @description, @unitPrice)
        ";

        private static readonly string _addPriceCategoryCmdTxt = @"
            INSERT INTO [catalogs].[ProductPriceCategory]
			([ProductId], [Description])
			VALUES
			(@productId, @description)
        ";

        private static readonly string _addPriceDetailCmdTxt = @"
            INSERT INTO [catalogs].[ProductPriceDetail]
			([ProductPriceCategoryId], [Description], [UnitPrice])
			VALUES
			(@priceCategoryId, @description, @unitPrice)
        ";

        private static readonly string _deleteAditionalServiceCmdTxt = @"
            DELETE FROM [catalogs].[ProductAditionalService]
			WHERE [ProductAditionalServiceId] = @aditionalServiceId AND [ProductId] = @productId
        ";

        private static readonly string _deletePriceCategoryCmdTxt = @"
            IF EXISTS(SELECT * FROM [catalogs].[ProductPriceCategory] WHERE [ProductPriceCategoryId] = @categoryId AND [ProductId] = @productId)
            BEGIN
	            DELETE FROM [catalogs].[ProductPriceDetail]
	            WHERE [ProductPriceCategoryId] = @categoryId

	            DELETE FROM [catalogs].[ProductPriceCategory]
	            WHERE [ProductPriceCategoryId] = @categoryId
            END
        ";

        private static readonly string _deletePriceDetailCmdTxt = @"
            IF EXISTS(
				SELECT * 
				FROM [catalogs].[ProductPriceDetail] WITH(NOLOCK) 
				JOIN [catalogs].[ProductPriceCategory] WITH(NOLOCK) ON [catalogs].[ProductPriceDetail].[ProductPriceCategoryId] = [catalogs].[ProductPriceCategory].[ProductPriceCategoryId]
				WHERE [catalogs].[ProductPriceCategory].[ProductId] = @productId AND [catalogs].[ProductPriceDetail].[ProductPriceDetailId] = @priceDetailId)
			BEGIN
				DELETE FROM [catalogs].[ProductPriceDetail]
				WHERE [ProductPriceDetailId] = @priceDetailId
			END
        ";

        private static readonly string _addExcursionCmdTxt = @"
            INSERT INTO [travel].[Excursion]
            ([ExcursionId], [ProductId], [Description], [BeginDate], [EndDate])
			SELECT
			@excursionId, @productId, [Product].[Description], @beginDate, @endDate
			FROM [catalogs].[Product] WITH(NOLOCK)
			WHERE [ProductId] = @productId

            INSERT INTO [travel].[AditionalService]
            ([ExcursionId], [Description], [UnitPrice])
            SELECT 
	            @excursionId,
	            [Description],
	            [UnitPrice]
            FROM [catalogs].[ProductAditionalService] WITH(NOLOCK)
            WHERE [ProductId] = @productId

			INSERT INTO [travel].[PriceCategory]
			([ExcursionId], [Description])
			SELECT 
				@excursionId,
				[Description]
			FROM [catalogs].[ProductPriceCategory] WITH(NOLOCK)
			WHERE [ProductId] = @productId

			INSERT INTO [travel].[PriceDetail]
			([PriceCategoryId], [Description], [UnitPrice])
			SELECT 
				[travel].[PriceCategory].[PriceCategoryId],
				[catalogs].[ProductPriceDetail].[Description],
				[catalogs].[ProductPriceDetail].[UnitPrice]
			FROM [travel].[PriceCategory] WITH(NOLOCK)
			JOIN [catalogs].[ProductPriceCategory] WITH(NOLOCK) ON [ProductPriceCategory].[Description] = [PriceCategory].[Description]
			JOIN [catalogs].[ProductPriceDetail] WITH(NOLOCK) ON [ProductPriceDetail].[ProductPriceCategoryId] =  [ProductPriceCategory].[ProductPriceCategoryId]
			WHERE [travel].[PriceCategory].[ExcursionId] = @excursionId
        ";

        private static readonly string _duplicateExcursionCmdTxt = @"
            IF EXISTS(
	            SELECT [Excursion].[ExcursionId]
	            FROM [travel].[Excursion] WITH(NOLOCK) 
	            WHERE [Excursion].[ProductId] = @productId
		              AND [Excursion].[BeginDate] = @beginDate
		              AND [Excursion].[EndDate] = @endDate
	            )
            BEGIN
	            SELECT 1
            END 
            ELSE
            BEGIN
	            SELECT 0
            END
        ";

        private static readonly string _duplicateProductCategoryCmdTxt = @"
            IF EXISTS(
	            SELECT [catalogs].[ProductPriceCategory].[ProductPriceCategoryId]
	            FROM [catalogs].[ProductPriceCategory] WITH(NOLOCK) 
	            WHERE [catalogs].[ProductPriceCategory].[ProductId] = @productId
		              AND [catalogs].[ProductPriceCategory].[Description] = @description
	            )
            BEGIN
	            SELECT 1
            END 
            ELSE
            BEGIN
	            SELECT 0
            END
        ";

        #endregion Command Texts

        #region Private Behaviour

        private async Task<bool> ExecuteBooleanScalarAsync(string cmdTxt, SqlParameter[] parameters)
        {
            try
            {
                return Convert.ToBoolean(await commandHelper.ExecuteScalarAsync(cmdTxt, false, parameters));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> ExecuteNonQueryAsync(string cmdTxt, SqlParameter[] parameters)
        {
            try
            {
                return await commandHelper.ExecuteNonQueryAsync(cmdTxt, false, parameters) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion Private Behaviour

        #region Public Protocol

        public override async Task<bool> AddAditionalServiceAsync(Guid productId, string description, decimal unitPrice)
        {
            return await ExecuteNonQueryAsync(_addAditionalServiceCmdTxt, new SqlParameter[3]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    },
                    new SqlParameter("@unitPrice", System.Data.SqlDbType.Decimal)
                    {
                        Value = unitPrice
                    }
                });
        }

        public override async Task<bool> AddPriceCategoryAsync(Guid productId, string description)
        {
            return await ExecuteNonQueryAsync(_addPriceCategoryCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    }
                });
        }

        public override async Task<bool> AddPriceDetailAsync(Guid productId, int priceCategoryId, string description, decimal unitPrice)
        {
            return await ExecuteNonQueryAsync(_addPriceDetailCmdTxt, new SqlParameter[4]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@priceCategoryId", System.Data.SqlDbType.Int)
                    {
                        Value = priceCategoryId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    },
                    new SqlParameter("@unitPrice", System.Data.SqlDbType.Decimal)
                    {
                        Value = unitPrice
                    }
                });
        }

        public override async Task<bool> DeleteAditionalServiceAsync(Guid productId, int aditionalServiceId)
        {
            return await ExecuteNonQueryAsync(_deleteAditionalServiceCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@aditionalServiceId", System.Data.SqlDbType.Int)
                    {
                        Value = aditionalServiceId
                    }
                });
        }

        public override async Task<bool> DeletePriceCategoryAsync(Guid productId, int categoryId)
        {
            return await ExecuteNonQueryAsync(_deletePriceCategoryCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@categoryId", System.Data.SqlDbType.Int)
                    {
                        Value = categoryId
                    }
                });
        }

        public override async Task<bool> DeletePriceDetailAsync(Guid productId, int priceDetailId)
        {
            return await ExecuteNonQueryAsync(_deletePriceDetailCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@priceDetailId", System.Data.SqlDbType.Int)
                    {
                        Value = priceDetailId
                    }
                });
        }

        public override async Task<bool> DeleteProductAsync(Guid productId)
        {
            return await ExecuteNonQueryAsync(_deleteProductCmdTxt, new SqlParameter[1]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    }
                });
        }

        public override async Task<bool> IsDuplicateAsync(Guid productId, string description)
        {
            return await ExecuteBooleanScalarAsync(_isDuplicateProductCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    }
                });
        }

        public override async Task<bool> SaveProductAsync(Guid productId, string description)
        {
            return await ExecuteNonQueryAsync(_saveProductCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    }
                });
        }

        public override async Task<bool> UpdateProductDescriptionAsync(Guid productId, string description)
        {
            return await ExecuteNonQueryAsync(_updateProductCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    }
                });
        }

        public override async Task<bool> AddExcursionAsync(Guid productId, Guid excursionId, DateTime beginDate, DateTime endDate)
        {
            return await ExecuteNonQueryAsync(_addExcursionCmdTxt, new SqlParameter[4]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@excursionId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = excursionId
                    },
                    new SqlParameter("@beginDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = beginDate
                    },
                    new SqlParameter("@endDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = endDate
                    }
                });
        }

        public override async Task<bool> IsDuplicateExcursionAsync(Guid productId, DateTime beginDate, DateTime endDate)
        {
            return await ExecuteBooleanScalarAsync(_duplicateExcursionCmdTxt, new SqlParameter[3]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@beginDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = beginDate
                    },
                    new SqlParameter("@endDate", System.Data.SqlDbType.SmallDateTime)
                    {
                        Value = endDate
                    }
                });
        }

        public override async Task<bool> IsDuplicatePriceCategory(Guid productId, string description)
        {
            return await ExecuteBooleanScalarAsync(_duplicateProductCategoryCmdTxt, new SqlParameter[2]
                {
                    new SqlParameter("@productId", System.Data.SqlDbType.UniqueIdentifier)
                    {
                        Value = productId
                    },
                    new SqlParameter("@description", System.Data.SqlDbType.VarChar)
                    {
                        Value = description
                    }
                }) ;
        }

        #endregion Public Protocol
    }
}