using System.Collections.Generic;
using System.Globalization;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;
using Attribute = Struct.PIM.Api.Models.Attribute.Attribute;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class NonFoodProductMapper : IProductMapper
    {
        public UcProductModel ToProduct(ProductModel productModel, ProductAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<NonfoodProductModel>();

            var ucProduct = new UcProductModel(productModel);
            ucProduct.Name = values.Name.Get(Settings.DefaultCultureCode);

            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucProduct.DisplayName.Add(cultureCode, values.Name.Get(cultureCode));
                ucProduct.ShortDescription.Add(cultureCode, values.ShortDescription.Get(cultureCode, Settings.Pim.DefaultSegment));
                ucProduct.LongDescription.Add(cultureCode, values.LongDescription.Get(cultureCode, Settings.Pim.DefaultSegment));

                ucProduct.AddLocalizedProperty(nameof(values.SalesUnit), cultureCode, values.SalesUnit?.Name.Get(cultureCode));
                ucProduct.AddLocalizedProperty(nameof(values.Brand), cultureCode, values.Brand?.Name);
            }
            
            ucProduct.Properties.Add(nameof(values.TaxPositionNo), values.TaxPositionNo);
            ucProduct.Properties.Add(nameof(values.PackageSize), values.PackageSize != null ? values.PackageSize.Width + " x " + values.PackageSize.Length + " x " + values.PackageSize.Height : string.Empty);
            ucProduct.Properties.Add(nameof(values.PackageWeight), values.PackageWeight?.ToString(CultureInfo.InvariantCulture));

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
