using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcProductDescription
    {
        public int ProductDescriptionId { get; set; }
        public int ProductId { get; set; }
        public string DisplayName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string CultureCode { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}