using BankSimulator.DataAccess;
using BankSimulator.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ModuleClients.Services
{
    public class ClientsRepository : IClientsRepository
    {
        public async Task<List<Client>> GetClientsAsync()
        {
            var fk =  new FakeContext();
            return fk.Clients.ToList();
        }
    }
}
