using BankSimulator.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ModuleClients.Services
{
    public interface IClientsRepository
    {
        Task<List<Client>> GetClientsAsync();
        Task<Client> GetClientAsync(string name);
        Task<Client> UpdateClient(string name);
        Task DleteCustomerAsync(string name);
    }
}
