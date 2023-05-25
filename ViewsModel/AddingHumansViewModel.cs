

using WareHouseWPF.Models;
using WareHouseWPF.Converters;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Regions;
using Prism.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using Microsoft.Win32;
using System.Windows;


namespace WareHouseWPF.ViewsModel
{
    internal class AddingHumansViewModel : BaseViewModel, INavigationAware
    {


        private string _modelName;


        public AddingHumansViewModel(IRegionManager regionManager,
                             ITranslationSource translation,
                             IVerifyService verifyService,
                             IDataService dataService)
            : base()
        {
            _regionManager = regionManager;
            _verifyService = verifyService;
            _dataService = dataService;
            _translation = translation;

            IsLoaded = "Hidden";//Visible
            IsClientVisible = "Collapsed";
            IsEmployeeVisible = "Collapsed";
            IsDeleteVisible = "Collapsed";
            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);
            IsValidInput = false;
        }


        #region property

        private HumanBaseModel _general;
        public HumanBaseModel General
        {
            get => _general;
            set => SetProperty(ref _general, value);
        }

        private ClientModel _clientMod;
        public ClientModel ClientMod
        {
            get => _clientMod;
            set => SetProperty(ref _clientMod, value);
        }


        private Employee _employeeMod;
        public Employee EmployeeMod
        {
            get => _employeeMod;
            set => SetProperty(ref _employeeMod, value);
        }


        private bool _isValidInput;
        public bool IsValidInput 
        { 
            get => _isValidInput; 
            set => SetProperty(ref _isValidInput, value); 
        }


        private XmlLanguage _datePickerLanguage;
        public XmlLanguage DatePickerLanguage
        {
            get => _datePickerLanguage;
            set => SetProperty(ref _datePickerLanguage, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        private string _isClientVisible;
        public string IsClientVisible
        {
            get => _isClientVisible;
            set => SetProperty(ref _isClientVisible, value);
        }


        private string _isEmployeeVisible;
        public string IsEmployeeVisible
        {
            get => _isEmployeeVisible;
            set => SetProperty(ref _isEmployeeVisible, value);
        }


        private string _isDeleteVisible;
        public string IsDeleteVisible
        {
            get => _isDeleteVisible;
            set => SetProperty(ref _isDeleteVisible, value);
        }


        private byte[] _pathPhoto;
        public byte[] PathPhoto
        {
            get => _pathPhoto;
            set => SetProperty(ref _pathPhoto, value);
        }

        private BankDetails _bankDetails;
        public BankDetails Bank_Details
        {
            get => _bankDetails;
            set => SetProperty(ref _bankDetails, value);
        }


        public DelegateCommand BackBtn => new DelegateCommand(GoBack);
        public DelegateCommand EditPhoto => new DelegateCommand(PhotoClick);
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

                if (_modelName == "client")
                {
                    result = await _dataService.DeleteAsync<ClientModel>(General.Id);
                }
                else if (_modelName == "employee")
                {
                    result = await _dataService.DeleteAsync<Employee>(General.Id);
                }

                IsLoaded = "Hidden";

                if (result)
                {
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

            if (!_isPressed && General != null && IsValidInput && PathPhoto != null && General.DateOfBirth != null)
            {
                _isPressed = true;
                IsLoaded = "Visible";
                bool result = false;

                if (_modelName == "client")
                {
                    ClientMod.Id = General.Id;
                    ClientMod.Name = General.Name;
                    ClientMod.SecondName = General.SecondName;
                    ClientMod.DateOfBirth = General.DateOfBirth;
                    ClientMod.Email = General.Email;
                    ClientMod.PhoneNumber = General.PhoneNumber;
                    ClientMod.City = General.City;
                    ClientMod.Region = General.Region;
                    ClientMod.House = General.House;
                    ClientMod.Street = General.Street;
                    ClientMod.Apartment = General.Apartment;
                    ClientMod.DateOfRegistration = General.DateOfRegistration;

                    ClientMod.BanksAccount = new List<BankDetails>() { Bank_Details };

                    result = await _dataService.InsertUpdateAsync(ClientMod.Id, ClientMod);

                }
                else if (_modelName == "employee")
                {
                    EmployeeMod.Id = General.Id;
                    EmployeeMod.Name = General.Name;
                    EmployeeMod.SecondName = General.SecondName;
                    EmployeeMod.DateOfBirth = General.DateOfBirth;
                    EmployeeMod.Email = General.Email;
                    EmployeeMod.PhoneNumber = General.PhoneNumber;
                    EmployeeMod.City = General.City;
                    EmployeeMod.Region = General.Region;
                    EmployeeMod.House = General.House;
                    EmployeeMod.Street = General.Street;
                    EmployeeMod.Apartment = General.Apartment;
                    EmployeeMod.DateOfRegistration = General.DateOfRegistration;

                    EmployeeMod.Photo = PathPhoto;
                    EmployeeMod.BankAccount = new List<BankDetails>(){Bank_Details};

                    result = await _dataService.InsertUpdateAsync(EmployeeMod.Id, EmployeeMod);
                }

                IsLoaded = "Hidden";

                if (result)
                {
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

        private void PhotoClick()
        {
            string message = TranslationSource.Instance["MsgCamFile"];

            var res = MessageBox.Show(message, TranslationSource.Instance["Notification"], MessageBoxButton.OKCancel);

            if (res == MessageBoxResult.OK)
            {

            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.jpg;*.png;*.jpeg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    PathPhoto = PathToByteArray.ToByteArr(openFileDialog.FileName);
                }
            }
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home");
        }

        private void Convert_Item(object item)
        {
            IsDeleteVisible = "Visible";

            if (_modelName == "employee")
            {
                var a = item as Employee;
                General = a;
                EmployeeMod = a;

                if (a.BankAccount != null && a.BankAccount.Count > 0)
                {
                    Bank_Details = a.BankAccount[0];
                }

                if (EmployeeMod.Photo != null)
                {
                    PathPhoto = EmployeeMod.Photo;
                }
                //  Debug.WriteLine("AAAAAAAAAAAAAAAAA " + a.Name);
            }
            else if (_modelName == "Client")
            {
                var a = item as ClientModel;
                General = a;
                ClientMod = a;

                if (a.BanksAccount != null && a.BanksAccount.Count > 0)
                {
                    Bank_Details = a.BanksAccount[0];
                }

                // Debug.WriteLine("ZZZZZZZZZZZ " + a.Name);
            }
        }

        #region implement interface

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case "General":
                    //  Debug.WriteLine("AAAAAAAAAAAA " + General.SecondName);
                    break;
                case "General.SecondName":
                    //    Debug.WriteLine("IIIIIIIIIII " + General.SecondName);
                    break;

                default:
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsDeleteVisible = "Collapsed";
            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);

            if (navigationContext.Parameters["entity"] != null)
            {
                _modelName = navigationContext.Parameters["entity"].ToString();

                Bank_Details = new BankDetails();
                Bank_Details.Name = "aaaa";
                Bank_Details.BankAccounts = "00000000000000000";
                Bank_Details.ClientId = null;
                Bank_Details.EmployeeId = null;
                Bank_Details.ShipperId = null;

                if (_modelName != null)
                {
                    if (_modelName == "client")
                    {
                        IsClientVisible = "Visible";
                        IsEmployeeVisible = "Collapsed";
                        General = new ClientModel();
                    }
                    else if (_modelName == "employee")
                    {
                        IsEmployeeVisible = "Visible";
                        IsClientVisible = "Collapsed";
                        General = new Employee();
                    }

                    ClientMod = new ClientModel();
                    ClientMod.BanksAccount = new List<BankDetails>();

                    EmployeeMod = new Employee();
                    EmployeeMod.BankAccount = new List<BankDetails>();
                    PathPhoto = PathToByteArray.ToByteArr("../../Images/editphoto.png");

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
