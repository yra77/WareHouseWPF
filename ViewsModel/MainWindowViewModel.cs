

using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.SettingsManager;
using Prism.Regions;


namespace WareHouseWPF.ViewsModel
{
    internal class MainWindowViewModel : BaseViewModel, INavigationAware
    {


        public MainWindowViewModel(IRegionManager regionManager,
                                   ITranslationSource translation,
                                   ISettingsManager settingsManager)
            : base()
        {
            _settingsManager = settingsManager;
            _regionManager = regionManager;
            _translation = translation;
           
            _translation.CurrentCulture = WareHouseWPF.Properties.Settings.Default.Language;//_settingsManager.Load().Language;
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.Auth));
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
           
        }

    }
}
