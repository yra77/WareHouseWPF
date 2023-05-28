

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Commands;
using Prism.Regions;
using Prism.Events;

using System.Collections.Generic;
using System.Windows;


namespace WareHouseWPF.ViewsModel
{
    internal class AddShipperViewModel : BaseViewModel, INavigationAware
    {

       
        public AddShipperViewModel(IRegionManager regionManager,
                             ITranslationSource translation,
                             IVerifyService verifyService,
                             IDataService dataService,
                             IEventAggregator eventAggregator)
            : base()
        {
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _translation = translation;
            _eventAggregator = eventAggregator;

            IsLoaded = "Hidden";//Visible
            IsDeleteVisible = "Collapsed";
            IsValidInput = false;
        }


        #region property

        private Shipper _shipperMod;
        public Shipper ShipperMod
        {
            get => _shipperMod;
            set => SetProperty(ref _shipperMod, value);
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


        private BankDetails _bankDetails;
        public BankDetails Bank_Details
        {
            get => _bankDetails;
            set => SetProperty(ref _bankDetails, value);
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

                result = await _dataService.DeleteAsync<Product>(ShipperMod.Id);

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

            if (!_isPressed && ShipperMod != null && IsValidInput)
            {
                _isPressed = true;
                IsLoaded = "Visible";

                ShipperMod.BankAccount = new List<BankDetails>() { Bank_Details };
                bool result = await _dataService.InsertUpdateAsync(ShipperMod.Id, ShipperMod);

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

            Bank_Details = new BankDetails();

            ShipperMod = new Shipper();
            ShipperMod.BankAccount = new List<BankDetails>();

            var item = navigationContext.Parameters["item"];
            if (item != null && item is Shipper ship)
            {
                ShipperMod = ship;

                if (ShipperMod.BankAccount != null && ShipperMod.BankAccount.Count > 0)
                {
                    Bank_Details = ShipperMod.BankAccount[0];
                }
            }
        }

        #endregion
    }
}
