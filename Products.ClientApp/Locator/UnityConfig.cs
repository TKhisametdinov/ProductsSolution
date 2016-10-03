using System;
using Microsoft.Practices.Unity;

namespace Products.ClientApp.Locator
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(
            () =>
                {
                    var container = new UnityContainer();
                    RegisterTypes(container);
                    return container;
                });

        public static IUnityContainer GetUnityContainer()
        {
            return Container.Value;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            var module = new UnityRegistrationModule();
            module.Register(container);
        }
    }
}
