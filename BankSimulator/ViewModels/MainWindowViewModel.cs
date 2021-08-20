using BankSimulator.Model;
using ModuleClients.Services;
using ModuleClients.ViewModels;
using ModuleClients.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace BankSimulator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Банк Skillbox";
        private object _currentView;
        private readonly IRegionManager _regionManager;
        private readonly IClientsRepository _clientsRepository;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                SetProperty(ref _currentView, value);
            }
        }
        public Bank SkillboxBank{ get; }



        public MainWindowViewModel(IRegionManager regionManager)
        {
            _clientsRepository = new ClientsInMemoryDataProvider();
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            SkillboxBank = Bank.getInstance();
            LoadAsync();
            Navigate(nameof(DashboardView));
        }

        private async void LoadAsync()
        {
            var clients = await _clientsRepository.GetClientsAsync();
            foreach (var client in clients)
            {
                SkillboxBank.Clients.Add(client);
            }
        }

        private void Navigate(string uri)
        {
            var p = new NavigationParameters();
            p.Add("bank", SkillboxBank);
            _regionManager.RequestNavigate("ContentRegion", uri, p);
        }
    }
}
