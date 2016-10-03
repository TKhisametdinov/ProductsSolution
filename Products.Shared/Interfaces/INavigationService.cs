using System;

namespace Products.Shared.Interfaces
{
    public  interface INavigationService
    {
        void AddPage(string pageName, Type pageType);

        void Navigate(string pageName, object parameter);

        void Navigate(string pageName);

        void Navigate(Type sourcePageType);

        void Navigate(Type sourcePageType, object parameter);

        void GoBack();
    }
}
