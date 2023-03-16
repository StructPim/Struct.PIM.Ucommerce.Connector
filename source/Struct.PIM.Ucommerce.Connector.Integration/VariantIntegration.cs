using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration
{
    public class VariantIntegration
    {
        private readonly PimBroker _pimBroker;
        private readonly UcommerceBroker _ucommerceBroker;
        private readonly List<string> _cultureCodes = Settings.CultureCodes;
        private readonly Dictionary<Guid, string> _pimProductStructureToUcProductDefinition = Settings.PimProductStructureToUcProductDefinitionMap;
        private readonly int _batchSize = Settings.BatchSize;

        public VariantIntegration(PimBroker pimBroker, UcommerceBroker ucommerceBroker)
        {
            _pimBroker = pimBroker;
            _ucommerceBroker = ucommerceBroker;
        }

        public void CreateOrUpdateVariants(List<int> pimVariantIds)
        {
            Console.WriteLine($"Variants to update: {pimVariantIds.Count}");
            var totalVariantsUpdated = 0;
            var ucProductDefinitionFieldIdMap = _ucommerceBroker.GetProductDefinitionFieldIdByKey();

            foreach (var pimVariantIdBatch in pimVariantIds.Batch(_batchSize))
            {
                var sw = Stopwatch.StartNew();
                var pimVariants = _pimBroker.GetVariants(pimVariantIdBatch);
                var pimProductIds = pimVariants.Select(x => x.ProductId).Distinct().ToList();
                var ucProductIdByPimId = _ucommerceBroker.GetProductIdByPimId(pimProductIds);
                var ucProductIds = ucProductIdByPimId.Values.ToList();
                var ucProductIdByVariantPimId = _ucommerceBroker.GetProductIdByVariantPimId(pimVariantIdBatch);
                var ucProductSkuByPimId = _ucommerceBroker.GetProductSkuByProductId(ucProductIds);
                var ucProductDefinitionMap = _ucommerceBroker.GetProductDefinitionIdByName();

                using (var transaction = new TransactionScope())
                {
                    UpdateBaseValues(pimVariants, ucProductIdByPimId, ucProductIdByVariantPimId, ucProductSkuByPimId, ucProductDefinitionMap);

                    UpdateProperties(pimVariants, ucProductIdByVariantPimId, ucProductDefinitionFieldIdMap);

                    UpdateDescriptions(pimVariants, ucProductIdByVariantPimId);

                    UpdateLocalizedProperties(pimVariants, ucProductIdByVariantPimId, ucProductDefinitionFieldIdMap);

                    transaction.Complete();
                }

                _ucommerceBroker.IndexProducts(ucProductIdByVariantPimId.Values.ToList());

                totalVariantsUpdated += pimVariants.Count;
                Console.WriteLine($"Updated {pimVariants.Count} variants in {sw.ElapsedMilliseconds} ms. ({totalVariantsUpdated} of {pimVariantIds.Count})");
            }
        }

        private void UpdateBaseValues(List<UcVariantModel> pimVariants, Dictionary<int, int> ucProductIdByPimId,
            Dictionary<int, int> ucProductIdByVariantPimId, Dictionary<int, string> ucProductSkuByPimId,
            Dictionary<string, int> ucProductDefinitionMap)
        {
            var ucProducts = new List<UcProduct>();
            var ucProductGuidByPimProductId = new Dictionary<int, Guid>();

            foreach (var pimVariant in pimVariants)
            {
                var productDefinitionName = _pimProductStructureToUcProductDefinition[pimVariant.ProductStructureUid];
                ucProductIdByVariantPimId.TryGetValue(pimVariant.VariantId, out var ucProductId);
                ucProductIdByPimId.TryGetValue(pimVariant.ProductId, out var ucParentProductId);
                var sku = ucProductSkuByPimId[ucParentProductId];
                var variantSku = pimVariant.VariantSku;

                var variant = new UcProduct
                {
                    ProductId = ucProductId,
                    Name = pimVariant.Name ?? string.Empty,
                    Sku = !string.IsNullOrWhiteSpace(sku) ? sku : pimVariant.ProductId.ToString(),
                    VariantSku = !string.IsNullOrWhiteSpace(variantSku) ? variantSku : pimVariant.VariantId.ToString(),
                    ParentProductId = ucParentProductId,
                    ProductDefinitionId = ucProductDefinitionMap[productDefinitionName],
                    DisplayOnSite = true,
                    AllowOrdering = true,
                    PrimaryImageMediaId = pimVariant.PrimaryImageMediaId,
                    ThumbnailImageMediaId = pimVariant.PrimaryImageMediaId,
                    ModifiedBy = "PIM Integration",
                    ModifiedOn = DateTime.Now,
                    CreatedBy = "PIM Integration",
                    CreatedOn = DateTime.Now
                };

                ucProductGuidByPimProductId.Add(pimVariant.VariantId, variant.Guid);
                ucProducts.Add(variant);
            }

            // Update ucProductIdByVariantPimId with created variants
            var updateResult = _ucommerceBroker.UpsertProducts(ucProducts);
            var ucProductIdByProductGuid = updateResult.ToDictionary(x => x.Guid, y => y.ItemId);

            foreach (var pimProduct in pimVariants)
            {
                if (!ucProductIdByVariantPimId.ContainsKey(pimProduct.VariantId))
                {
                    ucProductIdByVariantPimId.Add(pimProduct.VariantId, ucProductIdByProductGuid[ucProductGuidByPimProductId[pimProduct.VariantId]]);
                }
            }
        }

        private void UpdateDescriptions(List<UcVariantModel> pimVariants, Dictionary<int, int> ucProductIdByPimId)
        {
            var productDescriptions = new List<UcProductDescription>();

            foreach (var pimVariant in pimVariants)
            {
                var ucProductId = ucProductIdByPimId[pimVariant.VariantId];

                foreach (var cultureCode in _cultureCodes)
                {
                    var productDescription = new UcProductDescription
                    {
                        ProductId = ucProductId,
                        DisplayName = pimVariant.GetDisplayName(cultureCode) ?? string.Empty,
                        ShortDescription = pimVariant.GetShortDescription(cultureCode),
                        LongDescription = pimVariant.GetLongDescription(cultureCode),
                        CultureCode = cultureCode
                    };

                    productDescriptions.Add(productDescription);
                }
            }

            _ucommerceBroker.UpsertProductDescriptions(productDescriptions);
        }

        private void UpdateProperties(List<UcVariantModel> pimVariants, Dictionary<int, int> ucProductIdByPimId, Dictionary<string, int> ucProductDefinitionFieldIdMap)
        {
            var productProperties = new List<UcProductProperty>();

            foreach (var pimVariant in pimVariants)
            {
                var ucProductId = ucProductIdByPimId[pimVariant.VariantId];
                var productDefinitionName = _pimProductStructureToUcProductDefinition[pimVariant.ProductStructureUid];
                var properties = pimVariant.GetProperties();
                properties.Add("VariantPimID", pimVariant.VariantId.ToString());

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

        private void UpdateLocalizedProperties(List<UcVariantModel> pimVariants, Dictionary<int, int> ucProductIdByPimId, Dictionary<string, int> ucProductDefinitionFieldIdMap)
        {
            var ucProductIds = ucProductIdByPimId.Values.ToList();
            var ucProductDescriptionIdByKey = _ucommerceBroker.GetProductDescriptionIdByKey(ucProductIds);
            var productDescriptionProperties = new List<UcProductDescriptionProperty>();

            foreach (var pimVariant in pimVariants)
            {
                var ucProductDefinitionName = _pimProductStructureToUcProductDefinition[pimVariant.ProductStructureUid];
                var ucProductId = ucProductIdByPimId[pimVariant.VariantId];

                foreach (var cultureCode in _cultureCodes)
                {
                    var productDescriptionId = ucProductDescriptionIdByKey[ucProductId + "_" + cultureCode];

                    foreach (var property in pimVariant.GetLocalizedProperties(cultureCode))
                    {
                        if (!ucProductDefinitionFieldIdMap.ContainsKey(ucProductDefinitionName + "_" + property.Key))
                        {
                            Console.WriteLine("ProductDefinitionField is missing for: " + ucProductDefinitionName + "_" + property.Key);
                            continue;
                        }

                        var productProperty = new UcProductDescriptionProperty
                        {
                            ProductDescriptionId = productDescriptionId,
                            ProductDefinitionFieldId = ucProductDefinitionFieldIdMap[ucProductDefinitionName + "_" + property.Key],
                            Value = property.Value ?? string.Empty
                        };

                        productDescriptionProperties.Add(productProperty);
                    }
                }
            }

            _ucommerceBroker.UpsertProductLocalizedProperties(productDescriptionProperties);
        }

        public void DeleteVariants(List<int> pimVariantIds)
        {
            var ucProductIdByPimId = _ucommerceBroker.GetProductIdByVariantPimId(pimVariantIds);
            _ucommerceBroker.DeleteProducts(ucProductIdByPimId.Values.ToList());
        }
    }
}
