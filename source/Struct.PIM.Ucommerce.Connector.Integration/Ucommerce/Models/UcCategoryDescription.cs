using System;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce.Models
{
    public class UcCategoryDescription
    {
        public int CategoryDescriptionId { get; set; }
        public int CategoryId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CultureCode { get; set; }
        public bool RenderAsContent { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
