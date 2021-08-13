using ModuleClients.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace BankSimulator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand ClientsViewCommand { get; set; }
        public DelegateCommand DiscoveryViewCommand { get; set; }


        public ClientsViewModel ClientsVM { get; set; }
        //public DiscoveryViewModel DiscoveryVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                SetProperty(ref _currentView, value);
            }
        }

        public MainWindowViewModel()
        {
            ClientsVM = new ClientsViewModel();
            //DiscoveryVM = new DiscoveryViewModel();
            CurrentView = ClientsVM;

            ClientsViewCommand = new DelegateCommand(OnClientsView);
       
            //DiscoveryViewCommand = new DelegateCommand<object>(o =>
            //{
            //    CurrentView = DiscoveryVM;
            //});

        }

        private void OnClientsView()
        {
            CurrentView = ClientsVM;
        }
    }
}
