

using WareHouseWPF.Models;
using WareHouseWPF.Controls.Listviews;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;

using Prism.Regions;
using Prism.Commands;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;


namespace WareHouseWPF.ViewsModel
{
    internal class AddingHumansViewModel : BaseViewModel, INavigationAware
    {



        public AddingHumansViewModel(IRegionManager regionManager,
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
        }


        #region property

        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }

        #endregion


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
           return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["entity"] != null)
            {
               var entity = navigationContext.Parameters["entity"].ToString();

                if (entity != null)
                {
                    Debug.WriteLine("QQQQQQQQQQQ " + entity);
                }
                else
                {

                }
            }
        }
    }
}
