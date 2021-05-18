using Ucommerce.Content;

namespace Struct.PIM.Ucommerce.Connector.Content
{
    public class IntegrationTestImageService : IImageService
    {
        global::Ucommerce.Content.Content IImageService.GetImage(string id)
        {
            return new global::Ucommerce.Content.Content
            {
                Name = "Image",
                Url = "Url"
            };
        }
    }
}