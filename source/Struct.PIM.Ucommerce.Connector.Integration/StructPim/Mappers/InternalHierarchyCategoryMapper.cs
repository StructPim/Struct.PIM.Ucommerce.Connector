using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Catalogue;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class InternalHierarchyCategoryMapper : ICategoryMapper
    {
        public UcCategoryModel ToCategory(CategoryModel categoryModel, CategoryAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<InternalhierarchyCategoryModel>();

            var ucCategory = new UcCategoryModel(categoryModel);

            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucCategory.Name.Add(cultureCode, values.CategoryName.Get(cultureCode));
            }

            ucCategory.ImageMediaId = values.CategoryImage;

            return ucCategory;
        }
    }
}
