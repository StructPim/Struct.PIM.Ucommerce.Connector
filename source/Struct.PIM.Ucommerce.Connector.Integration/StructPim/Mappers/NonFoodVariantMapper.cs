using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Variant;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers
{
    public class NonFoodVariantMapper : IVariantMapper
    {
        public UcVariantModel ToVariant(VariantModel variantModel, VariantAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias)
        {
            var values = attributeValues.Values.As<NonfoodVariantModel>();
            
            var ucVariant = new UcVariantModel(variantModel);
            ucVariant.VariantSku = values.SKU;
            ucVariant.Name = values.Name.Get(Settings.DefaultCultureCode);

            foreach (var cultureCode in Settings.CultureCodes)
            {
                ucVariant.DisplayName.Add(cultureCode, values.Name.Get(cultureCode));

                ucVariant.AddLocalizedProperty(nameof(values.Graphics), cultureCode, attributesByAlias[nameof(values.Graphics)].RenderFirstValue(attributeValues, cultureCode));
                ucVariant.AddLocalizedProperty(nameof(values.Color), cultureCode, attributesByAlias[nameof(values.Color)].RenderFirstValue(attributeValues, cultureCode));
            }

            ucVariant.Properties.Add(nameof(values.CPU), attributesByAlias[nameof(values.CPU)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.HDD), attributesByAlias[nameof(values.HDD)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.InternalMemory), attributesByAlias[nameof(values.InternalMemory)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.NumInPackage), values.NumInPackage?.ToString());
            ucVariant.Properties.Add(nameof(values.Width), attributesByAlias[nameof(values.Width)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.Length), attributesByAlias[nameof(values.Length)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.ClotheSize), attributesByAlias[nameof(values.ClotheSize)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.ShoeSize), attributesByAlias[nameof(values.ShoeSize)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));
            ucVariant.Properties.Add(nameof(values.PantSize), attributesByAlias[nameof(values.PantSize)].RenderFirstValue(attributeValues, Settings.DefaultCultureCode));

            ucVariant.PrimaryImageMediaId = values.PrimaryImage;
            
            return ucVariant;
        }
    }
}
