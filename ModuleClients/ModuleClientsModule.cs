using ModuleClients.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleClients
{
    public class ModuleClientsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleClientsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RegisterViewWithRegion("ContentRegion", typeof(DashboardView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ClientsView>();
            containerRegistry.RegisterForNavigation<DashboardView>();
            containerRegistry.RegisterForNavigation<ClientDetailView>();
            containerRegistry.RegisterForNavigation<ProductView>();
            containerRegistry.RegisterForNavigation<VipClientsView>();
            containerRegistry.RegisterForNavigation<OrganizationsView>();
        }
    }
}
