

using WareHouseWPF.Services.SettingsManager;
using WareHouseWPF.Services.Localisation;

using Prism.Commands;
using Prism.Regions;

using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;

namespace WareHouseWPF.ViewsModel
{
    internal class SettingsViewModel : BaseViewModel, INavigationAware
    {


        public SettingsViewModel(IRegionManager regionManager,
                                 ITranslationSource translation,
                                 ISettingsManager settingsManager)
            : base()
        {
            _settingsManager = settingsManager;
            _regionManager = regionManager;
            _translation = translation;
        }


        #region property

        private bool _isUkrainian;
        public bool IsUkrainian
        {
            get => _isUkrainian;
            set => SetProperty(ref _isUkrainian, value);
        }

        private bool _isEnglish;
        public bool IsEnglish
        {
            get => _isEnglish;
            set => SetProperty(ref _isEnglish, value);
        }

        public DelegateCommand BackBtn => new DelegateCommand(GoBack);

        #endregion


        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home");
        }



        #region implement interface

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);


            switch (args.PropertyName)
            {
                case "IsEnglish":
                    IsUkrainian = false;
                    _translation.CurrentCulture = CultureInfo.CreateSpecificCulture("en"); 
                    WareHouseWPF.Properties.Settings.Default.Language = _translation.CurrentCulture;
                    WareHouseWPF.Properties.Settings.Default.Save();
                    //_settingsManager.SaveProperty(language: _translation.CurrentCulture);
                    break;
                case "IsUkrainian":
                    IsEnglish = false;
                    _translation.CurrentCulture = CultureInfo.CreateSpecificCulture("uk");
                    WareHouseWPF.Properties.Settings.Default.Language = _translation.CurrentCulture;
                    WareHouseWPF.Properties.Settings.Default.Save();
                   // _settingsManager.SaveProperty(language: _translation.CurrentCulture);
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
            if (_translation.CurrentCulture.Name == "uk-UA")
            {
                IsUkrainian = true;
                IsEnglish = false;
            }
            else
            {
                IsEnglish = true;
                IsUkrainian = false;
            }
        }

        #endregion
    }
}
