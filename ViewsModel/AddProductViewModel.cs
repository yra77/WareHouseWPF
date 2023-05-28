

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Commands;
using Prism.Regions;
using Prism.Events;

using System.Windows;


namespace WareHouseWPF.ViewsModel
{
    internal class AddProductViewModel : BaseViewModel, INavigationAware
    {


        public AddProductViewModel(IEventAggregator eventAggregator, 
                             IRegionManager regionManager,
                             ITranslationSource translation,
                             IVerifyService verifyService,
                             IDataService dataService)
            : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _translation = translation;

            IsLoaded = "Hidden";//Visible
            IsDeleteVisible = "Collapsed";
            IsValidInput = false;
        }


        #region propery


        private Product _productModel;
        public Product ProductModel 
        { 
            get => _productModel;
            set => SetProperty(ref _productModel, value);
        }


        private bool _isValidInput;
        public bool IsValidInput
        {
            get => _isValidInput;
            set => SetProperty(ref _isValidInput, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        private string _isDeleteVisible;
        public string IsDeleteVisible
        {
            get => _isDeleteVisible;
            set => SetProperty(ref _isDeleteVisible, value);
        }


        public DelegateCommand BackBtn => new DelegateCommand(GoBack);
        public DelegateCommand AgreeBtn => new DelegateCommand(AgreeClick);
        public DelegateCommand DeleteBtn => new DelegateCommand(DeleteClickAsync);


        #endregion


        private async void DeleteClickAsync()
        {
            if (!_isPressed)
            {
                _isPressed = true;
                bool result = false;
                IsLoaded = "Visible";

                    result = await _dataService.DeleteAsync<Product>(ProductModel.Id);
        
                IsLoaded = "Hidden";

                if (result)
                {
                    _eventAggregator.GetEvent<MyEvent>().Publish();
                    GoBack();
                }
                else
                {
                    MessageBox.Show(TranslationSource.Instance["ErrorDeletData"], "Error", MessageBoxButton.OK);
                    _isPressed = false;
                }
            }
        }

        private async void AgreeClick()
        {

            if (!_isPressed && ProductModel != null && IsValidInput)
            {
                _isPressed = true;
                IsLoaded = "Visible";

                bool result = await _dataService.InsertUpdateAsync(ProductModel.Id, ProductModel);

                IsLoaded = "Hidden";

                if (result)
                {
                    _eventAggregator.GetEvent<MyEvent>().Publish();
                    GoBack();
                }
                else
                {
                    MessageBox.Show(TranslationSource.Instance["ErrorSaveData"], "Error", MessageBoxButton.OK);
                    _isPressed = false;
                }
            }
            else
            {
                MessageBox.Show(TranslationSource.Instance["ErrorSaveData"], "Error", MessageBoxButton.OK);
            }
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home");
        }


        #region implement interfaces

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _isPressed = false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsDeleteVisible = "Collapsed";
            ProductModel = new Product();

            var item = navigationContext.Parameters["item"];
            if (item != null)
            {
                IsDeleteVisible = "Visible";
                if (item is Product prod)
                {
                    ProductModel = prod;
                }
            }
        }

        #endregion
    }
}
