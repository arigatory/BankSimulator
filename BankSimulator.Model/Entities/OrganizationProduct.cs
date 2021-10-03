using System;
using System.Collections.Generic;

#nullable disable

namespace BankSimulator.Model.Entities
{
    public partial class OrganizationProduct
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public long Amount { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public double ProductPercent { get; set; }
        public DateTime? OpenedDate { get; set; }
    }
}
