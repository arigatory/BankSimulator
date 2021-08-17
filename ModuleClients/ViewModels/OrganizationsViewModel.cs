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
    public class OrganizationsViewModel : BindableBase
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IRegionManager _regionManager;

        public ObservableCollection<Client> Organizations { get; }

        private Client _selectedOrganization;
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
            LoadAsync();
        }

        private async void LoadAsync()
        {
            var clients = await _clientsRepository.GetClientsAsync();
            foreach (var client in clients)
            {
                Organizations.Add(client);
            }
            SelectedOrganization = Organizations.FirstOrDefault();
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
            Organizations.Remove(SelectedOrganization);
        }

        private bool OnDeleteCanExecute()
        {
            return SelectedOrganization != null;
        }
    }
}
