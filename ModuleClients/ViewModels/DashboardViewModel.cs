using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleClients.ViewModels
{
    public class DashboardViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        private int _totalClients;

        public int TotalClients
        {
            get { return _totalClients; }
            set
            {
                _totalClients = value;
                SetProperty(ref _totalClients, value);
            }
        }



        public DelegateCommand<string> NavigateCommand { get; set; }

        public DashboardViewModel(IRegionManager regionManager)
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
