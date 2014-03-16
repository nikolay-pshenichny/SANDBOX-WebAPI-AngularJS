using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.Windsor;

using DemoProject.API.App_Start;
using DemoProject.Common.Windsor;

namespace DemoProject.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        // Windsor container for the application.
        private readonly IWindsorContainer container;

        public WebApiApplication()
        {
            // Windsor container setup
            this.container = new WindsorContainer();
            this.container.AddFacility(new InjectFacility());
            this.container.Install(new Windsor.Installer());
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Replacing default IHttpControllerActivator with its custom implementation, that will allow to use Castle Windsor in Controllers
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(this.container));
        }
    }
}
