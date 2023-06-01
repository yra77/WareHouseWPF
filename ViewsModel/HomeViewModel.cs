﻿

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.Sort;
using WareHouseWPF.Controls.Listviews;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.AccessRoles;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Events;
using Prism.Regions;
using Prism.Commands;

using System;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Markup;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WareHouseWPF.ViewsModel
{
    internal class HomeViewModel : BaseViewModel, INavigationAware
    {


        private bool _isComeIn;
        private string _tableName;


        public HomeViewModel(IRegionManager regionManager,
                             IVerifyService verifyService,
                             IDataService dataService,
                             IAccessRole accessRole,
                             ITranslationSource translation,
                             IEventAggregator eventAggregator,
                             ISortService sortService)
            : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _sortService = sortService;
            _translation = translation;
            _accessRole = accessRole;

            _isComeIn = false;
            IsLoaded = "Hidden";//Visible
            Logout = _employeeAuth.Role + " Logout";
            DateToday = DateTime.Now.ToShortDateString();
            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);

            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ProductList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(EmployeeList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ClientList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ShipperList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(CategoryList));
            _regionManager.RegisterViewWithRegion("ListViewRegion", typeof(ProductTypeList));

            HighestPermission = false;
            MediumPermission = false;
            LowPermission = false;
            LowestPermission = false;
        }


        #region public property

        private XmlLanguage _datePickerLanguage;
        public XmlLanguage DatePickerLanguage
        {
            get => _datePickerLanguage;
            set => SetProperty(ref _datePickerLanguage, value);
        }


        private List<Employee> _listEmployee;
        public List<Employee> ListEmployee
        {
            get => _listEmployee;
            set => SetProperty(ref _listEmployee, value);
        }


        private List<ClientModel> _listClient;
        public List<ClientModel> ListClient
        {
            get => _listClient;
            set => SetProperty(ref _listClient, value);
        }


        private List<Product> _listProduct;
        public List<Product> ListProduct
        {
            get => _listProduct;
            set => SetProperty(ref _listProduct, value);
        }


        private List<Categories> _listCategory;
        public List<Categories> ListCategory
        {
            get => _listCategory;
            set => SetProperty(ref _listCategory, value);
        }


        private List<ProductType> _listType;
        public List<ProductType> ListType
        {
            get => _listType;
            set => SetProperty(ref _listType, value);
        }


        private List<Shipper> _listShipper;
        public List<Shipper> ListShipper
        {
            get => _listShipper;
            set => SetProperty(ref _listShipper, value);
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

        private object _selectedProduct;
        public object SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        private Categories _selectedCategory;
        public Categories SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        private ProductType _selectedType;
        public ProductType SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }


        private Shipper _selectedShipper;
        public Shipper SelectedShipper
        {
            get => _selectedShipper;
            set => SetProperty(ref _selectedShipper, value);
        }


        private bool _highestPermission;
        public bool HighestPermission
        {
            get => _highestPermission;
            set => SetProperty(ref _highestPermission, value);
        }


        private bool _mediumPermission;
        public bool MediumPermission
        {
            get => _mediumPermission;
            set => SetProperty(ref _mediumPermission, value);
        }


        private bool _lowPermission;
        public bool LowPermission
        {
            get => _lowPermission;
            set => SetProperty(ref _lowPermission, value);
        }


        private bool _lowestPermission;
        public bool LowestPermission
        {
            get => _lowestPermission;
            set => SetProperty(ref _lowestPermission, value);
        }


        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set => SetProperty(ref _currentTime, value);
        }


        public DelegateCommand ClientsBtn => new DelegateCommand(ClientsPrint);
        public DelegateCommand EmployeesBtn => new DelegateCommand(EmployeesPrint);
        public DelegateCommand ShippersBtn => new DelegateCommand(ShippersPrint);
        public DelegateCommand ProductsBtn => new DelegateCommand(ProductsPrint);
        public DelegateCommand ProductTypeBtn => new DelegateCommand(ProductTypePrint);
        public DelegateCommand CategoryBtn => new DelegateCommand(CategoryPrint);
        public DelegateCommand AddProductBtn => new DelegateCommand(AddProduct);
        public DelegateCommand AddCategoryBtn => new DelegateCommand(AddCategory);
        public DelegateCommand AddTypeBtn => new DelegateCommand(AddType);
        public DelegateCommand AddBtnShipper => new DelegateCommand(AddShipper);
        public DelegateCommand<object> AddBtnInList => new DelegateCommand<object>(AddEntity);
        public DelegateCommand SettingsBtn => new DelegateCommand(SettingsClick);
        public DelegateCommand LogOutBtn => new DelegateCommand(LogOut_Click);
        public DelegateCommand<object> AccountingBtn => new DelegateCommand<object>(AccountingPrint);
        public DelegateCommand<string> HeaderClick => new DelegateCommand<string>(Header_Click);

        #endregion


        private void Header_Click(string columnName)
        {
           
            switch (_tableName)
            {
                case "Employee":
                    ListEmployee = _sortService.SortEmployee(columnName, ListEmployee);
                    RaisePropertyChanged("ListEmployee");
                    break;

                case "Client":
                    ListClient = _sortService.SortClient(columnName, ListClient);
                    RaisePropertyChanged("ListClient");
                    break;

                case "Shipper":
                    ListShipper = _sortService.SortShipper(columnName, ListShipper);
                    RaisePropertyChanged("ListShipper");
                    break;

                case "Product":
                    ListProduct = _sortService.SortProduct(columnName, ListProduct);
                    RaisePropertyChanged("ListProduct");
                    break;

                case "Category":
                    ListCategory = _sortService.SortCategory(columnName, ListCategory);
                    RaisePropertyChanged("ListCategory");
                    break;

                case "ProductType":
                    ListType = _sortService.SortProductType(columnName, ListType);
                    RaisePropertyChanged("ListType");
                    break;
            }
        }

        public void LogOut_Click()
        {
            _employeeAuth = null;
            _accessRole.SetRole("");
            _regionManager.RequestNavigate("MainRegion", "Auth");
        }

        private void SettingsClick()
        {
            _regionManager.RequestNavigate("MainRegion", "Settings");
        }

        private void AccountingPrint(object obj)
        {
            if (MediumPermission)
            {
                var res = obj.ToString();

                var parameters = new NavigationParameters
                {
                      { "entity", res }
                };
                _regionManager.RequestNavigate("MainRegion", "Accounting", parameters);
            }
        }

        private async void ClientsPrint()
        {
            if (MediumPermission)
            {
                IsLoaded = "Visible";

                ListClient = new List<ClientModel>(await _dataService.GetDataAsync<ClientModel>());
                _regionManager.RequestNavigate("ListViewRegion", "ClientList");

                _tableName = "Client";
                IsLoaded = "Hidden";
            }
        }

        private async void EmployeesPrint()
        {
            if (HighestPermission)
            {
                _regionManager.RequestNavigate("ListViewRegion", "EmployeeList");

                IsLoaded = "Visible";

                ListEmployee = new List<Employee>(await _dataService.GetDataAsync<Employee>());

                _tableName = "Employee";
                IsLoaded = "Hidden";
            }
        }

        private async void ShippersPrint()
        {
            if (MediumPermission)
            {
                _regionManager.RequestNavigate("ListViewRegion", "ShipperList");

                IsLoaded = "Visible";

                ListShipper = new List<Shipper>(await _dataService.GetDataAsync<Shipper>());

                _tableName = "Shipper";
                IsLoaded = "Hidden";
            }
        }

        private async void ProductsPrint()
        {
            if (LowPermission)
            {
                if (_isComeIn)
                {
                    _tableName = "Product";
                    _regionManager.RequestNavigate("ListViewRegion", "ProductList");
                }
                IsLoaded = "Visible";

                ListProduct = new List<Product>(await _dataService.GetDataAsync<Product>());

                RaisePropertyChanged("ListProduct");

                IsLoaded = "Hidden";
            }
        }

        private async void CategoryPrint()
        {
            if (MediumPermission)
            {
                _regionManager.RequestNavigate("ListViewRegion", "CategoryList");

                IsLoaded = "Visible";

                ListCategory = new List<Categories>(await _dataService.GetDataAsync<Categories>());

                _tableName = "Category";
                IsLoaded = "Hidden";
            }
        }

        private async void ProductTypePrint()
        {
            if (MediumPermission)
            {
                _regionManager.RequestNavigate("ListViewRegion", "ProductTypeList");

                IsLoaded = "Visible";

                ListType = new List<ProductType>(await _dataService.GetDataAsync<ProductType>());

                RaisePropertyChanged("ListType");
                _tableName = "ProductType";
                IsLoaded = "Hidden";
            }
        }

        private void AddEntity(object obj)
        {
            if (HighestPermission)
            {
                if (obj.ToString() == "client")
                {
                    _eventAggregator.GetEvent<MyEvent>().Subscribe(ClientsPrint);
                }
                else
                {
                    _eventAggregator.GetEvent<MyEvent>().Subscribe(EmployeesPrint);
                }

                var parameters = new NavigationParameters
                {
                      { "entity", obj }
                };
                _regionManager.RequestNavigate("MainRegion", "AddingHumans", parameters);
            }
        }

        private void AddProduct()
        {
            if (MediumPermission)
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(ProductsPrint);
                _regionManager.RequestNavigate("MainRegion", "AddProduct");
            }
        }

        private void AddCategory()
        {
            if (MediumPermission)
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(CategoryPrint);
                var parameters = new NavigationParameters
                                {
                                  { "entity", "categories" }
                                };
                _regionManager.RequestNavigate("MainRegion", "AddTypeCategory", parameters);
            }
        }

        private void AddType()
        {
            if (MediumPermission)
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(ProductTypePrint);
                var parameters = new NavigationParameters
                                {
                                  { "entity", "productType" }
                                };
                _regionManager.RequestNavigate("MainRegion", "AddTypeCategory", parameters);
            }
        }

        private void AddShipper()
        {
            if (MediumPermission)
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(ShippersPrint);
                _regionManager.RequestNavigate("MainRegion", "AddShipper");
            }
        }

        private async void UpdateTime()
        {
            CurrentTime = DateTime.Now.ToShortTimeString();
            await Task.Delay(1000);
            UpdateTime();
        }


        #region Interfaces

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "SelectedItem":

                    if (SelectedItem != null && HighestPermission)
                    {
                        string model = "";
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

                        var parameters = new NavigationParameters
                                {
                                  { "entity", model },
                                  { "item", SelectedItem}
                                };
                        _regionManager.RequestNavigate("MainRegion", "AddingHumans", parameters);
                        SelectedItem = null;
                    }
                    break;

                case "SelectedProduct":
                    if (SelectedProduct != null && MediumPermission)
                    {
                        var parameters = new NavigationParameters
                                {
                                  { "item", SelectedProduct}
                                };
                        _regionManager.RequestNavigate("MainRegion", "AddProduct", parameters);
                        SelectedProduct = null;
                    }
                    break;

                case "SelectedCategory":
                    if (SelectedCategory != null && MediumPermission)
                    {
                        var parameters = new NavigationParameters
                                {
                                  { "entity", "categories" },
                                  { "item", SelectedCategory}
                                };
                        _regionManager.RequestNavigate("MainRegion", "AddTypeCategory", parameters);
                        SelectedCategory = null;
                    }
                    break;

                case "SelectedType":
                    if (SelectedType != null && MediumPermission)
                    {
                        var parameters = new NavigationParameters
                                {
                                  { "entity", "productType" },
                                  { "item", SelectedType}
                                };
                        _regionManager.RequestNavigate("MainRegion", "AddTypeCategory", parameters);
                        SelectedType = null;
                    }
                    break;

                case "SelectedShipper":
                    if (SelectedShipper != null && MediumPermission)
                    {
                        var parameters = new NavigationParameters
                                {
                                  { "item", SelectedShipper}
                                };
                        _regionManager.RequestNavigate("MainRegion", "AddShipper", parameters);
                        SelectedShipper = null;
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
            _isComeIn = false;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (!_isComeIn)
            {
                _accessRole.GetAccessPermission(out _highestPermission,
                                               out _mediumPermission,
                                               out _lowPermission,
                                               out _lowestPermission);
             
                if (LowPermission)
                {
                    ProductsPrint();
                    _tableName = "Product";
                    _isComeIn = true;
                }
                else
                {
                    MessageBox.Show(TranslationSource.Instance["PermissionMsg"], "Information", MessageBoxButton.OK);
                }

                DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);
                UpdateTime();
            }
        }

        #endregion
    }
}
