using System;
using System.Linq;
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
        private readonly CategoryIntegration _categoryIntegration;

        public WebhookController()
        {
            _variantIntegration = new VariantIntegration(PimBroker.Instance, UcommerceBroker.Instance);
            _productIntegration = new ProductIntegration(PimBroker.Instance, UcommerceBroker.Instance);
            _categoryIntegration = new CategoryIntegration(PimBroker.Instance, UcommerceBroker.Instance);
        }

        [HttpPost]
        [Route("")]
        public void Update(object model)
        {
            var eventKey = Request.Headers.GetValues("X-Event-Key").First();
            var username = Request.Headers.GetValues("X-User").First();
            var webhookUid = Request.Headers.GetValues("X-Hook-UID").First();
            var requestUid = Request.Headers.GetValues("X-Request-UID").First();

            switch (eventKey)
            {
                case "products:updated":
                    break;
                case "products:created":
                    break;
                case "products:deleted":
                    break;
            }

            Console.WriteLine(model);
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
        [Route("CategoryUpdate")]
        public void CategoryUpdate(CategoryUpdateRequest updateRequest)
        {
            _categoryIntegration.CreateOrUpdateCategories(updateRequest.CategoryIds);
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

        [HttpPost]
        [Route("CategoryDelete")]
        public void CategoryDelete(CategoryUpdateRequest deleteRequest)
        {
            _categoryIntegration.DeleteCategories(deleteRequest.CategoryIds);
        }
    }
}
