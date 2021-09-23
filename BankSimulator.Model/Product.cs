using System;

namespace BankSimulator.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public double Percent { get; set; }
        public DateTime OpenedDate { get; set; }
        public int Amount { get; set; }
    }
}
