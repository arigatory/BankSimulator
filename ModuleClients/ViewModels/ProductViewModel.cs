using BankSimulator.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ModuleClients.ViewModels
{
    public class ProductViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; set; }
       
        public ProductViewModel(IRegionManager regionManager)
        {
            NavigateCommand = new DelegateCommand<string>(Navigate);
            _regionManager = regionManager;
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //if (navigationContext.Parameters.ContainsKey("client"))
            //{
            //    SelectedClient = navigationContext.Parameters.GetValue<Client>("client");
            //    Products.Clear();
            //    foreach (var prod in SelectedClient.Products)
            //    {
            //        Products.Add(prod);
            //    }
            //    SelectedProduct = Products.FirstOrDefault();
            //}
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
