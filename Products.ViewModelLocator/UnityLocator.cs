using Microsoft.Practices.Unity;
using Products.Shared;

namespace Products.Locator
{
    public class UnityLocator : DependencyInjectionServiceLocator<IUnityContainer>
    {
        public UnityLocator(IUnityContainer container)
            : base(container)
        {
        }

        protected override T Get<T>(IUnityContainer container)
        {
            return container.Resolve<T>();
        }

    }
}
