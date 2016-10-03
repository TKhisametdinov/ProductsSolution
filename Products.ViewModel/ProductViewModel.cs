using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Products.Model;
using Products.Shared.Interfaces;
using Products.ViewModel.Annotations;
using Products.ViewModel.Interfaces;

namespace Products.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private static IService<Product> _productService;
        private readonly INavigationService _navigationService;
        public ICommand GotoDetailsPageCommand { get; set; }

        public INotifyTaskCompletion<IEnumerable<Product>> ProductsAsync { get; private set; }

        public ProductViewModel(IService<Product> productService, INavigationService navigationService)
        {
            _productService = productService;
            _navigationService = navigationService;

            GotoDetailsPageCommand = new RelayCommand(x => _navigationService.Navigate("Details", x));
            ProductsAsync = NotifyTaskCompletion.Create(_productService.Get());
        }

        public void Reload()
        {
            ProductsAsync = NotifyTaskCompletion.Create(_productService.Get());
            OnPropertyChanged(nameof(ProductsAsync));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
