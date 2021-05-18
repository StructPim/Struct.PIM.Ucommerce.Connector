using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcProduct
    {
        public int ProductId { get; set; }
        public int? ParentProductId { get; set; }
        public string Sku { get; set; }
        public string VariantSku { get; set; }
        public string Name { get; set; }
        public bool DisplayOnSite { get; set; }
        public string ThumbnailImageMediaId { get; set; }
        public string PrimaryImageMediaId { get; set; }
        public decimal Weight { get; set; }
        public int ProductDefinitionId { get; set; }
        public bool AllowOrdering { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public float Rating { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
