using BankSimulator.Model;
using ModuleClients.Services;
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
    public class OrganizationsViewModel : BindableBase, INavigationAware
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IRegionManager _regionManager;

        public ObservableCollection<Client> Organizations { get; }

        private Client _selectedOrganization;
        private Bank _bank;

        public Client SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                SetProperty(ref _selectedOrganization, value);
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }


        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        public OrganizationsViewModel(IRegionManager regionManager)
        {
            _clientsRepository = new ClientsInMemoryDataProvider();
            _regionManager = regionManager;

            Organizations = new ObservableCollection<Client>();
            DeleteCommand = new DelegateCommand(OndDeleteExecute, OnDeleteCanExecute);
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }


        private void Navigate(string uri)
        {
            if (SelectedOrganization == null)
            {
                return;
            }

            var p = new NavigationParameters();
            p.Add("client", _selectedOrganization);
            _regionManager.RequestNavigate("ContentRegion", uri, p);
        }


        private void OndDeleteExecute()
        {
            _bank.Clients.Remove(SelectedOrganization);
            Organizations.Remove(SelectedOrganization);
        }

        private bool OnDeleteCanExecute()
        {
            return SelectedOrganization != null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("bank"))
            {
                _bank = navigationContext.Parameters.GetValue<Bank>("bank");
                Organizations.Clear();
                foreach (var client in _bank.Clients)
                {
                    if (client is Organization)
                    {
                        Organizations.Add(client);
                    }
                }
                SelectedOrganization = Organizations.FirstOrDefault();
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
