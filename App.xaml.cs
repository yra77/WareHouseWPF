﻿

using WareHouseWPF.Services.SettingsManager;
using WareHouseWPF.Services.VerifyService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.AccessRoles;
using WareHouseWPF.Services.Repository;
using WareHouseWPF.Services.Search;
using WareHouseWPF.Services.Sort;
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
            regionManager.RegisterViewWithRegion("MainRegion", typeof(AddProduct));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(AddTypeCategory));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(AddShipper));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(Accounting));
            regionManager.RegisterViewWithRegion("MainRegion", typeof(AddAccounting));
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
            containerRegistry.RegisterSingleton<IAccessRole, AccessRole>();
            containerRegistry.RegisterSingleton<ISortService, SortService>();
            containerRegistry.RegisterScoped<ISearch, Search>();


            // register pages
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
            containerRegistry.RegisterForNavigation<Views.Auth, AuthViewModel>();
            containerRegistry.RegisterForNavigation<AddingHumans, AddingHumansViewModel>();
            containerRegistry.RegisterForNavigation<AddProduct, AddProductViewModel>();
            containerRegistry.RegisterForNavigation<AddTypeCategory, AddTypeCategoryViewModel>();
            containerRegistry.RegisterForNavigation<AddShipper, AddShipperViewModel>();
            containerRegistry.RegisterForNavigation<Views.Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<Accounting, AccountingViewModel>();
            containerRegistry.RegisterForNavigation<AddAccounting, AddAccountingViewModel>();
        }
        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }
    }
}
