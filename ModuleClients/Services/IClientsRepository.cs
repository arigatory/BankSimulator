using BankSimulator.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ModuleClients.Services
{
    public interface IClientsRepository
    {
        Task<List<Client>> GetClientsAsync();
    }
}
