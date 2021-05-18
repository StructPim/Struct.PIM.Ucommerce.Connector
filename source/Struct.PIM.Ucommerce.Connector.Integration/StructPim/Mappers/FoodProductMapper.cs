using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class FoodProductMapper : IProductMapper
    {
        public UcProductModel ToProduct(ProductModel productModel, ProductAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<FoodProductModel>();

            var ucProduct = new UcProductModel(productModel);
            ucProduct.Sku = values.SKU;
            ucProduct.Name = values.Name.Get(Settings.DefaultCultureCode);

            ucProduct.Properties.Add(nameof(values.PackageWeight), values.PackageWeight?.ToString());
            
            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucProduct.DisplayName.Add(cultureCode, values.Name.Get(cultureCode));
                ucProduct.ShortDescription.Add(cultureCode, values.ShortDescription.Get(cultureCode, Settings.Pim.DefaultSegment));
                ucProduct.LongDescription.Add(cultureCode, values.LongDescription.Get(cultureCode, Settings.Pim.DefaultSegment));

                ucProduct.AddLocalizedProperty(nameof(values.SalesUnit), cultureCode, values.SalesUnit?.Name.Get(cultureCode));
            }

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