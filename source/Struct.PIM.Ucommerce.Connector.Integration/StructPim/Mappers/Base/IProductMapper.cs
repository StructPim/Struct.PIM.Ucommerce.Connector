using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Product;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base
{
    public interface IProductMapper
    {
        UcProductModel ToProduct(ProductModel productModel, ProductAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias);
    }
}