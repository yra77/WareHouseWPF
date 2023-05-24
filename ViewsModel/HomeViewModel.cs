﻿

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
using System.Diagnostics;
using System.Windows.Markup;
using System.ComponentModel;


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
            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);
        }


        #region public property

        private XmlLanguage _datePickerLanguage;
        public XmlLanguage DatePickerLanguage
        {
            get => _datePickerLanguage;
            set => SetProperty(ref _datePickerLanguage, value);
        }


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


        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }


        public DelegateCommand ClientsBtn => new DelegateCommand(ClientsPrintAsync);
        public DelegateCommand EmployeesBtn => new DelegateCommand(EmployeesPrintAsync);
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

            if (List == null || List.Count == 0)
            {
                EmptyListTextVisible = "Visible";
            }

            RaisePropertyChanged("List");
            IsLoaded = "Hidden";//Visible
        }

        private async void EmployeesPrintAsync()
        {
            _regionManager.RequestNavigate("ListViewRegion", "EmployeeList");

            IsLoaded = "Visible";

            List = new List<object>(await _dataService.GetDataAsync<Employee>());

            if (List == null || List.Count == 0)
            {
                EmptyListTextVisible = "Visible";
            }

            RaisePropertyChanged("List");
            IsLoaded = "Hidden";//Visible
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            string model = "";

            switch (args.PropertyName)
            {
                case "SelectedItem":

                    if (SelectedItem != null)
                    {
                        switch (SelectedItem.ToString().Split('.').Last())
                        {
                            case "Employee":
                                if (SelectedItem is Employee employee)
                                {
                                    model = "employee";
                                }

                                break;

                            case "ClientModel":
                                if (SelectedItem is ClientModel client)
                                {
                                    model = "client";
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    break;
            }

            if (SelectedItem != null)
            {
                var parameters = new NavigationParameters
                                {
                                  { "entity", model },
                                  { "item", SelectedItem}
                                };

                _regionManager.RequestNavigate("MainRegion", "AddingHumans", parameters);
            }

            SelectedItem = null;
        }


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

            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);
            //_region = _regionManager.Regions["ListViewRegion"];
            //foreach(var a in _region.Views)
            //Debug.WriteLine("AAAAAAAAAAAAAAa " + a);
        }

        #endregion
    }
}
