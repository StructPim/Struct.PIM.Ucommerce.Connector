using System.Collections.Generic;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;
using Attribute = Struct.PIM.Api.Models.Attribute.Attribute;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class WineProductMapper : IProductMapper
    {
        public UcProductModel ToProduct(ProductModel productModel, ProductAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<WineProductModel>();

            var ucProduct = new UcProductModel(productModel);
            ucProduct.Sku = values.SKU;
            ucProduct.Name = values.Name.Get(Settings.DefaultCultureCode);

            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucProduct.DisplayName.Add(cultureCode, values.Name.Get(cultureCode));
                ucProduct.ShortDescription.Add(cultureCode, values.ShortDescription.Get(cultureCode, Settings.Pim.DefaultSegment));
                ucProduct.LongDescription.Add(cultureCode, values.LongDescription.Get(cultureCode, Settings.Pim.DefaultSegment));

                ucProduct.AddLocalizedProperty(nameof(values.SalesUnit), cultureCode, values.SalesUnit?.Name?.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.WineManufacturer), cultureCode, values.WineManufacturer.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.Country), cultureCode, values.Country.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.Region), cultureCode, values.Region.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.District), cultureCode, values.District?.Name);
                ucProduct.AddLocalizedProperty(nameof(values.WineMaker), cultureCode, values.WineMaker?.Name);
                ucProduct.AddLocalizedProperty(nameof(values.SealingSystem), cultureCode, values.SealingSystem.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.PackagingType), cultureCode, values.PackagingType.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.Grapes), cultureCode, attributesByAlias[nameof(values.Grapes)].RenderFirstValue(attributeValues, cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.VinificationTags), cultureCode, attributesByAlias[nameof(values.VinificationTags)].RenderFirstValue(attributeValues, cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.BottleStorageTags), cultureCode, attributesByAlias[nameof(values.BottleStorageTags)].RenderFirstValue(attributeValues, cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.CharacteristicaTags), cultureCode, attributesByAlias[nameof(values.CharacteristicaTags)].RenderFirstValue(attributeValues, cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.WellSuitedForTags), cultureCode, attributesByAlias[nameof(values.WellSuitedForTags)].RenderFirstValue(attributeValues, cultureCode));
            }

            ucProduct.Properties.Add(nameof(values.TaxPositionNo), values.TaxPositionNo);
            ucProduct.Properties.Add(nameof(values.PackageSize), values.PackageSize != null ? values.PackageSize.Width + " x " + values.PackageSize.Length + " x " + values.PackageSize.Height : string.Empty);
            ucProduct.Properties.Add(nameof(values.PackageWeight), values.PackageWeight?.ToString());
            ucProduct.Properties.Add(nameof(values.LimitedBatch), values.LimitedBatch?.ToString());
            ucProduct.Properties.Add(nameof(values.Year), values.Year?.ToString());
            ucProduct.Properties.Add(nameof(values.FineWine), values.FineWine?.ToString());
            ucProduct.Properties.Add(nameof(values.NetContent), attributesByAlias[nameof(values.NetContent)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucProduct.Properties.Add(nameof(values.WineField), values.WineField?.Name);
            ucProduct.Properties.Add(nameof(values.Alcohol), values.Alcohol?.ToString());
            ucProduct.Properties.Add(nameof(values.BottlesProduced), values.BottlesProduced?.ToString());
            ucProduct.Properties.Add(nameof(values.ServingTemperature), attributesByAlias[nameof(values.ServingTemperature)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucProduct.Properties.Add(nameof(values.PHValue), values.PHValue?.ToString());
            ucProduct.Properties.Add(nameof(values.ResidualSugar), values.ResidualSugar?.ToString());
            ucProduct.Properties.Add(nameof(values.Acid), values.Acid?.ToString());
            ucProduct.Properties.Add(nameof(values.IsOrganic), values.IsOrganic?.ToString());
            ucProduct.Properties.Add(nameof(values.Fat), values.Fat?.ToString());
            ucProduct.Properties.Add(nameof(values.SaturatedFat), values.SaturatedFat?.ToString());
            ucProduct.Properties.Add(nameof(values.MonounsaturatedFat), values.MonounsaturatedFat?.ToString());

            // Related products
            ucProduct.RelatedProducts.AddRange(values.Accessories ?? new List<int>(0));
            ucProduct.RelatedProducts.AddRange(values.Upsale ?? new List<int>(0));
            ucProduct.RelatedProducts.AddRange(values.CrossSale ?? new List<int>(0));
            ucProduct.RelatedProducts.AddRange(values.Complimentary ?? new List<int>(0));

            ucProduct.PrimaryImageMediaId = values.PrimaryImage;

            return ucProduct;
        }
    }
}