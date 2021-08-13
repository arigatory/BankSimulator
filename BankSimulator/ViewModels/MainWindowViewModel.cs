using ModuleClients.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
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

        public DelegateCommand<string> NavigateCommand { get; set; }

        private object _currentView;
        private readonly IRegionManager _regionManager;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                SetProperty(ref _currentView, value);
            }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);
        }
    }
}
