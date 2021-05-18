using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcCategoryProperty
    {
        public int CategoryPropertyId { get; set; }
        public int CategoryId { get; set; }
        public int DefinitionFieldId { get; set; }
        public string Value { get; set; }
        public string CultureCode { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
