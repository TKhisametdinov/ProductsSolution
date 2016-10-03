using Microsoft.Practices.Unity;
using Products.Shared.Interfaces;
using Products.ViewModel;

namespace Products.ClientApp.Locator
{
    public class ViewModelLocator
    {
        private static IUnityContainer _unityContainer;
        private static IServiceLocator _serviceLocator;

        private static IUnityContainer UnityContainer => _unityContainer ?? (_unityContainer = UnityConfig.GetUnityContainer());
        private static IServiceLocator ServiceLocator => _serviceLocator ?? (_serviceLocator = UnityContainer.Resolve<IServiceLocator>());

        public ProductViewModel ProductViewModel => ServiceLocator.Get<ProductViewModel>();
        public ProductDetailViewModel ProductDetailViewModel => ServiceLocator.Get<ProductDetailViewModel>();
    }
}
