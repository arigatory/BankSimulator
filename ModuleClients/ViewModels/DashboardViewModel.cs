using BankSimulator.Model;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSimulator.Core.Events;

namespace ModuleClients.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private int _totalClients;
        private long _totalMoney;
        private int _totalProducts;


        public Bank _bank { get; private set; }
        public int TotalClients
        {
            get { return _totalClients; }
            set
            {
                SetProperty(ref _totalClients, value);
            }
        }

        public long TotalMoney
        {
            get { return _totalMoney; }
            set { SetProperty(ref _totalMoney, value); }
        }

        public int TotalProducts
        {
            get { return _totalProducts; }
            set { SetProperty(ref _totalProducts, value); }
        }


        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand NextYearCommand { get; set; }


        public DashboardViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextYearCommand = new DelegateCommand(NextYear);
            _eventAggregator.GetEvent<OneYearPassedEvent>().Subscribe(OneYearPassed);
            _eventAggregator.GetEvent<DeletedItemEvent>().Subscribe(OneYearPassed);
        }

        private void OneYearPassed(string message)
        {
            TotalClients = _bank.TotalClients;
            TotalMoney = _bank.TotalMoney;
            TotalProducts = _bank.TotalProducts;
        }

        private void NextYear()
        {
            foreach (var client in _bank.Clients)
            {
                foreach (var product in client.Products)
                {
                    product.Amount = (int)(product.Amount * (1 + product.Percent / 100));
                }
            }
            _eventAggregator.GetEvent<OneYearPassedEvent>().Publish("One year passed!");
        }

        private void Navigate(string uri)
        {
            var p = new NavigationParameters();
            p.Add("bank", _bank);
            _regionManager.RequestNavigate("ContentRegion", uri, p);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("bank"))
            {
                _bank = navigationContext.Parameters.GetValue<Bank>("bank");
                TotalClients = _bank.Clients.Count;
                TotalMoney = _bank.TotalMoney;
                TotalProducts = _bank.TotalProducts;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
