using System.Collections.Generic;
using System.Linq;

namespace BankSimulator.Model
{
    public class Bank
    {
        private static Bank instance;
        private Bank()
        {
            Clients = new List<Client>();
        }

        public static Bank getInstance()
        {
            if(instance == null)
                instance = new Bank();
            return instance;
        }


        public List<Client> Clients { get; set; }
        public int TotalClients
        {
            get => Clients.Count;
        }
        public int TotalMoney
        {
            get
            {
                double total = Clients.Sum(client => client.Balance);
                return (int)total;
            }
        }

        public int TotalProducts
        {
            get
            {
                return Clients.Sum(c => c.Products.Count);
            }
        }

    }
}
