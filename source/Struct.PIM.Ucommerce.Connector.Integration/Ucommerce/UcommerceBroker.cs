using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Dapper;
using Newtonsoft.Json;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce
{
    public class UcommerceBroker
    {
        public static UcommerceBroker Instance { get; } = new UcommerceBroker();

        public int GetProductRelationTypeId(string relationName)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.ExecuteScalar<int>(@"
                    SELECT [ProductRelationTypeId]
                    FROM [uCommerce_ProductRelationType]
                    WHERE [Name] = @relationName", new { relationName = relationName });
            }
        }

        public Dictionary<string, int> GetProductCatalogIdByName()
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.Query(@"
                    SELECT [ProductCatalogId],[Name]
                    FROM [uCommerce_ProductCatalog]
                    WHERE Deleted = 0").ToDictionary(
                    row => (string)row.Name,
                    row => (int)row.ProductCatalogId);
            }
        }

        public int GetDefinitionId(string definitionName)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.ExecuteScalar<int>(@"
                    SELECT [DefinitionId]
                    FROM [uCommerce_Definition]
                    WHERE [Name] = @definitionName AND Deleted = 0", new { definitionName = definitionName });
            }
        }

        public Dictionary<int, int> GetCategoryIdByPimId()
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.Query(@"
                    SELECT [Value] AS PimID, [CategoryId]
                    FROM [uCommerce_CategoryProperty] cp
                    JOIN [uCommerce_DefinitionField] df on cp.DefinitionFieldId = df.DefinitionFieldId
                    WHERE df.[Name] = 'PimID' AND Deleted = 0").ToDictionary(
                    row => int.Parse((string)row.PimID),
                    row => (int)row.CategoryId);
            }
        }

        public Dictionary<string, int> GetProductDefinitionIdByName()
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.Query(@"
                    SELECT [Name],[ProductDefinitionId]
                    FROM [uCommerce_ProductDefinition]
                    WHERE Deleted = 0").ToDictionary(
                    row => (string)row.Name,
                    row => (int)row.ProductDefinitionId);
            }
        }

        public Dictionary<string, int> GetProductDescriptionIdByKey(List<int> productIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                BulkOperationsHelper.InsertIntoTempTable(
                    connection,
                    "struct",
                    "#tmp",
                    new Dictionary<string, string> { { "ProductId", "int" } },
                    productIds.Select(x => new { ProductId = x }).ToList()
                );

                return connection.Query(@"
                    SELECT pd.[ProductId],[CultureCode],[ProductDescriptionId]
                    FROM [uCommerce_ProductDescription] pd
                    JOIN #tmp as t on t.ProductId = pd.ProductId").ToDictionary(
                    row => (string)(row.ProductId + "_" + row.CultureCode),
                    row => (int)row.ProductDescriptionId);
            }
        }

        public Dictionary<string, int> GetProductDefinitionFieldIdByKey()
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.Query(@"
                    ;WITH cte ([ProductDefinitionId], [ParentProductDefinitionId]) AS
                    (SELECT [ProductDefinitionId],[ParentProductDefinitionId]
	                    FROM [uCommerce_ProductDefinitionRelation]
	                    UNION ALL
	                    SELECT cte.[ProductDefinitionId],t.[ParentProductDefinitionId]
	                    FROM [uCommerce_ProductDefinitionRelation] t
	                    JOIN cte ON t.[ProductDefinitionId] = cte.[ParentProductDefinitionId] AND t.[ParentProductDefinitionId] <> cte.[ParentProductDefinitionId])
                    SELECT pd.[Name] AS [Definition],pdf.[Name] AS [Field],pdf.ProductDefinitionFieldId
                    FROM cte
                    JOIN [uCommerce_ProductDefinition] pd ON pd.ProductDefinitionId = cte.ProductDefinitionId
                    JOIN [uCommerce_ProductDefinitionField] pdf ON cte.ParentProductDefinitionId = pdf.ProductDefinitionId
                    WHERE pd.Deleted = 0 AND pdf.Deleted = 0
                    UNION
                    SELECT pd.[Name] AS [Definition],pdf.[Name] AS [Field],pdf.ProductDefinitionFieldId
                    FROM [uCommerce_ProductDefinition] pd
                    JOIN [uCommerce_ProductDefinitionField] pdf ON pd.ProductDefinitionId = pdf.ProductDefinitionId
                    WHERE pd.Deleted = 0 AND pdf.Deleted = 0
                    ORDER BY [Definition], [Field]").ToDictionary(
                    row => (string)row.Definition + "_" + (string)row.Field,
                    row => (int)row.ProductDefinitionFieldId);
            }
        }

        public Dictionary<string, int> GetDefinitionFieldIdByKey()
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                return connection.Query(@"
                    SELECT d.[Name] + '_'  + df.[Name] as [Key], df.DefinitionFieldId
                    FROM [uCommerce_Definition] d
                    JOIN [uCommerce_DefinitionField] df on d.DefinitionId = df.DefinitionId
                    WHERE d.Deleted = 0 AND df.Deleted = 0 and d.[Name] = 'Default Category Definition'").ToDictionary(
                    row => (string)row.Key,
                    row => (int)row.DefinitionFieldId);
            }
        }

        public Dictionary<int, int> GetProductIdByPimId(List<int> pimProductIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                BulkOperationsHelper.InsertIntoTempTable(
                    connection,
                    "struct",
                    "#tmp",
                    new Dictionary<string, string> { { "PimID", "nvarchar(50)" } },
                    pimProductIds.Select(x => new { PimID = x }).ToList()
                );

                return connection.Query(@"
                    SELECT pp.[Value] as PimID, pp.ProductId
                    FROM [uCommerce_ProductDefinitionField] pdf
                    JOIN [uCommerce_ProductProperty] pp on pdf.ProductDefinitionFieldId = pp.ProductDefinitionFieldId
                    JOIN #tmp as t on t.PimID = pp.[Value]
                    WHERE pdf.[name] = 'PimID'").ToDictionary(
                    row => int.Parse((string)row.PimID),
                    row => (int)row.ProductId);
            }
        }

        public Dictionary<int, string> GetProductSkuByProductId(List<int> productIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                BulkOperationsHelper.InsertIntoTempTable(
                    connection,
                    "struct",
                    "#tmp",
                    new Dictionary<string, string> { { "ProductId", "int" } },
                    productIds.Select(x => new { ProductId = x }).ToList()
                );

                return connection.Query(@"
                    SELECT p.[ProductId],p.[Sku]
                    FROM [uCommerce_Product] p
                    JOIN #tmp as t on t.ProductId = p.ProductId").ToDictionary(
                    row => (int)row.ProductId,
                    row => (string)row.Sku);
            }
        }

        public Dictionary<int, int> GetProductIdByVariantPimId(List<int> pimVariantIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                BulkOperationsHelper.InsertIntoTempTable(
                    connection,
                    "struct",
                    "#tmp",
                    new Dictionary<string, string> { { "VariantPimID", "nvarchar(50)" } },
                    pimVariantIds.Select(x => new { VariantPimID = x }).ToList()
                );

                return connection.Query(@"
                    SELECT pp.[Value] as PimID, pp.ProductId
                    FROM [uCommerce_ProductDefinitionField] pdf
                    JOIN [uCommerce_ProductProperty] pp on pdf.ProductDefinitionFieldId = pp.ProductDefinitionFieldId
                    JOIN #tmp as t on t.VariantPimID = pp.[Value]
                    WHERE [name] = 'VariantPimID'").ToDictionary(
                    row => int.Parse((string)row.PimID),
                    row => (int)row.ProductId);
            }
        }

        public void UpsertProductDescriptions(List<UcProductDescription> productDescriptions)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                    {
                        { "ProductDescriptionId", "int" },
                        { "ProductId", "int" },
                        { "DisplayName", "nvarchar(512)" },
                        { "ShortDescription", "nvarchar(max)" },
                        { "LongDescription", "nvarchar(max)" },
                        { "CultureCode", "nvarchar(60)" },
                        { "Guid", "uniqueidentifier" },
                    };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, productDescriptions);

                var targetTable = "uCommerce_ProductDescription";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductId = SOURCE.ProductId and 
                                TARGET.CultureCode = SOURCE.CultureCode)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED
                            THEN UPDATE SET TARGET.DisplayName = SOURCE.DisplayName,
                                            TARGET.ShortDescription = SOURCE.ShortDescription,
                                            TARGET.LongDescription = SOURCE.LongDescription

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([ProductId],[DisplayName],[ShortDescription],[LongDescription],[CultureCode],[Guid])
                                VALUES (SOURCE.[ProductId],SOURCE.[DisplayName],SOURCE.[ShortDescription],SOURCE.[LongDescription],SOURCE.[CultureCode],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public void UpsertProductProperties(List<UcProductProperty> productProperties)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "ProductPropertyId", "int" },
                    { "ProductId", "int" },
                    { "ProductDefinitionFieldId", "int" },
                    { "Value", "nvarchar(max)" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, productProperties);

                var targetTable = "uCommerce_ProductProperty";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductId = SOURCE.ProductId and 
                                TARGET.ProductDefinitionFieldId = SOURCE.ProductDefinitionFieldId)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED AND ((TARGET.Value <> SOURCE.Value) OR (ISNULL(TARGET.Value, SOURCE.Value) IS NOT NULL))
                            THEN UPDATE SET TARGET.Value = SOURCE.Value

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([Value],[ProductDefinitionFieldId],[ProductId],[Guid])
                                VALUES (SOURCE.[Value],SOURCE.[ProductDefinitionFieldId],SOURCE.[ProductId],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public void UpsertProductLocalizedProperties(List<UcProductDescriptionProperty> productDescriptionProperties)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "ProductDescriptionPropertyId", "int" },
                    { "ProductDescriptionId", "int" },
                    { "ProductDefinitionFieldId", "int" },
                    { "Value", "nvarchar(max)" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, productDescriptionProperties);

                var targetTable = "uCommerce_ProductDescriptionProperty";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductDescriptionId = SOURCE.ProductDescriptionId and 
                                TARGET.ProductDefinitionFieldId = SOURCE.ProductDefinitionFieldId)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED AND ((TARGET.Value <> SOURCE.Value) OR (ISNULL(TARGET.Value, SOURCE.Value) IS NOT NULL))
                            THEN UPDATE SET TARGET.Value = SOURCE.Value

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([ProductDescriptionId],[ProductDefinitionFieldId],[Value],[Guid])
                                VALUES (SOURCE.[ProductDescriptionId],SOURCE.[ProductDefinitionFieldId],SOURCE.[Value],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public List<MergeResult> UpsertProducts(List<UcProduct> products)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "ProductId", "int" },
                    { "ParentProductId", "int" },
                    { "Sku", "[nvarchar](400)" },
                    { "VariantSku", "[nvarchar](400)" },
                    { "Name", "[nvarchar](512)" },
                    { "DisplayOnSite", "bit" },
                    { "ThumbnailImageMediaId", "[nvarchar](max)" },
                    { "PrimaryImageMediaId", "[nvarchar](max)" },
                    { "Weight", "[decimal](18, 4)" },
                    { "ProductDefinitionId", "int" },
                    { "AllowOrdering", "bit" },
                    { "ModifiedBy", "[nvarchar](50)" },
                    { "ModifiedOn", "datetime" },
                    { "CreatedOn", "datetime" },
                    { "CreatedBy", "[nvarchar](50)" },
                    { "Rating", "float" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, products);

                var targetTable = "uCommerce_Product";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductId = SOURCE.ProductId)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED
                            THEN UPDATE SET TARGET.[ParentProductId]=SOURCE.[ParentProductId],
                                            TARGET.[Sku]=SOURCE.[Sku],
                                            TARGET.[VariantSku]=SOURCE.[VariantSku],
                                            TARGET.[Name]=SOURCE.[Name],
                                            TARGET.[DisplayOnSite]=SOURCE.[DisplayOnSite],
                                            TARGET.[ThumbnailImageMediaId]=SOURCE.[ThumbnailImageMediaId],
                                            TARGET.[PrimaryImageMediaId]=SOURCE.[PrimaryImageMediaId],
                                            TARGET.[Weight]=SOURCE.[Weight],
                                            TARGET.[ProductDefinitionId]=SOURCE.[ProductDefinitionId],
                                            TARGET.[AllowOrdering]=SOURCE.[AllowOrdering],
                                            TARGET.[ModifiedBy]=SOURCE.[ModifiedBy],
                                            TARGET.[ModifiedOn]=SOURCE.[ModifiedOn],
                                            TARGET.[Rating]=SOURCE.[Rating]

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([ParentProductId],[Sku],[VariantSku],[Name],[DisplayOnSite],[ThumbnailImageMediaId],[PrimaryImageMediaId],
                                        [Weight],[ProductDefinitionId],[AllowOrdering],[ModifiedBy],[ModifiedOn],[CreatedOn],[CreatedBy],[Rating],[Guid])
                                VALUES (SOURCE.[ParentProductId],SOURCE.[Sku],SOURCE.[VariantSku],SOURCE.[Name],SOURCE.[DisplayOnSite],SOURCE.[ThumbnailImageMediaId],SOURCE.[PrimaryImageMediaId],
                                        SOURCE.[Weight],SOURCE.[ProductDefinitionId],SOURCE.[AllowOrdering],SOURCE.[ModifiedBy],SOURCE.[ModifiedOn],SOURCE.[CreatedOn],SOURCE.[CreatedBy],SOURCE.[Rating],SOURCE.[Guid])

                            --$action specifies a column of type nvarchar(10) in the OUTPUT clause that returns 
                            --one of three values for each row: 'INSERT', 'UPDATE', or 'DELETE' according to the action that was performed on that row
                            OUTPUT $action, 
                            INSERTED.ProductId AS ItemId,
                            INSERTED.Guid AS Guid;";

                var mergeResult = connection.Query<MergeResult>(sql).ToList();
                return mergeResult;
            }
        }

        public void UpsertCategoryProductRelations(List<UcCategoryProductRelation> categoryProductRelations)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "CategoryProductRelationId", "int" },
                    { "ProductId", "int" },
                    { "CategoryId", "int" },
                    { "SortOrder", "int" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, categoryProductRelations);

                var targetTable = "uCommerce_CategoryProductRelation";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductId = SOURCE.ProductId and TARGET.CategoryId = SOURCE.CategoryId)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED AND (TARGET.SortOrder <> SOURCE.SortOrder)
                            THEN UPDATE SET TARGET.SortOrder = SOURCE.SortOrder

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([ProductId],[CategoryId],[SortOrder],[Guid])
                                VALUES (SOURCE.[ProductId],SOURCE.[CategoryId],SOURCE.[SortOrder],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public List<MergeResult> UpsertCategories(List<UcCategory> categories)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "CategoryId", "int" },
                    { "Name", "[nvarchar](60)" },
                    { "ImageMediaId", "[nvarchar](max)" },
                    { "DisplayOnSite", "bit" },
                    { "ParentCategoryId", "int" },
                    { "ProductCatalogId", "int" },
                    { "CreatedBy", "[nvarchar](255)" },
                    { "CreatedOn", "datetime" },
                    { "ModifiedOn", "datetime" },
                    { "ModifiedBy", "[nvarchar](50) " },
                    { "Deleted", "bit" },
                    { "SortOrder", "int" },
                    { "DefinitionId", "int" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, categories);

                var targetTable = "uCommerce_Category";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.CategoryId = SOURCE.CategoryId)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED
                            THEN UPDATE SET TARGET.Name = SOURCE.Name,
                                                  TARGET.ImageMediaId = SOURCE.ImageMediaId,
                                                  TARGET.DisplayOnSite = SOURCE.DisplayOnSite,
                                                  TARGET.CreatedOn = SOURCE.CreatedOn,
                                                  TARGET.ParentCategoryId = SOURCE.ParentCategoryId,
                                                  TARGET.ProductCatalogId = SOURCE.ProductCatalogId,
                                                  TARGET.ModifiedOn = SOURCE.ModifiedOn,
                                                  TARGET.ModifiedBy = SOURCE.ModifiedBy,
                                                  TARGET.Deleted = SOURCE.Deleted,
                                                  TARGET.SortOrder = SOURCE.SortOrder,
                                                  TARGET.CreatedBy = SOURCE.CreatedBy,
                                                  TARGET.DefinitionId = SOURCE.DefinitionId

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([Name],[ImageMediaId],[DisplayOnSite],[CreatedOn],[ParentCategoryId],[ProductCatalogId],[ModifiedOn],[ModifiedBy],[Deleted],[SortOrder],[CreatedBy],[DefinitionId],[Guid])
                                VALUES (SOURCE.[Name],SOURCE.[ImageMediaId],SOURCE.[DisplayOnSite],SOURCE.[CreatedOn],SOURCE.[ParentCategoryId],SOURCE.[ProductCatalogId],SOURCE.[ModifiedOn],SOURCE.[ModifiedBy],SOURCE.[Deleted],SOURCE.[SortOrder],SOURCE.[CreatedBy],SOURCE.[DefinitionId],SOURCE.[Guid])

                            --$action specifies a column of type nvarchar(10) in the OUTPUT clause that returns 
                            --one of three values for each row: 'INSERT', 'UPDATE', or 'DELETE' according to the action that was performed on that row
                            OUTPUT $action, 
                            INSERTED.CategoryId AS ItemId,
                            INSERTED.Guid AS Guid;";

                var mergeResult = connection.Query<MergeResult>(sql).ToList();
                return mergeResult;
            }
        }

        public void UpsertCategoryProperties(List<UcCategoryProperty> categoryProperties)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "CategoryPropertyId", "int" },
                    { "CategoryId", "int" },
                    { "DefinitionFieldId", "int" },
                    { "Value", "nvarchar(max)" },
                    { "CultureCode", "nvarchar(60)" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, categoryProperties);

                var targetTable = "uCommerce_CategoryProperty";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.CategoryId = SOURCE.CategoryId and 
                                TARGET.DefinitionFieldId = SOURCE.DefinitionFieldId and 
                                ((TARGET.CultureCode IS NULL AND SOURCE.CultureCode IS NULL) OR (TARGET.CultureCode = SOURCE.CultureCode)))

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED AND ((TARGET.Value <> SOURCE.Value) OR (ISNULL(TARGET.Value, SOURCE.Value) IS NOT NULL))
                            THEN UPDATE SET TARGET.Value = SOURCE.Value

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([Value],[DefinitionFieldId],[CultureCode],[CategoryId],[Guid])
                                VALUES (SOURCE.[Value],SOURCE.[DefinitionFieldId],SOURCE.[CultureCode],SOURCE.[CategoryId],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public void UpsertCategoryDescriptions(List<UcCategoryDescription> categoryDescriptions)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "CategoryDescriptionId", "int" },
                    { "CategoryId", "int" },
                    { "DisplayName", "[nvarchar](512)" },
                    { "Description", "[nvarchar](max)" },
                    { "CultureCode", "[nvarchar](60)" },
                    { "RenderAsContent", "bit" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, categoryDescriptions);

                var targetTable = "uCommerce_CategoryDescription";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.CategoryId = SOURCE.CategoryId and TARGET.CultureCode = SOURCE.CultureCode)

                            --When records are matched, update the records if there is any change
                            WHEN MATCHED
                            THEN UPDATE SET TARGET.DisplayName = SOURCE.DisplayName,
                                            TARGET.Description = SOURCE.Description,
                                            TARGET.RenderAsContent = SOURCE.RenderAsContent

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([CategoryId],[DisplayName],[Description],[CultureCode],[RenderAsContent],[Guid])
                                VALUES (SOURCE.[CategoryId],SOURCE.[DisplayName],SOURCE.[Description],SOURCE.[CultureCode],SOURCE.[RenderAsContent],SOURCE.[Guid]);";

                connection.Execute(sql);
            }
        }

        public void UpsertProductRelations(List<UcProductRelation> productRelations)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                var columns = new Dictionary<string, string>
                {
                    { "ProductRelationId", "int" },
                    { "ProductId", "int" },
                    { "RelatedProductId", "int" },
                    { "ProductRelationTypeId", "int" },
                    { "Guid", "uniqueidentifier" },
                };

                BulkOperationsHelper.InsertIntoTempTable(connection, "struct", "#tmp", columns, productRelations);

                var targetTable = "uCommerce_ProductRelation";

                var sql = $@"--Synchronize the target table with refreshed data from source table
                            MERGE {targetTable} AS TARGET
                            USING #tmp AS SOURCE 
                            ON (TARGET.ProductId = SOURCE.ProductId and 
                                TARGET.RelatedProductId = SOURCE.RelatedProductId and 
                                TARGET.ProductRelationTypeId = SOURCE.ProductRelationTypeId)

                            --When no records are matched, insert the incoming records from source table to target table
                            WHEN NOT MATCHED BY TARGET 
                            THEN INSERT ([ProductId],[RelatedProductId],[ProductRelationTypeId],[Guid])
                                VALUES (SOURCE.[ProductId],SOURCE.[RelatedProductId],SOURCE.[ProductRelationTypeId],SOURCE.[Guid])

                            WHEN NOT MATCHED BY SOURCE AND TARGET.ProductId IN (SELECT ProductId FROM #tmp) 
                            THEN DELETE;";

                connection.Execute(sql);
            }
        }

        public void DeleteProducts(List<int> productIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                foreach (var productIdBatch in productIds.Batch(2000))
                {
                    connection.Execute(@"
                        BEGIN TRANSACTION;
                        DELETE pp FROM [uCommerce_ProductDescriptionProperty] pp
                            JOIN [uCommerce_ProductDescription] pd ON pp.ProductDescriptionId = pd.ProductDescriptionId
                            WHERE ProductId IN @productIds
                        DELETE FROM [uCommerce_ProductDescription] WHERE ProductId IN @productIds;
                        DELETE FROM [uCommerce_CategoryProductRelation] WHERE ProductId IN @productIds;
                        DELETE FROM [uCommerce_ProductRelation] WHERE ProductId IN @productIds;
                        DELETE FROM [uCommerce_ProductProperty] WHERE ProductId IN @productIds;
                        DELETE FROM [uCommerce_ProductPrice] WHERE ProductId IN @productIds;
                        DELETE FROM [uCommerce_Product] WHERE ProductId IN @productIds;
                        COMMIT TRANSACTION;", new { productIds = productIdBatch });
                }
            }
        }

        public void DeleteCategories(List<int> categoryIds)
        {
            using (var connection = DBUtility.GetOpenConnection())
            {
                foreach (var categoryIdBatch in categoryIds.Batch(2000))
                {
                    connection.Execute(@"
                        BEGIN TRANSACTION;
                        DELETE FROM [uCommerce_CategoryDescription] WHERE [CategoryId] IN @categoryIds;
                        DELETE FROM [uCommerce_CategoryProperty] WHERE [CategoryId] IN @categoryIds;
                        DELETE FROM [uCommerce_Category] WHERE [CategoryId] IN @categoryIds;
                        COMMIT TRANSACTION;", new {categoryIds = categoryIdBatch });
                }
            }
        }

        public void IndexProducts(List<int> productIds)
        {
            SendIndexRequest(productIds, Settings.Ucommerce.IndexProductsApiUrl);
        }

        public void IndexCategories(List<int> categoryIds)
        {
            SendIndexRequest(categoryIds, Settings.Ucommerce.IndexCategoriesApiUrl);
        }

        private static void SendIndexRequest(List<int> categoryIds, string indexUrl)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("APIKEY", Settings.Ucommerce.IndexingApiKey);
            var response = client.PostAsync(indexUrl,
                new StringContent(JsonConvert.SerializeObject(categoryIds),
                    Encoding.UTF8,
                    "application/json")).GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();
        }
    }
}
