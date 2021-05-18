using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class EventProductMapper : IProductMapper
    {
        public UcProductModel ToProduct(ProductModel productModel, ProductAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<EventProductModel>();

            var ucProduct = new UcProductModel(productModel);
            ucProduct.Name = values.Name.Get(Settings.DefaultCultureCode);

            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucProduct.DisplayName.Add(cultureCode, values.Name.Get(cultureCode));
                ucProduct.ShortDescription.Add(cultureCode, values.ShortDescription.Get(cultureCode, Settings.Pim.DefaultSegment));
                ucProduct.LongDescription.Add(cultureCode, values.LongDescription.Get(cultureCode, Settings.Pim.DefaultSegment));
            }
            
            ucProduct.PrimaryImageMediaId = values.PrimaryImage;

            return ucProduct;
        }
    }
}
