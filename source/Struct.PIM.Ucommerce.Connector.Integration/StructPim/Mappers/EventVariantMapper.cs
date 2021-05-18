using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Variant;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class EventVariantMapper : IVariantMapper
    {
        public UcVariantModel ToVariant(VariantModel variantModel, VariantAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<EventVariantModel>();

            var ucVariant = new UcVariantModel(variantModel);
            ucVariant.VariantSku = values.SKU;
            ucVariant.Name = values.EventLocation + ", " + values.EventDate;

            ucVariant.Properties.Add(nameof(values.EventDate), values.EventDate?.ToString());
            ucVariant.Properties.Add(nameof(values.EventLocation), values.EventLocation);
            
            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucVariant.DisplayName.Add(cultureCode, values.EventLocation + ", " + values.EventDate);
                ucVariant.AddLocalizedProperty(nameof(values.EventLocationDescription), cultureCode, values.EventLocationDescription.Get(cultureCode));
            }
            
            return ucVariant;
        }
    }
}