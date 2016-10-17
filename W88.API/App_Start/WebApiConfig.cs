using System.Web.Http;
using WebApiThrottle;

namespace W88.API
{
    /// <summary>
    /// Summary description for WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();

            config.MapHttpAttributeRoutes();
            // Web Api Throttle https://github.com/stefanprodan/WebApiThrottle 
            config.MessageHandlers.Add(new ThrottlingHandler()
            {
                Policy = ThrottlePolicy.FromStore(new PolicyConfigurationProvider()),
                Repository = new CacheRepository()

            });

            config.Routes.MapHttpRoute(
             name: "DefaultApiWithAction",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional });

        }
    }
}