using System.Collections.Generic;
using Struct.PIM.Api.Models.Attribute;
using Struct.PIM.Api.Models.Variant;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim.Models;

namespace Struct.PIM.Ucommerce.Connector.Integration.StructPim.Mappers.Base
{
    public interface IVariantMapper
    {
        UcVariantModel ToVariant(VariantModel variantModel, VariantAttributeValuesModel attributeValues, Dictionary<string, Attribute> attributesByAlias);
    }
}