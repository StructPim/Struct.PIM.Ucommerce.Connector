using System.Web.Http;
using Struct.PIM.Ucommerce.Connector.Integration;
using Struct.PIM.Ucommerce.Connector.Integration.StructPim;
using Struct.PIM.Ucommerce.Connector.Integration.Ucommerce;
using Struct.PIM.Ucommerce.ConnectorApi.Models;

namespace Struct.PIM.Ucommerce.ConnectorApi.Controllers
{
    public class WebhookController : ApiController
    {
        private readonly VariantIntegration _variantIntegration;
        private readonly ProductIntegration _productIntegration;

        public WebhookController()
        {
            _variantIntegration = new VariantIntegration(PimBroker.Instance, UcommerceBroker.Instance);
            _productIntegration = new ProductIntegration(PimBroker.Instance, UcommerceBroker.Instance);
        }

        [HttpPost]
        [Route("VariantUpdate")]
        public void VariantUpdate(VariantUpdateRequest updateRequest)
        {
            _variantIntegration.CreateOrUpdateVariants(updateRequest.VariantIds);
        }

        [HttpPost]
        [Route("ProductUpdate")]
        public void ProductUpdate(ProductUpdateRequest updateRequest)
        {
            _productIntegration.CreateOrUpdateProducts(updateRequest.ProductIds);
        }

        [HttpPost]
        [Route("VariantDelete")]
        public void VariantDelete(VariantUpdateRequest deleteRequest)
        {
            _variantIntegration.DeleteVariants(deleteRequest.VariantIds);
        }

        [HttpPost]
        [Route("ProductDelete")]
        public void ProductDelete(ProductUpdateRequest deleteRequest)
        {
            _productIntegration.DeleteProducts(deleteRequest.ProductIds);
        }
    }
}
