using System;
using System.Collections.Generic;
using Struct.PIM.Api.Models.Catalogue;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models
{
    public class UcCategoryModel
    {
        public UcCategoryModel(CategoryModel categoryModel)
        {
            Id = categoryModel.Id;
            ParentId = categoryModel.ParentId;
            CatalogueUid = categoryModel.CatalogueUid;
            SortOrder = categoryModel.SortOrder;

            Name = new Dictionary<string, string>();
            Description = new Dictionary<string, string>();
            Properties = new Dictionary<string, string>();
            LocalizedProperties = new Dictionary<string, Dictionary<string, string>>();
        }

        public Dictionary<string, string> Name { get; set; }
        public Dictionary<string, string> Description { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public Dictionary<string, Dictionary<string, string>> LocalizedProperties { get; set; }
        public int? ParentId { get; set; }
        public int Id { get; set; }
        public Guid CatalogueUid { get; set; }
        public int SortOrder { get; set; }
        public string ImageMediaId { get; set; }

        public string GetDescription(string cultureCode)
        {
            return Description.GetValueOrFallback(cultureCode, string.Empty); ;
        }

        public Dictionary<string, string> GetProperties()
        {
            return Properties ?? new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetLocalizedProperties(string cultureCode)
        {
            return new Dictionary<string, string> {};
        }
        
        public void AddLocalizedProperty(string propertyName, string cultureCode, string value)
        {
            if (!LocalizedProperties.TryGetValue(propertyName, out var attribute))
            {
                attribute = new Dictionary<string, string>();
                LocalizedProperties.Add(propertyName, attribute);
            }

            attribute.Add(cultureCode, value);
        }
    }
}
