using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;
using Ucommerce.Search.Indexers;

namespace $rootnamespace$.StructPim
{
    [ApiKeyAuthentication]
    [RoutePrefix("Ucommerceapi")]
    public class SearchController : ApiController
    {
        [Route("search/index")]
        [HttpPost]
        public IHttpActionResult BuildIndex()
        {
            ObjectFactory.Instance.Resolve<IScratchIndexer>().Index();

            return Ok();
        }

        [Route("search/products/index")]
        [HttpPost]
        public IHttpActionResult IndexProducts([FromBody] List<int> productIds)
        {
            var session = ObjectFactory.Instance.Resolve<IStatelessSessionProvider>().GetStatelessSession();
            var productUids = session.Query<Product>()
                .Where(x => productIds.Contains(x.ProductId))
                .Select(x => x.Guid)
                .ToList();

            Index<global::Ucommerce.Search.Models.Product>(productUids);

            return Ok();
        }

        [Route("search/categories/index")]
        [HttpPost]
        public IHttpActionResult IndexCategories([FromBody] List<int> categoryIds)
        {
            var session = ObjectFactory.Instance.Resolve<IStatelessSessionProvider>().GetStatelessSession();
            var categoryUids = session.Query<Category>()
                .Where(x => categoryIds.Contains(x.CategoryId))
                .Select(x => x.Guid)
                .ToList();

            Index<global::Ucommerce.Search.Models.Category>(categoryUids);

            return Ok();
        }

        private void Index<T>(List<Guid> uids) where T : global::Ucommerce.Search.Models.Model
        {
            ObjectFactory.Instance.Resolve<IBatchIndexer>().UpsertInBatches(typeof(T), uids);
        }
    }
}
