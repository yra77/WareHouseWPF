

using WareHouseWPF.Services.SettingsManager;
using WareHouseWPF.Services.VerifyService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Repository;
using WareHouseWPF.Services.Auth;
using WareHouseWPF.ViewsModel;
using WareHouseWPF.Views;

using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;

using System.Windows;


namespace WareHouseWPF
{

    public partial class App : PrismApplication
    {

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegion>();
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.Auth));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Home));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(AddingHumans));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.Settings));
            //   regionManager.RegisterViewWithRegion("CoinInfoRegion", typeof(Settings));

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            // register services
            containerRegistry.Register<IRepository, Repository>();
            containerRegistry.Register<IDataService, DataService>();
            containerRegistry.Register<IVerifyService, VerifyService>();
            containerRegistry.Register<IAuth, Services.Auth.Auth>();
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<ITranslationSource, TranslationSource>();


            // register pages
           containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
           containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
           containerRegistry.RegisterForNavigation<Views.Auth, AuthViewModel>();
           containerRegistry.RegisterForNavigation<AddingHumans, AddingHumansViewModel>();
           containerRegistry.RegisterForNavigation<Views.Settings, SettingsViewModel>();
        }
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }
    }
}
