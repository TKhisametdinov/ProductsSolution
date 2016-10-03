using System.ComponentModel;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Products.Model;
using Products.Shared.Interfaces;
using Products.ViewModel.Annotations;
using Products.ViewModel.Interfaces;
using Windows.UI.Xaml.Media.Imaging;

namespace Products.ViewModel
{
    public class ProductDetailViewModel : INotifyPropertyChanged
    {
        private static IService<Product> _productService;
        private readonly INavigationService _navigationService;
        private bool _viewMode;
        private bool _editMode;
        private bool _addMode;
        public ICommand SetEditModeCommand { get; set; }
        public ICommand SetViewModeCommand { get; set; }
        public ICommand SetAddModeCommand { get; set; }

        public ICommand DeleteProductCommand { get; set; }
        public ICommand SaveProductCommand { get; set; }
        public ICommand AddImageCommand { get; set; }

        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                LoadImageTaskCompletion = NotifyTaskCompletion.Create(LoadImageAsync());
            }
        }

        public bool ViewMode
        {
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                _editMode = !value;
                OnViewModeChanged();
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                _viewMode = !value;
                OnViewModeChanged();
            }
        }

        public bool AddMode
        {
            get { return _addMode; }
            set
            {
                _addMode = value;
                OnViewModeChanged();
            }
        }

        public INotifyTaskCompletion<bool> DeleteProductOperationAsync { get; private set; }
        public INotifyTaskCompletion<bool> SaveProductOperationAsync { get; private set; }
        public INotifyTaskCompletion<bool> LoadImageTaskCompletion { get; private set; }

        public ProductDetailViewModel(IService<Product> productService, INavigationService navigationService)
        {
            ViewMode = true;
            _productService = productService;
            _navigationService = navigationService;

            SetEditModeCommand = new RelayCommand(x => { EditMode = true; });
            SetViewModeCommand = new RelayCommand(x => { ViewMode = true; });

            DeleteProductCommand = new RelayCommand(x =>
            {
                DeleteProductOperationAsync = NotifyTaskCompletion.Create(DeleteProduct());
            });

            SaveProductCommand = new RelayCommand(x =>
            {
                SaveProductOperationAsync = NotifyTaskCompletion.Create(SaveProduct());
            });


            SetAddModeCommand = new RelayCommand(x =>
            {
                EditMode = true;
                Product = new Product();
                OnPropertyChanged(nameof(Product));
            });

            AddImageCommand = new RelayCommand(async x =>
            {
                await AddImageAsync();
            });
        }

        private async Task AddImageAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;

            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile == null)
            {
                // The user cancelled the picking operation
                return;
            }
            WriteableBitmap bitmapImage = new WriteableBitmap(200, 100);
            using (IRandomAccessStream stream = await inputFile.OpenAsync(FileAccessMode.Read))
            {
                // Create the decoder from the stream
                await bitmapImage.SetSourceAsync(stream);
                ImageSource = bitmapImage;
                OnPropertyChanged(nameof(ImageSource));

                Product.PhotoData = await ImageToBytes(bitmapImage);
                Product.PhotoWidth = bitmapImage.PixelWidth;
                Product.PhotoHeight = bitmapImage.PixelHeight;

                OnPropertyChanged(nameof(Product));
            }
        }


        public static async Task<byte[]> ImageToBytes(WriteableBitmap img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var stream = img.PixelBuffer.AsStream())
                {
                    await stream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private WriteableBitmap _imageSource;
        private Product _product;

        public WriteableBitmap ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private async Task<bool> LoadImageAsync()
        {
            if (Product.PhotoData == null)
            {
                return false;
            }

            ImageSource = await PrepareWritableBitmap();

            return true;
        }

        public async Task<WriteableBitmap> PrepareWritableBitmap()
        {
            var width = Product.PhotoWidth;
            var height = Product.PhotoHeight;
            WriteableBitmap bmp = new WriteableBitmap(width, height);
            using (Stream pixStream = bmp.PixelBuffer.AsStream())
            {
                await pixStream.WriteAsync(Product.PhotoData, 0, width*height*4);
                return bmp;
            }
        }

        private async Task<bool> DeleteProduct()
        {
            var response = await _productService.Remove(Product.ID);
            ViewMode = true;
            _navigationService.Navigate("Products", "reload");
            return response;
        }

        private async Task<bool> SaveProduct()
        {
            bool response;
            if (Product.ID == default(int))
                response = await _productService.Add(Product);
            else
                response = await _productService.Update(Product);

            ViewMode = true;
            _navigationService.Navigate("Products", "reload");
            return response;
        }

        private void OnViewModeChanged()
        {
            OnPropertyChanged(nameof(AddMode));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(ViewMode));
        }
    }
}
