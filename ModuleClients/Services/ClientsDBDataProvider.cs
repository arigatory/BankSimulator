using BankSimulator.DataAccess;
using BankSimulator.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModuleClients.Services
{

    public class ClientsDBDataProvider : IClientsRepository
    {
        public async Task<List<Client>> GetClientsAsync()
        {
            PersonsDal dal = new PersonsDal();

            var persons = await dal.GetAllPersons();
            var organizations = await dal.GetAllOrganizations();

            var clients = new List<Client>();
     
            foreach (var pers in persons)
            {
                clients.Add(pers);
            }
            foreach (var org in organizations)
            {
                clients.Add(org);
            }
            return clients;
        }
    }
}
