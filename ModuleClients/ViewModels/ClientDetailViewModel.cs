using BankSimulator.Model;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
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
        
        public ClientDetailViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("client"))
            {
                SelectedClient = navigationContext.Parameters.GetValue<Client>("client");
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
