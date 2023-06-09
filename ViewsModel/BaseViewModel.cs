﻿

using WareHouseWPF.Models;
using WareHouseWPF.Services.Auth;
using WareHouseWPF.Services.Sort;
using WareHouseWPF.Services.Search;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.AccessRoles;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;
using WareHouseWPF.Services.SettingsManager;

using Prism.Mvvm;
using Prism.Regions;
using Prism.Events;


namespace WareHouseWPF.ViewsModel
{

    public delegate void ChangeNethwork(bool state);


    internal class BaseViewModel : BindableBase
    {

        protected static Employee _employeeAuth;
        protected static bool _stateNethwork;

        protected IEventAggregator _eventAggregator;
        protected ISettingsManager _settingsManager;
        protected ITranslationSource _translation;
        protected IRegionManager _regionManager;
        protected IVerifyService _verifyService;
        protected IDataService _dataService;
        protected ISortService _sortService;
        protected IAccessRole _accessRole;
        protected ISearch _search;
        protected IAuth _auth;

        protected ChangeNethwork _changeNethwork;

        protected bool _isPressed;


        static BaseViewModel()
        {
            _stateNethwork = true;
            _employeeAuth = new Employee();
        }

        public BaseViewModel() 
        {
            _changeNethwork = IsNethwork;
            _isPressed = false;
        }


        #region public property

      

        #endregion


        protected virtual void IsNethwork(bool state)
        {
            _stateNethwork = state;
        }

    }
}
