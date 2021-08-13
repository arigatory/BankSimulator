using BankSimulator.Model;
using ModuleClients.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleClients.ViewModels
{
    public class ClientsViewModel : BindableBase
    {
        private readonly IClientsRepository _clientsRepository;
        public ObservableCollection<Client> Clients { get; }

        public ClientsViewModel()
        {
            _clientsRepository = new ClientsRepository();

            Clients = new ObservableCollection<Client>(_clientsRepository.GetClientsAsync().Result);
        }
    }
}
