using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucommerce.Content;

namespace Struct.PIM.Ucommerce.Connector.Content
{
    public class IntegrationTestContentService : IContentService
    {
        public global::Ucommerce.Content.Content GetContent(string contentId)
        {
            return new global::Ucommerce.Content.Content()
            {
                Icon = "",
                Id = "",
                Name = "",
                Url = ""
            };
        }
    }
}
