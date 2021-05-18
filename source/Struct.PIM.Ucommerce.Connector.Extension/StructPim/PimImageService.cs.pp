using System;
using System.Configuration;
using System.Web;
using Ucommerce.Umbraco8;
using Ucommerce.Umbraco8.Content;
using Content = Ucommerce.Content.Content;

namespace $rootnamespace$.StructPim
{
    public class PimImageService : ImageService
    {
        public override Content GetImage(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return ImageNotFound(id);
            }

            try
            {
                var imageUrl = ConfigurationManager.AppSettings["PimConnector.ImageUrl"] + id;
                var content = new Content
                {
                    Id = id,
                    Name = id,
                    Url = imageUrl,
                    NodeType = Constants.ImagePicker.Image
                };

                return content;
            }
            catch (Exception)
            {
                return ImageNotFound(id);
            }
        }

        private Content ImageNotFound(string id)
        {
            return new Content
            {
                Id = id,
                Name = "image-not-found.png",
                Url = VirtualPathUtility.ToAbsolute("~/umbraco/Ucommerce/images/ui/image-not-found.png"),
                NodeType = Constants.ImagePicker.Image
            };
        }
    }
}