using BankSimulator.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleClients.ViewModels
{
    public class ClientDetailViewModel : BindableBase, INavigationAware
    {
        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                SetProperty(ref _selectedClient, value);
            }
        }

        private Product _selectedProduct;
        private readonly IRegionManager _regionManager;

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                SetProperty(ref _selectedProduct, value);
                CloseProductCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Product> Products { get; set; }

        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand CloseProductCommand { get; set; }
        public DelegateCommand OpenProductCommand { get; set; }



        public ClientDetailViewModel(IRegionManager regionManager)
        {
            CloseProductCommand = new DelegateCommand(CloseProductExecute, CloseProductCanExecute);
            OpenProductCommand = new DelegateCommand(OpenProductExecute);
            Products = new ObservableCollection<Product>();
            NavigateCommand = new DelegateCommand<string>(Navigate);
            _regionManager = regionManager;
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }


        private void OpenProductExecute()
        {
            var p = new Product{
                Name = "Новый банковский продукт",
                Amount = 0,
                Number = 123,
                OpenedDate = DateTime.Now,
                Percent = 7.0
            };
            SelectedClient.Products.Add(p);
            Products.Add(p);
        }

        private void CloseProductExecute()
        {
            SelectedClient.Products.Remove(SelectedProduct);
            Products.Remove(SelectedProduct);
        }

        private bool CloseProductCanExecute()
        {
            return SelectedProduct != null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("client"))
            {
                SelectedClient = navigationContext.Parameters.GetValue<Client>("client");
                Products.Clear();
                foreach (var prod in SelectedClient.Products)
                {
                    Products.Add(prod);
                }
                SelectedProduct = Products.FirstOrDefault();
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
