

using WareHouseWPF.Events;
using WareHouseWPF.Models;
using WareHouseWPF.Services.Sort;
using WareHouseWPF.Services.AccessRoles;
using WareHouseWPF.Services.DataService;
using WareHouseWPF.Services.Localisation;
using WareHouseWPF.Services.VerifyService;
using WareHouseWPF.Controls.AccountingList;

using Prism.Events;
using Prism.Regions;
using Prism.Commands;

using System.Collections.Generic;
using System.ComponentModel;


namespace WareHouseWPF.ViewsModel
{
    internal class AccountingViewModel : BaseViewModel, INavigationAware
    {


        private string _model;


        public AccountingViewModel(IRegionManager regionManager,
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
            _translation = translation;
            _sortService = sortService;
            _accessRole = accessRole;

            IsLoaded = "Hidden";//Visible

            _regionManager.RegisterViewWithRegion("AccountingListRegion", typeof(TransferList));
            _regionManager.RegisterViewWithRegion("AccountingListRegion", typeof(ArrivalList));
        }


        #region property

        private List<TransferOfGoods> _listTransfer;
        public List<TransferOfGoods> ListTransfer
        { 
            get => _listTransfer;
            set => SetProperty(ref _listTransfer, value); 
        }


        private List<ArrivalOfGoods> _listArrival;
        public List<ArrivalOfGoods> ListArrival
        {
            get => _listArrival;
            set => SetProperty(ref _listArrival, value);
        }


        private string _isLoaded;
        public string IsLoaded
        {
            get => _isLoaded;
            set => SetProperty(ref _isLoaded, value);
        }


        private TransferOfGoods _selectedTransfer;
        public TransferOfGoods SelectedTransfer
        {
            get => _selectedTransfer;
            set => SetProperty(ref _selectedTransfer, value);
        }


        private ArrivalOfGoods _selectedArrival;
        public ArrivalOfGoods SelectedArrival
        {
            get => _selectedArrival;
            set => SetProperty(ref _selectedArrival, value);
        }


        public DelegateCommand BackBtn => new DelegateCommand(GoBack);
        public DelegateCommand AddBtn => new DelegateCommand(AddClick);
        public DelegateCommand<string> HeaderClick => new DelegateCommand<string>(Header_Click);

        #endregion


        private void Header_Click(string columnName)
        {

            switch (_model)
            {
                case "arrival":
                    ListArrival = _sortService.SortArrival(columnName, ListArrival);
                    RaisePropertyChanged("ListArrival");
                    break;

                case "transfer":
                    ListTransfer = _sortService.SortTransfer(columnName, ListTransfer);
                    RaisePropertyChanged("ListTransfer");
                    break;
            }
        }

        private async void ArrivalPrint()
        {
            IsLoaded = "Visible";
            ListArrival = await _dataService.GetDataAsync<ArrivalOfGoods>();
            _regionManager.RequestNavigate("AccountingListRegion", "ArrivalList");
            RaisePropertyChanged("ListArrival");
            IsLoaded = "Hidden";//Visible
        }

        private async void TransferPrint()
        {
            IsLoaded = "Visible";
            _regionManager.RequestNavigate("AccountingListRegion", "TransferList");
            ListTransfer = await _dataService.GetDataAsync<TransferOfGoods>();
            RaisePropertyChanged("ListTransfer");
            IsLoaded = "Hidden";//Visible
        }

        private void SubscribeEvent()
        {
            if (_model == "arrival")
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(ArrivalPrint);
            }
            else if(_model == "transfer")
            {
                _eventAggregator.GetEvent<MyEvent>().Subscribe(TransferPrint);
            }
        }

        private void AddClick()
        {
            SubscribeEvent();

            var param = new NavigationParameters
            {
                {"entity", _model }
            };
            _regionManager.RequestNavigate("MainRegion", "AddAccounting", param);
        }

        private void GoBack()
        {
            _regionManager.RequestNavigate("MainRegion", "Home");
        }


        #region implement intarface

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            NavigationParameters parameters = null; ;

            switch (args.PropertyName)
            {
                case "SelectedArrival":
                     parameters = new NavigationParameters
                                {
                                  { "entity", _model },
                                  { "item", SelectedArrival}
                                };
                    break;

                case "SelectedTransfer":
                    parameters = new NavigationParameters
                                {
                                  { "entity", _model },
                                  { "item", SelectedTransfer}
                                };
                    break;
            }

            if(SelectedArrival != null || SelectedTransfer != null)
            {
                SubscribeEvent();
                _regionManager.RequestNavigate("MainRegion", "AddAccounting", parameters);
                SelectedArrival = null;
                SelectedTransfer = null;
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

            if (navigationContext.Parameters["entity"] != null)
            {
                _model = navigationContext.Parameters["entity"].ToString();

                if (_model != null)
                {
                    if (_model == "arrival")
                    {
                        ArrivalPrint();
                    }
                    else if (_model == "transfer")
                    {
                        TransferPrint();
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
