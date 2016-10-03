using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Products.Shared.Interfaces;

namespace Products.ClientApp.Common
{
    public class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _navigationPages;

        public NavigationService(Dictionary<string, Type> initialPages)
        {
            _navigationPages = initialPages;
        }

        public void AddPage(string pageName, Type pageType)
        {
            if (!_navigationPages.ContainsKey(pageName))
            {
                _navigationPages.Add(pageName, pageType);
            }
            else
            {
                _navigationPages[pageName] = pageType;
            }
        }

        public void Navigate(Type sourcePageType)
        {
            ((Frame)Window.Current.Content).Navigate(sourcePageType);
        }

        public void Navigate(Type sourcePageType, object parameter)
        {
            ((Frame)Window.Current.Content).Navigate(sourcePageType, parameter);
        }

        public void Navigate(string pageName)
        {
            if (_navigationPages.ContainsKey(pageName))
            {
                Navigate(_navigationPages[pageName]);
            }
        }

        public void Navigate(string pageName, object parameter)
        {
            if (_navigationPages.ContainsKey(pageName))
            {
                Navigate(_navigationPages[pageName], parameter);
            }
        }

        public void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
