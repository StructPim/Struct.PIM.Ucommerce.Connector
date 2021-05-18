using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcCategoryProductRelation
    {
        public int CategoryProductRelationId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SortOrder { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
