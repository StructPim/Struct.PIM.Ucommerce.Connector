using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration
{
    public class ProductIntegration
    {
        private readonly PimBroker _pimBroker;
        private readonly UcommerceBroker _ucommerceBroker;
        private readonly List<string> _cultureCodes = Settings.CultureCodes;
        private readonly Dictionary<Guid, string> _pimProductStructureToUcProductDefinition = Settings.PimProductStructureToUcProductDefinitionMap;
        private readonly int _batchSize = Settings.BatchSize;

        public ProductIntegration(PimBroker pimBroker, UcommerceBroker ucommerceBroker)
        {
            _ucommerceBroker = ucommerceBroker;
            _pimBroker = pimBroker;
        }

        public void CreateOrUpdateProducts(List<int> pimProductIds)
        {
            Console.WriteLine($"Products to update: {pimProductIds.Count}");
            var totalProductsUpdated = 0;
            var ucProductDefinitionFieldIdMap = _ucommerceBroker.GetProductDefinitionFieldIdByKey();
            var allPimProducts = new List<UcProductModel>();

            foreach (var pimProductIdBatch in pimProductIds.Batch(_batchSize))
            {
                var sw = Stopwatch.StartNew();
                var pimProducts = _pimBroker.GetProducts(pimProductIdBatch);
                allPimProducts.AddRange(pimProducts);
                var ucProductIdByPimId = _ucommerceBroker.GetProductIdByPimId(pimProductIdBatch);
                var pimClassifications = _pimBroker.GetProductClassifications(pimProductIdBatch);
                var ucCategoryIdByPimId = _ucommerceBroker.GetCategoryIdByPimId();
                var ucProductDefinitionMap = _ucommerceBroker.GetProductDefinitionIdByName();

                using (var transaction = new TransactionScope())
                {
                    UpdateBaseValues(pimProducts, ucProductIdByPimId, ucProductDefinitionMap);

                    UpdateProperties(pimProducts, ucProductIdByPimId, ucProductDefinitionFieldIdMap);

                    UpdateDescriptions(pimProducts, ucProductIdByPimId);

                    UpdateLocalizedProperties(pimProducts, ucProductIdByPimId, ucProductDefinitionFieldIdMap);

                    UpdateCategoryRelations(pimClassifications, ucProductIdByPimId, ucCategoryIdByPimId);

                    transaction.Complete();
                }

                _ucommerceBroker.IndexProducts(ucProductIdByPimId.Values.ToList());

                totalProductsUpdated += pimProducts.Count;
                Console.WriteLine($"Updated {pimProducts.Count} products in {sw.ElapsedMilliseconds} ms. ({totalProductsUpdated} of {pimProductIds.Count})");
            }

            // Product relations cannot be created before all products are created
            Console.WriteLine("Updating product relations");
            var ucProductRelationTypeId = _ucommerceBroker.GetProductRelationTypeId(Settings.Ucommerce.DefaultProductRelationTypeName);

            foreach (var pimProducts in allPimProducts.Batch(_batchSize))
            {
                var pimProductIdBatch = pimProducts.Select(x => x.ProductId).ToList();
                var ucProductIdByPimId = _ucommerceBroker.GetProductIdByPimId(pimProductIdBatch);
                UpdateProductRelations(pimProducts, ucProductIdByPimId, ucProductRelationTypeId);
            }
        }

        private void UpdateBaseValues(List<UcProductModel> pimProducts, Dictionary<int, int> ucProductIdByPimId, Dictionary<string, int> ucProductDefinitionMap)
        {
            var ucProducts = new List<UcProduct>();
            var ucProductGuidByPimProductId = new Dictionary<int, Guid>();

            foreach (var pimProduct in pimProducts)
            {
                var productDefinitionName = _pimProductStructureToUcProductDefinition[pimProduct.ProductStructureUid];
                ucProductIdByPimId.TryGetValue(pimProduct.ProductId, out var ucProductId);

                var sku = pimProduct.Sku;
                var product = new UcProduct
                {
                    ProductId = ucProductId,
                    Name = pimProduct.Name ?? string.Empty,
                    Sku = !string.IsNullOrWhiteSpace(sku) ? sku : pimProduct.ProductId.ToString(),
                    ProductDefinitionId = ucProductDefinitionMap[productDefinitionName],
                    DisplayOnSite = true,
                    AllowOrdering = true,
                    PrimaryImageMediaId = pimProduct.PrimaryImageMediaId,
                    ThumbnailImageMediaId = pimProduct.PrimaryImageMediaId,
                    ModifiedBy = "PIM Integration",
                    ModifiedOn = DateTime.Now,
                    CreatedBy = "PIM Integration",
                    CreatedOn = DateTime.Now
                };

                ucProductGuidByPimProductId.Add(pimProduct.ProductId, product.Guid);
                ucProducts.Add(product);
            }

            // Update ucProductIdByPimId with created products
            var updateResult = _ucommerceBroker.UpsertProducts(ucProducts);
            var ucProductIdByProductGuid = updateResult.ToDictionary(x => x.Guid, y => y.ItemId);

            foreach (var pimProduct in pimProducts)
            {
                if (!ucProductIdByPimId.ContainsKey(pimProduct.ProductId))
                {
                    ucProductIdByPimId.Add(pimProduct.ProductId, ucProductIdByProductGuid[ucProductGuidByPimProductId[pimProduct.ProductId]]);
                }
            }
        }

        private void UpdateDescriptions(List<UcProductModel> pimProducts, Dictionary<int, int> ucProductIdByPimId)
        {
            var productDescriptions = new List<UcProductDescription>();

            foreach (var pimProduct in pimProducts)
            {
                var ucProductId = ucProductIdByPimId[pimProduct.ProductId];

                foreach (var cultureCode in _cultureCodes)
                {
                    var productDescription = new UcProductDescription
                    {
                        ProductId = ucProductId,
                        DisplayName = pimProduct.GetDisplayName(cultureCode) ?? string.Empty,
                        ShortDescription = pimProduct.GetShortDescription(cultureCode),
                        LongDescription = pimProduct.GetLongDescription(cultureCode),
                        CultureCode = cultureCode
                    };

                    productDescriptions.Add(productDescription);
                }
            }

            _ucommerceBroker.UpsertProductDescriptions(productDescriptions);
        }

        private void UpdateProperties(List<UcProductModel> pimProducts, Dictionary<int, int> ucProductIdByPimId, Dictionary<string, int> ucProductDefinitionFieldIdMap)
        {
            var productProperties = new List<UcProductProperty>();

            foreach (var pimProduct in pimProducts)
            {
                var ucProductId = ucProductIdByPimId[pimProduct.ProductId];
                var productDefinitionName = _pimProductStructureToUcProductDefinition[pimProduct.ProductStructureUid];
                var properties = pimProduct.GetProperties();
                properties.Add("PimID", pimProduct.ProductId.ToString());

                foreach (var property in properties)
                {
                    if (!ucProductDefinitionFieldIdMap.ContainsKey(productDefinitionName + "_" + property.Key))
                    {
                        Console.WriteLine("ProductDefinitionField is missing for: " + productDefinitionName + "_" + property.Key);
                        continue;
                    }

                    var productProperty = new UcProductProperty
                    {
                        ProductId = ucProductId,
                        ProductDefinitionFieldId = ucProductDefinitionFieldIdMap[productDefinitionName + "_" + property.Key],
                        Value = property.Value ?? string.Empty
                    };

                    productProperties.Add(productProperty);
                }
            }

            _ucommerceBroker.UpsertProductProperties(productProperties);
        }

        private void UpdateLocalizedProperties(List<UcProductModel> pimProducts, Dictionary<int, int> ucProductIdByPimId, Dictionary<string, int> ucProductDefinitionFieldIdMap)
        {
            var ucProductIds = ucProductIdByPimId.Values.ToList();
            var ucProductDescriptionIdByKey = _ucommerceBroker.GetProductDescriptionIdByKey(ucProductIds);
            var productDescriptionProperties = new List<UcProductDescriptionProperty>();

            foreach (var pimProduct in pimProducts)
            {
                var productDefinitionName = _pimProductStructureToUcProductDefinition[pimProduct.ProductStructureUid];
                var ucProductId = ucProductIdByPimId[pimProduct.ProductId];

                foreach (var cultureCode in _cultureCodes)
                {
                    var ucProductDescriptionId = ucProductDescriptionIdByKey[ucProductId + "_" + cultureCode];

                    foreach (var property in pimProduct.GetLocalizedProperties(cultureCode))
                    {
                        if (!ucProductDefinitionFieldIdMap.ContainsKey(productDefinitionName + "_" + property.Key))
                        {
                            Console.WriteLine("ProductDefinitionField is missing for: " + productDefinitionName + "_" + property.Key);
                            continue;
                        }

                        var productProperty = new UcProductDescriptionProperty
                        {
                            ProductDescriptionId = ucProductDescriptionId,
                            ProductDefinitionFieldId = ucProductDefinitionFieldIdMap[productDefinitionName + "_" + property.Key],
                            Value = property.Value ?? string.Empty
                        };

                        productDescriptionProperties.Add(productProperty);
                    }
                }
            }

            _ucommerceBroker.UpsertProductLocalizedProperties(productDescriptionProperties);
        }

        private void UpdateCategoryRelations(Dictionary<int, List<ProductClassificationModel>> classifications, 
            Dictionary<int, int> ucProductIdByPimId, Dictionary<int, int> ucCategoryIdByPimId)
        {
            var products = new List<UcCategoryProductRelation>();

            foreach (var classification in classifications)
            {
                foreach (var classificationModel in classification.Value)
                {
                    if (ucCategoryIdByPimId.ContainsKey(classificationModel.CategoryId))
                    {
                        var relation = new UcCategoryProductRelation
                        {
                            ProductId = ucProductIdByPimId[classification.Key],
                            CategoryId = ucCategoryIdByPimId[classificationModel.CategoryId],
                            SortOrder = classificationModel.SortOrder.GetValueOrDefault()
                        };

                        products.Add(relation);
                    }
                }
            }

            _ucommerceBroker.UpsertCategoryProductRelations(products);
        }

        private void UpdateProductRelations(List<UcProductModel> pimProducts, Dictionary<int, int> ucProductIdByPimId, int ucProductRelationTypeId)
        {
            var relations = new List<UcProductRelation>();

            foreach (var pimProduct in pimProducts)
            {
                var ucProductId = ucProductIdByPimId[pimProduct.ProductId];

                foreach (var relatedProductId in pimProduct.GetRelatedProducts() ?? Enumerable.Empty<int>())
                {
                    if (ucProductIdByPimId.ContainsKey(relatedProductId))
                    {
                        var relation = new UcProductRelation
                        {
                            ProductId = ucProductId,
                            RelatedProductId = ucProductIdByPimId[relatedProductId],
                            ProductRelationTypeId = ucProductRelationTypeId
                        };

                        relations.Add(relation);
                    }
                }
            }

            _ucommerceBroker.UpsertProductRelations(relations);
        }

        public void DeleteProducts(List<int> pimProductIds)
        {
            var ucProductIdByPimId = _ucommerceBroker.GetProductIdByPimId(pimProductIds);
            _ucommerceBroker.DeleteProducts(ucProductIdByPimId.Values.ToList());
        }
    }
}
