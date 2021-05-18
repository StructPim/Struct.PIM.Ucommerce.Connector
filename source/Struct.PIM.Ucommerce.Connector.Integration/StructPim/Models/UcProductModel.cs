using System;
using System.Collections.Generic;
using Struct.PIM.Api.Models.Product;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models
{
    public class UcProductModel
    {
        public UcProductModel(ProductModel productModel)
        {
            ProductId = productModel.Id;
            ProductStructureUid = productModel.ProductStructureUid;

            DisplayName = new Dictionary<string, string>();
            ShortDescription = new Dictionary<string, string>();
            LongDescription = new Dictionary<string, string>();
            Properties = new Dictionary<string, string>();
            LocalizedProperties = new Dictionary<string, Dictionary<string, string>>();
            RelatedProducts = new List<int>();
        }

        public int ProductId { get; set; }
        public Guid ProductStructureUid { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> DisplayName { get; set; }
        public Dictionary<string, string> ShortDescription { get; set; }
        public Dictionary<string, string> LongDescription { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public Dictionary<string, Dictionary<string, string>> LocalizedProperties { get; set; }
        public List<int> RelatedProducts { get; set; }
        public string PrimaryImageMediaId { get; set; }

        public void AddLocalizedProperty(string propertyName, string cultureCode, string value)
        {
            if (!LocalizedProperties.TryGetValue(propertyName, out var propertyValues))
            {
                propertyValues = new Dictionary<string, string>();
                LocalizedProperties.Add(propertyName, propertyValues);
            }

            propertyValues.Add(cultureCode, value);
        }

        public string GetDisplayName(string cultureCode)
        {
            return DisplayName.GetValueOrFallback(cultureCode, string.Empty);
        }

        public string GetShortDescription(string cultureCode)
        {
            return ShortDescription.GetValueOrFallback(cultureCode, string.Empty);
        }

        public string GetLongDescription(string cultureCode)
        {
            return LongDescription.GetValueOrFallback(cultureCode, string.Empty);
        }

        public Dictionary<string, string> GetProperties()
        {
            return Properties;
        }

        public Dictionary<string, string> GetLocalizedProperties(string cultureCode)
        {
            var result = new Dictionary<string, string>();

            foreach (var localizedProperty in LocalizedProperties)
            {
                result.Add(localizedProperty.Key, localizedProperty.Value[cultureCode]);
            }

            return result;
        }

        public List<int> GetRelatedProducts()
        {
            return RelatedProducts;
        }
    }
}
