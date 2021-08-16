using System;

namespace BankSimulator.Model
{
    public class Product
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public double Percent { get; set; }
        public DateTime OpenedDate { get; set; }
        public int Amount { get; set; }
    }
}
