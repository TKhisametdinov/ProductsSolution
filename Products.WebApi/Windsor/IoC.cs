using System.Reflection;
using System.Web.Http.ExceptionHandling;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NLog;
using Products.WebApi.BL;
using Products.WebApi.BL.Interfaces;
using Products.WebApi.DAL;
using Products.WebApi.DAL.Interfaces;
using Products.WebApi.DAL.Models;
using Products.WebApi.DAL.Repositories;
using Products.WebApi.Logging;

namespace Products.WebApi.Windsor
{
    public class IoC
    {
        public static IWindsorContainer BootstrapContainer()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IRepository<Product>>().ImplementedBy<Repository<Product>>().LifestylePerWebRequest());
            container.Register(Component.For<IProductService>().ImplementedBy<ProductService>().LifestylePerWebRequest());
            container.Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestylePerWebRequest());
            container.Register(Component.For<IProductRepository>().ImplementedBy<ProductRepository>().LifestylePerWebRequest());
            container.Register(Component.For<ProductContext>().ImplementedBy<ProductContext>().LifestylePerWebRequest());

            container.Register(Component.For<IExceptionLogger>().ImplementedBy<NLogExceptionLogger>());
            container.Register(Component.For<MessageHandler>().ImplementedBy<MessageLoggingHandler>());

            container.Register(Component.For<ILogger>()
                .Instance(LogManager.GetLogger(Assembly.GetExecutingAssembly().GetName().Name)));

            container.Install(FromAssembly.This());

            return container;
        }
    }
}