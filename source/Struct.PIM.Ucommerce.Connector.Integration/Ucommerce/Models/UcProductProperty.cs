using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcProductProperty
    {
        public int ProductPropertyId { get; set; }
        public string Value { get; set; }
        public int ProductDefinitionFieldId { get; set; }
        public int ProductId { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
