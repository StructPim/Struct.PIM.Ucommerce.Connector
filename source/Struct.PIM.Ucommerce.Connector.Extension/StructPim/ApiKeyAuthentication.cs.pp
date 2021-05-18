using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace $rootnamespace$.StructPim
{
    public class ApiKeyAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsValid(actionContext))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            base.OnAuthorization(actionContext);
        }

        private bool IsValid(HttpActionContext actionContext)
        {
            var apiKey = ConfigurationManager.AppSettings["PimConnector.ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return false;
            }

            return actionContext.Request.Headers.TryGetValues("APIKEY", out var extractedApiKey) && extractedApiKey.First() == apiKey;
        }
    }
}