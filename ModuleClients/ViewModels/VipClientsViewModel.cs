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
    public class VipClientsViewModel : BindableBase
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IRegionManager _regionManager;

        public ObservableCollection<Client> Clients { get; }

        private Client _selectedClient;
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
        public VipClientsViewModel(IRegionManager regionManager)
        {
            _clientsRepository = new ClientsInMemoryDataProvider();
            _regionManager = regionManager;

            Clients = new ObservableCollection<Client>();
            DeleteCommand = new DelegateCommand(OndDeleteExecute, OnDeleteCanExecute);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            LoadAsync();
        }

        private async void LoadAsync()
        {
            var clients = await _clientsRepository.GetClientsAsync();
            foreach (var client in clients)
            {
                Clients.Add(client);
            }
            SelectedClient = Clients.FirstOrDefault();
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
            Clients.Remove(SelectedClient);
        }

        private bool OnDeleteCanExecute()
        {
            return SelectedClient != null;
        }
    }
}
