using System.Configuration;
using System.Reflection;
using System.Web.Http.ExceptionHandling;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NLog;
using Products.WebApi.DAL;
using Products.WebApi.Interfaces;
using Products.WebApi.Logging;
using Products.WebApi.Models;

namespace Products.WebApi.Windsor
{
    public class IoC
    {
        public static IWindsorContainer BootstrapContainer()
        {
            var container = new WindsorContainer();

            var conn = ConfigurationManager.ConnectionStrings["ProductContext"].ConnectionString;

            container.Register(Component.For<ProductContext>().ImplementedBy<ProductContext>().
                DependsOn(Dependency.OnValue("connectionStringName", conn))
                .LifestylePerWebRequest()
            );

            container.Register(Component.For<IRepository<Product>>().ImplementedBy<Repository<Product>>()
                .LifestylePerWebRequest());

            container.Register(Component.For<IExceptionLogger>().ImplementedBy<NLogExceptionLogger>());
            container.Register(Component.For<MessageHandler>().ImplementedBy<MessageLoggingHandler>());

            container.Register(Component.For<ILogger>()
                .Instance(LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name)));

            container.Install(FromAssembly.This());

            return container;
        }
    }
}