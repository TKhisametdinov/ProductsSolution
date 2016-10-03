using System;
using Windows.UI.Xaml.Controls;
using Products.ClientApp.Common;
using Windows.UI.Xaml.Navigation;
using Products.ViewModel;

namespace Products.ClientApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class Products
    {
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper => navigationHelper;


        public Products()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;
            navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //var products = await _productService.Get();
            //_productItems = new ObservableCollection<ProductDataItem>(products);
            //DefaultViewModel["ProductItems"] = _productItems;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            var action = e.Parameter as string;
            if (action == null) return;

            var vm = DataContext as ProductViewModel;

            if (vm == null) return;
            if (action == "reload")
            {
                vm.Reload();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
