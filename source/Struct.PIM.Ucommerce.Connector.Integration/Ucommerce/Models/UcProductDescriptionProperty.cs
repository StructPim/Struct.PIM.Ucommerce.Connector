using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcProductDescriptionProperty
    {
        public int ProductDescriptionPropertyId { get; set; }
        public int ProductDescriptionId { get; set; }
        public int ProductDefinitionFieldId { get; set; }
        public string Value { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}