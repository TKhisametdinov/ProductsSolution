using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Products.BusinessLayer;
using Products.ClientApp.Common;
using Products.Locator;
using Products.Model;
using Products.Shared;
using Products.Shared.Interfaces;
using Products.ViewModel;

namespace Products.ClientApp.Locator
{
    internal class UnityRegistrationModule : IContainerRegistrationModule<IUnityContainer>
    {
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IServiceLocator, UnityLocator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IHttpClientHelper, HttpClientHelper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IService<Product>, ProductService>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, NavigationService>(
                new InjectionConstructor(
                    new Dictionary<string, Type>
                    {
                        {"Details", typeof(ProductDetail)},
                        {"Products", typeof(Products)}
                    }
                ));
            container.RegisterType<ProductViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ProductDetailViewModel>(new ContainerControlledLifetimeManager());

            // TODO: move http://localhost/Products.WebApi to config
            container.RegisterType<IProductApiUrlProvider, ProductsApiUrlProvider>(
                new InjectionConstructor("http://localhost:48774"));
        }
    }
}
