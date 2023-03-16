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
    public class CategoryIntegration
    {
        private readonly PimBroker _pimBroker;
        private readonly UcommerceBroker _ucommerceBroker;
        private readonly List<string> _cultureCodes = Settings.CultureCodes;

        public CategoryIntegration(PimBroker pimBroker, UcommerceBroker ucommerceBroker)
        {
            _pimBroker = pimBroker;
            _ucommerceBroker = ucommerceBroker;
        }

        public void CreateOrUpdateCategories(List<int> pimCategoryIds)
        {
            Console.WriteLine($"Categories to update: {pimCategoryIds.Count}");
            var totalCategoriesUpdated = 0;
            var sw = Stopwatch.StartNew();
            var pimCategories = _pimBroker.GetCategories(pimCategoryIds);
            var ucCategoryDefinitionId = _ucommerceBroker.GetDefinitionId(Settings.Ucommerce.DefaultCategoryDefinitionName);
            var ucProductCatalogIdByName = _ucommerceBroker.GetProductCatalogIdByName();
            var ucCategoryIdByPimId = _ucommerceBroker.GetCategoryIdByPimId();
            var ucDefinitionFieldMap = _ucommerceBroker.GetDefinitionFieldIdByKey();

            // Create categories top-down: Parent categories needs to be created before child categories
            var pimCategoriesByParentId = pimCategories.ToLookup(x => x.ParentId);

            // Start with the top-most nodes (parent=null or parent not in update)
            var pimCategoryIdSet = pimCategories.Select(x => x.Id).ToHashSet();
            var pimCategoryChildren = pimCategoriesByParentId
                .Where(x => x.Key == null || !pimCategoryIdSet.Contains(x.Key.Value))
                .SelectMany(x => x)
                .ToList();
            
            using (var transaction = new TransactionScope())
            {
                while (pimCategoryChildren.Any())
                {
                    CreateOrUpdateCategories(pimCategoryChildren, ucCategoryDefinitionId, ucProductCatalogIdByName, ucCategoryIdByPimId);

                    // Proceed to the next layer in the tree
                    pimCategoryChildren = pimCategoryChildren.SelectMany(x => pimCategoriesByParentId[x.Id]).ToList();
                }

                UpdateCategoryDescriptions(pimCategories, ucCategoryIdByPimId);

                UpdateCategoryProperties(pimCategories, ucCategoryIdByPimId, ucDefinitionFieldMap);

                transaction.Complete();
            }

            _ucommerceBroker.IndexCategories(ucCategoryIdByPimId.Values.ToList());

            totalCategoriesUpdated += pimCategories.Count;
            Console.WriteLine($"Updated {pimCategories.Count} categories in {sw.ElapsedMilliseconds} ms. ({totalCategoriesUpdated} of {pimCategoryIds.Count})");
        }

        /// <summary>
        /// Create or update categories
        /// Precondition: All parent categories exist
        /// </summary>
        private void CreateOrUpdateCategories(List<UcCategoryModel> pimCategories, int ucCategoryDefinitionId,
            Dictionary<string, int> ucProductCatalogIdByName, Dictionary<int, int> ucCategoryIdByPimId)
        {
            var ucCategories = new List<UcCategory>();
            var ucCategoryGuidByPimId = new Dictionary<int, Guid>();

            foreach (var pimCategory in pimCategories)
            {
                var catalogId = ucProductCatalogIdByName[Settings.PimCatalogueToUcCatalogMap[pimCategory.CatalogueUid]];
                ucCategoryIdByPimId.TryGetValue(pimCategory.Id, out var categoryId);
                ucCategoryIdByPimId.TryGetValue(pimCategory.ParentId.GetValueOrDefault(), out var parentCategoryId);

                var ucCategory = new UcCategory
                {
                    CategoryId = categoryId,
                    Name = pimCategory.Name.Get(Settings.DefaultCultureCode),
                    SortOrder = pimCategory.SortOrder,
                    DefinitionId = ucCategoryDefinitionId,
                    ParentCategoryId = parentCategoryId != 0 ? parentCategoryId : default(int?),
                    ProductCatalogId = catalogId,
                    ImageMediaId = pimCategory.ImageMediaId,
                    DisplayOnSite = true,
                    Deleted = false,
                    CreatedBy = "PIM Integration",
                    CreatedOn = DateTime.Now,
                    ModifiedBy = "PIM Integration",
                    ModifiedOn = DateTime.Now
                };

                ucCategoryGuidByPimId.Add(pimCategory.Id, ucCategory.Guid);
                ucCategories.Add(ucCategory);
            }

            // Update ucCategoryIdByPimId-map with created categories
            var updateResult = _ucommerceBroker.UpsertCategories(ucCategories);
            var ucCategoryIdByGuid = updateResult.ToDictionary(x => x.Guid, y => y.ItemId);

            foreach (var pimCategory in pimCategories)
            {
                if (!ucCategoryIdByPimId.ContainsKey(pimCategory.Id))
                {
                    ucCategoryIdByPimId.Add(pimCategory.Id, ucCategoryIdByGuid[ucCategoryGuidByPimId[pimCategory.Id]]);
                }
            }
        }

        private void UpdateCategoryProperties(List<UcCategoryModel> pimCategories, Dictionary<int, int> ucCategoryIdByPimId, Dictionary<string, int> ucDefinitionFieldMap)
        {
            var categoryProperties = new List<UcCategoryProperty>();

            foreach (var pimCategory in pimCategories)
            {
                var ucCategoryId = ucCategoryIdByPimId[pimCategory.Id];
                var properties = pimCategory.GetProperties();
                properties.Add("PimID", pimCategory.Id.ToString());

                foreach (var property in properties)
                {
                    var productProperty = ToCategoryProperty(ucDefinitionFieldMap, ucCategoryId, property, null);
                    categoryProperties.Add(productProperty);
                }

                foreach (var cultureCode in _cultureCodes)
                {
                    var localizedProperties = pimCategory.GetLocalizedProperties(cultureCode);

                    foreach (var localizedProperty in localizedProperties)
                    {
                        var productProperty = ToCategoryProperty(ucDefinitionFieldMap, ucCategoryId, localizedProperty, cultureCode);
                        categoryProperties.Add(productProperty);
                    }
                }
            }

            _ucommerceBroker.UpsertCategoryProperties(categoryProperties);
        }

        private static UcCategoryProperty ToCategoryProperty(Dictionary<string, int> ucDefinitionFieldMap, int ucCategoryId,
            KeyValuePair<string, string> localizedProperty, string cultureCode)
        {
            var productProperty = new UcCategoryProperty
            {
                CategoryId = ucCategoryId,
                DefinitionFieldId = ucDefinitionFieldMap[Settings.Ucommerce.DefaultCategoryDefinitionName + "_" + localizedProperty.Key],
                Value = localizedProperty.Value,
                CultureCode = cultureCode
            };
            return productProperty;
        }

        private void UpdateCategoryDescriptions(List<UcCategoryModel> pimCategories, Dictionary<int, int> ucCategoryIdByPimId)
        {
            var categoryDescriptions = new List<UcCategoryDescription>();

            foreach (var pimCategory in pimCategories)
            {
                var ucCategoryId = ucCategoryIdByPimId[pimCategory.Id];

                foreach (var cultureCode in _cultureCodes)
                {
                    var categoryDescription = new UcCategoryDescription
                    {
                        CategoryId = ucCategoryId,
                        DisplayName = pimCategory.Name.Get(cultureCode) ?? pimCategory.Name.Get(Settings.DefaultCultureCode),
                        Description = pimCategory.GetDescription(cultureCode),
                        CultureCode = cultureCode
                    };

                    categoryDescriptions.Add(categoryDescription);
                }
            }

            _ucommerceBroker.UpsertCategoryDescriptions(categoryDescriptions);
        }

        public void DeleteCategories(List<int> pimCategoryIds)
        {
            var ucToPimCategoryId = _ucommerceBroker.GetCategoryIdByPimId(pimCategoryIds);
            _ucommerceBroker.DeleteCategories(ucToPimCategoryId.Values.ToList());
        }
    }
}
