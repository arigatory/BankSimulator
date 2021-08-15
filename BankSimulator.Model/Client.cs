using System.Collections.Generic;
using System.Collections.Generic;

namespace BankSimulator.Model
{
    public class Client
    {
        public int Balance { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string ImageSource { get; set; }
    }
}
