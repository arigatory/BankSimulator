using System.Collections.Generic;
using System.Linq;

namespace BankSimulator.Model
{
    public class Client
    {
        public Client()
        {
            Products = new List<Product>();
        }
        public int Balance
        {
            get
            {
                int total = Products.Sum(item => item.Amount);

                return total;
            }
        }

        public List<Product> Products { get; set; }
        public string ImageSource { get; set; }
    }
}
