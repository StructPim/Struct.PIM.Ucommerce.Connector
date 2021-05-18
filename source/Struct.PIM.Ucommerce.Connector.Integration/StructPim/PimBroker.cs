using System;
using System.Collections.Generic;
using System.Linq;
using Struct.PIM.Api.Client;
using Struct.PIM.Api.Models.Catalogue;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Api.Models.Shared;
using Struct.PIM.Api.Models.Variant;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim
{
    public class PimBroker
    {
        private readonly StructPIMApiClient _pimApiClient;

        public PimBroker(StructPIMApiClient pimApiClient)
        {
            _pimApiClient = pimApiClient;
        }

        public static PimBroker Instance { get; } = new PimBroker(new StructPIMApiClient(Settings.Pim.Url, Settings.Pim.ApiKey));

        public List<CategoryModel> GetUpdatedCategoriesInCatalogue(Guid pimCatalogueUid, DateTimeOffset? lastUpdate)
        {
            var catalogueResult = _pimApiClient.Catalogues.GetCatalogueDescendants(pimCatalogueUid);
            return catalogueResult.Categories.Where(x => lastUpdate == null || x.LastModified > lastUpdate).ToList();
        }

        public List<int> GetUpdatedProductIds(Guid pimCatalogueUid, DateTimeOffset? lastUpdate)
        {
            return _pimApiClient.Products.Search(CreateLastModifiedSearchModel(pimCatalogueUid, lastUpdate));
        }

        public List<int> GetUpdatedVariantIds(Guid pimCatalogueUid, DateTimeOffset? lastUpdate)
        {
            return _pimApiClient.Variants.Search(CreateLastModifiedSearchModel(pimCatalogueUid, lastUpdate));
        }

        private SearchModel CreateLastModifiedSearchModel(Guid pimCatalogueUid, DateTimeOffset? lastUpdate)
        {
            return new SearchModel
            {
                QueryModel = new SimpleQueryModel
                {
                    BooleanOperator = BooleanOperator.And,
                    Filters = new List<FieldFilterModel>
                    {
                        new FieldFilterModel
                        {
                            FieldUid = "PIM_LastModified",
                            FilterValue = lastUpdate,
                            QueryOperator = QueryOperator.LargerThan
                        },
                        new FieldFilterModel
                        {
                            FieldUid = "PIM_Catalogue_" + pimCatalogueUid,
                            FilterValue = pimCatalogueUid,
                            QueryOperator = QueryOperator.IsNotEmpty
                        }
                    }
                }
            };
        }

        public Dictionary<int, List<ProductClassificationModel>> GetProductClassifications(List<int> productIds)
        {
            return _pimApiClient.Products.GetProductClassifications(productIds);
        }

        public List<UcProductModel> GetProducts(List<int> pimProductIds)
        {
            var productModels = _pimApiClient.Products.GetProducts(pimProductIds);
            var productAttributeValuesModels = _pimApiClient.Products.GetProductAttributeValues(new ProductValuesRequestModel
            {
                ProductIds = pimProductIds,
                // NOTE: Possible performance optimization, specify subset of attributes to minimize request
                //IncludeValues = ValueIncludeMode.Aliases,
                //Aliases = new List<string> { "SKU", "NAME", "DESCRIPTION" }
            });
            var productStructuresByProductId = productModels.ToDictionary(x => x.Id, y => y.ProductStructureUid);
            var productsById = productModels.ToDictionary(x => x.Id);
            var attributesByAlias = _pimApiClient.Attributes.GetAttributes().ToDictionary(x => x.Alias);
            var result = new List<UcProductModel>();

            foreach (var productAttributeValuesModel in productAttributeValuesModels)
            {
                var productModel = productsById[productAttributeValuesModel.ProductId];
                var pimProductStructureUid = productStructuresByProductId[productAttributeValuesModel.ProductId];
                var mapper = MapperFactory.CreateProductMapper(pimProductStructureUid);
                var mapped = mapper.ToProduct(productModel, productAttributeValuesModel, attributesByAlias);
                result.Add(mapped);
            }

            return result;
        }

        public List<UcVariantModel> GetVariants(List<int> pimVariantIds)
        {
            var variantModels = _pimApiClient.Variants.GetVariants(pimVariantIds);
            var variantAttributeValuesModels = _pimApiClient.Variants.GetVariantAttributeValues(new VariantValuesRequestModel
            {
                VariantIds = pimVariantIds,
                // NOTE: Possible performance optimization, specify subset of attributes to minimize request
                //IncludeValues = ValueIncludeMode.Aliases,
                //Aliases = new List<string> { "COLOR", "SKU" }
            });
            var productStructuresByVariantId = variantModels.ToDictionary(x => x.Id, y => y.ProductStructureUid);
            var variantsById = variantModels.ToDictionary(x => x.Id);
            var attributesByAlias = _pimApiClient.Attributes.GetAttributes().ToDictionary(x => x.Alias);
            var result = new List<UcVariantModel>();

            foreach (var variantAttributeValuesModel in variantAttributeValuesModels)
            {
                var variantModel = variantsById[variantAttributeValuesModel.VariantId];
                var pimProductStructureUid = productStructuresByVariantId[variantAttributeValuesModel.VariantId];
                var mapper = MapperFactory.CreateVariantMapper(pimProductStructureUid);
                var mapped = mapper.ToVariant(variantModel, variantAttributeValuesModel, attributesByAlias);
                result.Add(mapped);
            }

            return result;
        }

        public List<UcCategoryModel> GetCategories(List<int> pimCategoryIds)
        {
            var categoryModels = _pimApiClient.Catalogues.GetCategories(pimCategoryIds);
            var categoryAttributes = _pimApiClient.Catalogues.GetCategoryAttributeValues(
                new CategoryValueRequestModel { CategoryIds = pimCategoryIds }).ToDictionary(x => x.CategoryId);
            var attributesByAlias = _pimApiClient.Attributes.GetAttributes().ToDictionary(x => x.Alias);
            var result = new List<UcCategoryModel>();

            foreach (var categoryModel in categoryModels)
            {
                var values = categoryAttributes[categoryModel.Id];
                var mapper = MapperFactory.CreateCategoryMapper(categoryModel.CatalogueUid);
                var mapped = mapper.ToCategory(categoryModel, values, attributesByAlias);
                result.Add(mapped);
            }

            return result;
        }
    }
}
