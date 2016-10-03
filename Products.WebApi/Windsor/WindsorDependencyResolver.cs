using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;

namespace Products.WebApi.Windsor
{
    internal class WindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public WindsorDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_kernel);
        }

        public object GetService(Type type)
        {
            return _kernel.HasComponent(type) ? _kernel.Resolve(type) : null;
        }

        public IEnumerable<object> GetServices(Type type)
        {
            return _kernel.ResolveAll(type).Cast<object>();
        }

        public void Dispose()
        {
        }
    }
}