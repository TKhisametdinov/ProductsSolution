using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace Products.WebApi.Windsor
{
    internal sealed class WindsorDependencyScope : IDependencyScope
    {
        private readonly IKernel _kernel;

        private readonly IDisposable _disposable;

        public WindsorDependencyScope(IKernel kernel)
        {
            _kernel = kernel;
            _disposable = kernel.BeginScope();
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
            _disposable.Dispose();
        }
    }

}