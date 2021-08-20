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

namespace ModuleClients.ViewModels
{
    public class VipClientsViewModel : ClientsViewModel
    {
        public VipClientsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator): 
            base(regionManager, eventAggregator)
        {

        }

        override public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("bank"))
            {
                _bank = navigationContext.Parameters.GetValue<Bank>("bank");
                Clients.Clear();
                foreach (var client in _bank.Clients)
                {
                    if (client is Person && client.IsVIP)
                    {
                        Clients.Add(client);
                    }
                }
                SelectedClient = Clients.FirstOrDefault();
            }
        }
    }
}
