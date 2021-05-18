using System.Web.Http;
using Ucommerce.EntitiesV2;
using Ucommerce.Infrastructure;

namespace $rootnamespace$.StructPim
{
    [ApiKeyAuthentication]
    [RoutePrefix("Ucommerceapi")]
    public class CacheController : ApiController
    {
        /// <summary>
        /// This endpoint to clear the cache.
        /// </summary>
        [Route("cache/clear")]
        [HttpPost]
        public IHttpActionResult ClearCache()
        {
            ObjectFactory.Instance.Resolve<ICacheProvider>().ClearCache();
            return Ok();
        }
    }
}
