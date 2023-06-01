

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Commands;
using Prism.Regions;
using Prism.Events;

using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;


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


        private List<ProductType> _listType;
        public List<ProductType> ListType
        {
            get => _listType;
            set => SetProperty(ref _listType, value);
        }


        private List<Categories> _listCategory;
        public List<Categories> ListCategory
        {
            get => _listCategory;
            set => SetProperty(ref _listCategory, value);
        }


        private List<Shipper> _listShipper;
        public List<Shipper> ListShipper
        {
            get => _listShipper;
            set => SetProperty(ref _listShipper, value);
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


        private ProductType _selectType;
        public ProductType SelectType
        {
            get => _selectType;
            set => SetProperty(ref _selectType, value);
        }


        private Categories _selectCategory;
        public Categories SelectCategory
        {
            get => _selectCategory;
            set => SetProperty(ref _selectCategory, value);
        }


        private Shipper _selectShipper;
        public Shipper SelectShipper
        {
            get => _selectShipper;
            set => SetProperty(ref _selectShipper, value);
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "SelectType":
                    if (SelectType != null)
                    {
                        ProductModel.TypeId = SelectType.Id;
                        RaisePropertyChanged("ProductModel");
                    }
                    break;

                case "SelectCategory":
                    if (SelectCategory != null)
                    {
                        ProductModel.CategoryId = SelectCategory.Id;
                        RaisePropertyChanged("ProductModel");
                    }
                    break;

                case "SelectShipper":
                    if (SelectShipper != null)
                    {
                        ProductModel.ShipperId = SelectShipper.Id;
                        RaisePropertyChanged("ProductModel");
                    }
                    break;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _isPressed = false;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsDeleteVisible = "Collapsed";
            ProductModel = new Product();
            ListType = await _dataService.GetDataAsync<ProductType>();
            ListCategory = await _dataService.GetDataAsync<Categories>();
            ListShipper = await _dataService.GetDataAsync<Shipper>();

            var item = navigationContext.Parameters["item"];
            if (item != null)
            {
                IsDeleteVisible = "Visible";
                if (item is Product prod)
                {
                    ProductModel = prod;
                    SelectType = ListType.Find(x => x.Id == prod.TypeId);
                    SelectCategory = ListCategory.Find(x => x.Id == prod.CategoryId);
                    SelectShipper = ListShipper.Find(x => x.Id == prod.ShipperId);
                }
            }
        }

        #endregion
    }
}
