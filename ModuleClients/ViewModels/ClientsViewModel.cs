using BankSimulator.Core.Events;
using BankSimulator.Model;
using ModuleClients.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModuleClients.ViewModels
{
    public class ClientsViewModel : BindableBase, INavigationAware
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<Client> Clients { get; }

        private Client _selectedClient;
        protected Bank _bank;

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                SetProperty(ref _selectedClient, value);
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }


        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public ClientsViewModel(IRegionManager regionManager,
            IEventAggregator eventAggregator)
        {
            _clientsRepository = new ClientsInMemoryDataProvider();
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            Clients = new ObservableCollection<Client>();
            DeleteCommand = new DelegateCommand(OndDeleteExecute, OnDeleteCanExecute);
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uri)
        {
            if (SelectedClient == null)
            {
                return;
            }

            var p = new NavigationParameters();
            p.Add("client", _selectedClient);
            _regionManager.RequestNavigate("ContentRegion", uri, p);
        }


        private void OndDeleteExecute()
        {
            _bank.Clients.Remove(SelectedClient);
            Clients.Remove(SelectedClient);
            SelectedClient = Clients.FirstOrDefault();
            _eventAggregator.GetEvent<DeletedItemEvent>().Publish("");
        }

        private bool OnDeleteCanExecute()
        {
            return SelectedClient != null;
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("bank"))
            {
                _bank = navigationContext.Parameters.GetValue<Bank>("bank");
                Clients.Clear();
                foreach (var client in _bank.Clients)
                {
                    if (client is Person  && !client.IsVIP)
                    {
                        Clients.Add(client);
                    }
                }
                SelectedClient = Clients.FirstOrDefault();
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
