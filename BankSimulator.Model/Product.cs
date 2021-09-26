using System;

namespace BankSimulator.Model
{
    public class Product
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public double Percent { get; set; }
        public DateTime OpenedDate { get; set; }
        public long Amount { get; set; }
    }
}
