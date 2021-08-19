using BankSimulator.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleClients.ViewModels
{
    public class DashboardViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _regionManager;
        private int _totalClients;
        private int _totalMoney;
        private int _totalProducts;


        public Bank _bank { get; private set; }
        public int TotalClients
        {
            get { return _totalClients; }
            set
            {
                _totalClients = value;
                SetProperty(ref _totalClients, value);
            }
        }
        public int TotalMoney
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


        public DashboardViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextYearCommand = new DelegateCommand(NextYear);
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
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("bank"))
            {
                _bank = navigationContext.Parameters.GetValue<Bank>("bank");
                TotalClients = _bank.TotalClients;
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
