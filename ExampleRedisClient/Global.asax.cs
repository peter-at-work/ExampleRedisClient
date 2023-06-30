using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ExampleRedisClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var services = new ServiceCollection();
            services
                .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnection"]));

            // Anti-pattern; convert static classes to real dependency-injection.
            SingletonServices.ServiceProvider = services.BuildServiceProvider();
        }
    }
}
