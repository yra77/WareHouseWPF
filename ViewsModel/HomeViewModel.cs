

using WareHouseWPF.Models;
using WareHouseWPF.Controls.Listviews;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Regions;
using Prism.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace WareHouseWPF.ViewsModel
{
    internal class HomeViewModel : BaseViewModel, INavigationAware
    {


        public HomeViewModel(IRegionManager regionManager,
                             IVerifyService verifyService,
                             IDataService dataService,
                             ITranslationSource translation)
            : base()
        {
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _translation = translation;

            IsLoaded = "Hidden";//Visible
            Logout = _employeeAuth.Role + " Logout";
            DateToday = DateTime.Now.ToShortDateString();
            EmptyListTextVisible = "Collapsed";
        }


        #region public property

        private List<object> _list;
        public List<object> List
        {
            get => _list;
            set => SetProperty(ref _list, value);
        }


        private string _emptyListTextVisible;
        public string EmptyListTextVisible
        { 
            get => _emptyListTextVisible; 
            set => SetProperty(ref _emptyListTextVisible, value);
        }


        private string _dateToday;
        public string DateToday
        {
            get => _dateToday;
            set => SetProperty(ref _dateToday, value);
        }


        private string _logout;
        public string Logout
        {
            get => _logout;
            set => SetProperty(ref _logout, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        public DelegateCommand ClientsBtn => new DelegateCommand(ClientsPrintAsync);
        public DelegateCommand EmployeesBtn => new DelegateCommand(EmployeesPrint);
        public DelegateCommand ShippersBtn => new DelegateCommand(ShippersPrint);
        public DelegateCommand ProductsBtn => new DelegateCommand(ProductsPrint);
        public DelegateCommand AddProductBtn => new DelegateCommand(AddProduct);
        public DelegateCommand<object> AddBtnInList => new DelegateCommand<object>(AddEntity);
        public DelegateCommand SettingsBtn => new DelegateCommand(SettingsClick);

        #endregion


        private void AddEntity(object obj)
        {
            var parameters = new NavigationParameters
            {
                { "entity", obj }
            };

            _regionManager.RequestNavigate("MainRegion", "AddingHumans", parameters);
        }

        private void SettingsClick()
        {
            _regionManager.RequestNavigate("MainRegion", "Settings");
        }

        private async void ClientsPrintAsync()
        {
            _regionManager.RequestNavigate("ListViewRegion", "ClientList");
            
            IsLoaded = "Visible";

            List = new List<object>(await _dataService.GetDataAsync<ClientModel>());
            
            if(List == null || List.Count == 0)
            {
                EmptyListTextVisible = "Visible";
            }
            
            RaisePropertyChanged("List");
            IsLoaded = "Hidden";//Visible
        }

        private void EmployeesPrint()
        {
            _regionManager.RequestNavigate("ListViewRegion", "EmployeeList");
        }

        private void ShippersPrint()
        {
            _regionManager.RequestNavigate("ListViewRegion", "ShipperList");
        }

        private void ProductsPrint()
        {
            _regionManager.RequestNavigate("ListViewRegion", "ProductList");
        }

        private void AddProduct()
        {

        }

        #region Interfaces

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(EmployeeList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ClientList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ShipperList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ProductList));

            //_region = _regionManager.Regions["ListViewRegion"];
            //foreach(var a in _region.Views)
            //Debug.WriteLine("AAAAAAAAAAAAAAa " + a);
        }

        #endregion
    }
}
