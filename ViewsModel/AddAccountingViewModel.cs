

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.AccessRoles;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Events;
using Prism.Regions;
using Prism.Commands;

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;

namespace WareHouseWPF.ViewsModel
{
    internal class AddAccountingViewModel : BaseViewModel, INavigationAware
    {


        private string _model;


        public AddAccountingViewModel(IRegionManager regionManager,
                             IVerifyService verifyService,
                             IDataService dataService,
                             IAccessRole accessRole,
                             ITranslationSource translation,
                             IEventAggregator eventAggregator)
            : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _translation = translation;
            _accessRole = accessRole;

            IsLoaded = "Hidden";//Visible
            IsTransferVisible = "Collapsed";
            IsArrivalVisible = "Collapsed";
            IsDeleteVisible = "Collapsed";
            Quantity = 0;
            Code = "000000000000";
            Id = 0;
        }


        #region property

        private BaseModel _general;
        public BaseModel General
        {
            get => _general;
            set => SetProperty(ref _general, value);
        }


        private List<ClientModel> _listClient;
        public List<ClientModel> ListClient
        {
            get => _listClient;
            set => SetProperty(ref _listClient, value);
        }


        private List<Shipper> _listShipper;
        public List<Shipper> ListShipper
        {
            get => _listShipper;
            set => SetProperty(ref _listShipper, value);
        }


        private ObservableCollection<ListCode> _listCode_Quantity;
        public ObservableCollection<ListCode> ListCode_Quantity
        {
            get => _listCode_Quantity;
            set => SetProperty(ref _listCode_Quantity, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        private int _employeeId;
        public int EmployeeId
        {
            get => _employeeId;
            set => SetProperty(ref _employeeId, value);
        }


        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }


        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }


        private ClientModel _selectClient;
        public ClientModel SelectClient
        {
            get => _selectClient;
            set => SetProperty(ref _selectClient, value);
        }


        private Shipper _selectShipper;
        public Shipper SelectShipper
        {
            get => _selectShipper;
            set => SetProperty(ref _selectShipper, value);
        }


        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }


        private bool _isValidInput;
        public bool IsValidInput
        {
            get => _isValidInput;
            set => SetProperty(ref _isValidInput, value);
        }


        private string _extCodeQuantity;
        public string TextCodeQuantity
        {
            get => _extCodeQuantity;
            set => SetProperty(ref _extCodeQuantity, value);
        }


        private string _isTransferVisible;
        public string IsTransferVisible
        {
            get => _isTransferVisible;
            set => SetProperty(ref _isTransferVisible, value);
        }


        private string _isArrivalVisible;
        public string IsArrivalVisible
        {
            get => _isArrivalVisible;
            set => SetProperty(ref _isArrivalVisible, value);
        }


        private string _isDeleteVisible;
        public string IsDeleteVisible
        {
            get => _isDeleteVisible;
            set => SetProperty(ref _isDeleteVisible, value);
        }


        private ListCode _selectedListCode;
        public ListCode SelectedListCode
        {
            get => _selectedListCode;
            set => SetProperty(ref _selectedListCode, value);
        }


        public DelegateCommand BackBtn => new DelegateCommand(GoBack);
        public DelegateCommand AgreeBtn => new DelegateCommand(AgreeClick);
        public DelegateCommand DeleteBtn => new DelegateCommand(DeleteClickAsync);
        public DelegateCommand AddListCode => new DelegateCommand(Add_ListCode);
        public DelegateCommand<string> DeleteListCodeItem => new DelegateCommand<string>(Delete_ListCodeItem);

        #endregion


        private void Delete_ListCodeItem(string obj)
        {
            var item = ListCode_Quantity.FirstOrDefault(x => x.Id == Int32.Parse(obj));

            if (ListCode_Quantity.Remove(item))
            {
                RaisePropertyChanged("ListCode_Quantity");
            }
            else
            {
                MessageBox.Show(TranslationSource.Instance["ErrorDeletData"], "Error", MessageBoxButton.OK);
            }
        }

        private async void DeleteClickAsync()
        {
            if (!_isPressed)
            {
                _isPressed = true;
                bool result = false;
                IsLoaded = "Visible";

                if (_model == "arrival")
                {
                    result = await _dataService.DeleteAsync<ArrivalOfGoods>(General.Id);
                }
                else if (_model == "transfer")
                {
                    result = await _dataService.DeleteAsync<TransferOfGoods>(General.Id);
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

            if (!_isPressed && General != null && IsValidInput)
            {
                _isPressed = true;
                var result = false;
                IsLoaded = "Visible";

                if (_model == "arrival")
                {
                    var transfer = General as ArrivalOfGoods;
                    transfer.Id = General.Id;
                    transfer.Name = General.Name;
                    transfer.EmployeeId = EmployeeId;
                    transfer.ShipperId = Id;
                    transfer.WarehouseNumber = (General as ArrivalOfGoods).WarehouseNumber;
                    transfer.Code_Quantity = new List<ListCode>(ListCode_Quantity);

                    result = await _dataService.InsertUpdateAsync(transfer.Id, transfer);
                }
                else if (_model == "transfer")
                {
                    var transfer = General as TransferOfGoods;
                    transfer.Id = General.Id;
                    transfer.Name = General.Name;
                    transfer.EmployeeId = EmployeeId;
                    transfer.ClientId = Id;
                    transfer.WarehouseNumber = (General as TransferOfGoods).WarehouseNumber;
                    transfer.Code_Quantity = new List<ListCode>(ListCode_Quantity);

                    result = await _dataService.InsertUpdateAsync(transfer.Id, transfer);
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

        private void Add_ListCode()
        {
            ListCode_Quantity.Add(new ListCode() { Code = Code, Quantity = Quantity });
            RaisePropertyChanged("ListCode_Quantity");
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Accounting");
        }

        private void Convert_Item(object item)
        {

            IsDeleteVisible = "Visible";

            if (_model == "arrival")
            {
                if (item is ArrivalOfGoods arrival)
                {
                    General = arrival;

                    EmployeeId = (int)arrival.EmployeeId;
                    Id = (int)arrival.ShipperId;
                    ListCode_Quantity = new ObservableCollection<ListCode>(arrival.Code_Quantity);
                    SelectShipper = ListShipper.FirstOrDefault(x => x.Id == Id);

                    if (ListCode_Quantity != null && ListCode_Quantity.Count > 0)
                    {
                        TextCodeQuantity = ListCode_Quantity[0].Code + " " + ListCode_Quantity[0].Quantity;
                    }
                }
            }
            else if (_model == "transfer")
            {
                if (item is TransferOfGoods transfer)
                {
                    General = transfer;

                    EmployeeId = (int)transfer.EmployeeId;
                    Id = (int)transfer.ClientId;
                    ListCode_Quantity = new ObservableCollection<ListCode>(transfer.Code_Quantity);
                    SelectClient = ListClient.FirstOrDefault(x => x.Id == Id);

                    if (ListCode_Quantity != null && ListCode_Quantity.Count > 0)
                    {
                        TextCodeQuantity = ListCode_Quantity[0].Code + " " + ListCode_Quantity[0].Quantity;
                    }
                }
            }
        }


        #region implement intarface

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "SelectedListCode":
                    if (TextCodeQuantity != null)
                    {
                        TextCodeQuantity = SelectedListCode.Code + " " + SelectedListCode.Quantity;
                    }
                    RaisePropertyChanged("ListCode_Quantity");
                    break;

                case "SelectClient":
                    if (SelectClient != null)
                    {
                        Id = SelectClient.Id;
                    }
                    break;

                case "SelectShipper":
                    if (SelectShipper != null)
                    {
                        Id = SelectShipper.Id;
                    }
                    break;
            }

            TextCodeQuantity = null;
            SelectedListCode = null;
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

            SelectClient = null;
            SelectShipper = null;
            IsValidInput = false;
            IsDeleteVisible = "Collapsed";
            IsTransferVisible = "Collapsed";
            IsArrivalVisible = "Collapsed";
            Quantity = 0;
            Code = "000000000000";

            if (navigationContext.Parameters["entity"] != null)
            {
                _model = navigationContext.Parameters["entity"].ToString();
                ListCode_Quantity = new ObservableCollection<ListCode>();
                EmployeeId = _employeeAuth.Id;
                Id = 0;

                if (_model != null)
                {
                    if (_model == "arrival")
                    {
                        General = new ArrivalOfGoods();
                        IsArrivalVisible = "Visible";
                        ListShipper = new List<Shipper>(await _dataService.GetDataAsync<Shipper>());
                    }
                    else if (_model == "transfer")
                    {
                        General = new TransferOfGoods();
                        IsTransferVisible = "Visible";

                        ListClient = new List<ClientModel>(await _dataService.GetDataAsync<ClientModel>());
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
