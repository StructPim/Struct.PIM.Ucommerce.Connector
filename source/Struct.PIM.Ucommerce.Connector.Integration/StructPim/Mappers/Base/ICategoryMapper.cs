using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Catalogue;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base
{
    public interface ICategoryMapper
    {
        UcCategoryModel ToCategory(CategoryModel categoryModel, CategoryAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias);
    }
}