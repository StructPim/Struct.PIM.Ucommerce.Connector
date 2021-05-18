using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageMediaId { get; set; }
        public bool DisplayOnSite { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ParentCategoryId { get; set; }
        public int ProductCatalogId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public int SortOrder { get; set; }
        public string CreatedBy { get; set; }
        public int DefinitionId { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
