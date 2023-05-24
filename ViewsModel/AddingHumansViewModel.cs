

using WareHouseWPF.Models;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Regions;
using Prism.Commands;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using System.Globalization;
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
            PathPhoto = "../Images/editphoto.png";
            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);
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


        private string _pathPhoto;
        public string PathPhoto
        {
            get => _pathPhoto;
            set => SetProperty(ref _pathPhoto, value);
        }


        public DelegateCommand BackBtn => new DelegateCommand(GoBack);
        public DelegateCommand EditPhoto => new DelegateCommand(PhotoClick);
        public DelegateCommand AgreeBtn => new DelegateCommand(AgreeClick);

        #endregion


        private void AgreeClick()
        {
            if(General != null)
            {
                IsLoaded = "Visible";

               //if()
               // {

               // }


                IsLoaded = "Hidden";
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
                    PathPhoto = openFileDialog.FileName;
                }
            }
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home");
        }

        private void Convert_Item(object item)
        {
            if (_modelName == "employee")
            {
               var a = item as Employee;
                General = a;
                EmployeeMod = a;
              //  Debug.WriteLine("AAAAAAAAAAAAAAAAA " + a.Name);
            }
            else if(_modelName == "Client")
            {
                var a = item as ClientModel;
                Debug.WriteLine("ZZZZZZZZZZZ " + a.Name);
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
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            DatePickerLanguage = XmlLanguage.GetLanguage(WareHouseWPF.Properties.Settings.Default.Language.IetfLanguageTag);

            if (navigationContext.Parameters["entity"] != null)
            {
                _modelName = navigationContext.Parameters["entity"].ToString();

                if (_modelName != null)
                {
                    if(_modelName == "client")
                    {
                        IsClientVisible = "Visible";
                        IsEmployeeVisible = "Collapsed";
                        General = new ClientModel();
                        ClientMod = new ClientModel();
                    }
                    else if(_modelName == "employee")
                    {
                        IsEmployeeVisible = "Visible";
                        IsClientVisible = "Collapsed";
                        General = new Employee();
                        EmployeeMod = new Employee();
                    }

                    var item = navigationContext.Parameters["item"];
                    if (item != null)
                    {
                        Convert_Item(item);
                    }

                    RaisePropertyChanged("List");
                }
                else
                {

                }
            }
        }

        #endregion
    }
}
