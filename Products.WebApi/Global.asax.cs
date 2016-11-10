using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using Castle.Windsor;
using Products.WebApi.Logging;
using Products.WebApi.Windsor;

namespace Products.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            _container = IoC.BootstrapContainer();
            ProductContextInitializationHandler.Initialize();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);

            var nlogExceptionLogger = _container.Resolve<IExceptionLogger>();
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), nlogExceptionLogger);

            var messageHandler = _container.Resolve<MessageHandler>();
            GlobalConfiguration.Configuration.MessageHandlers.Add(messageHandler);
        }

        protected void Application_End()
        {
            _container?.Dispose();
        }
    }
}
