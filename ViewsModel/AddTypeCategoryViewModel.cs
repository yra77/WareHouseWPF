

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
    internal class AddTypeCategoryViewModel : BaseViewModel, INavigationAware
    {


        private string _modelName;
        private string _goback;


        public AddTypeCategoryViewModel(IEventAggregator eventAggregator,
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


        #region property

        private BaseModel _otherModel;
        public BaseModel OtherModel
        {
            get => _otherModel;
            set => SetProperty(ref _otherModel, value);
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


                if (_modelName == "productType")
                {
                    result = await _dataService.DeleteAsync<ProductType>(OtherModel.Id);
                }
                else if (_modelName == "categories")
                {
                    result = await _dataService.DeleteAsync<Categories>(OtherModel.Id);
                }

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

            if (!_isPressed && OtherModel != null && IsValidInput)
            {
                _isPressed = true;
                bool result = false;
                IsLoaded = "Visible";

                if (_modelName == "productType")
                {
                    result = await _dataService.InsertUpdateAsync<ProductType>(OtherModel.Id, OtherModel as ProductType);
                }
                else if (_modelName == "categories")
                {
                    result = await _dataService.InsertUpdateAsync<Categories>(OtherModel.Id, OtherModel as Categories);
                }

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
            if (_goback != null)
            {
                _regionManager.RequestNavigate("MainRegion", _goback);
            }
            else
            {
                _regionManager.RequestNavigate("MainRegion", "Home");
            }
        }

        private void Convert_Item(object item)
        {

            IsDeleteVisible = "Visible";

            if (_modelName == "productType" && item is ProductType productType)
            {
                OtherModel = productType;
            }
            else if (_modelName == "categories" && item is Categories category)
            {
                OtherModel = category;
            }
        }


        #region implement intarface

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

            if (navigationContext.Parameters["entity"] != null)
            {
                _modelName = navigationContext.Parameters["entity"].ToString();

                if (_modelName != null)
                {
                    if (_modelName == "productType")
                    {
                        OtherModel = new ProductType();
                    }
                    else if (_modelName == "categories")
                    {
                        OtherModel = new Categories();
                    }

                    var goback = navigationContext.Parameters["goback"];
                    if (goback != null)
                    {
                        _goback = goback.ToString();
                    }

                    var item = navigationContext.Parameters["item"];
                    if (item != null)
                    {
                        Convert_Item(item);
                    }
                }
                else
                {

                }
            }
        }

        #endregion
    }
}
