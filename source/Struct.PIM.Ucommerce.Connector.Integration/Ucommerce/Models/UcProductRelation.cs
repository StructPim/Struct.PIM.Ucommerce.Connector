using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcProductRelation
    {
        public int ProductRelationId { get; set; }
        public int ProductId { get; set; }
        public int RelatedProductId { get; set; }
        public int ProductRelationTypeId { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
