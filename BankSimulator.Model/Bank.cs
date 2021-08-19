using System.Collections.Generic;
using System.Linq;

namespace BankSimulator.Model
{
    public class Bank
    {
        public List<Client> Clients { get; set; }
        public int TotalClients
        {
            get => Clients.Count();
        }

        public Bank()
        {
            Clients = new List<Client>();
        }
    }
}
