using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Products.ClientApp.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Products.Model;
using Products.ViewModel;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Products.ClientApp
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ProductDetail
    {
        private readonly NavigationHelper _navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper => _navigationHelper;

        public ProductDetail()
        {
            InitializeComponent();
            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //object navigationParameter;
            //if (e.PageState != null && e.PageState.ContainsKey("SelectedItem"))
            //{
            //    navigationParameter = e.PageState["SelectedItem"];
            //}
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
            _navigationHelper.OnNavigatedTo(e);
            var productDataItem = e.Parameter as Product;
            if (productDataItem != null)
            {
                var vm = DataContext as ProductDetailViewModel;
                if (vm != null)
                {
                    vm.Product = productDataItem;
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

       
        #endregion
    }
}
