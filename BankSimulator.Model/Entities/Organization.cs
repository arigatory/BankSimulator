using System;
using System.Collections.Generic;

#nullable disable

namespace BankSimulator.Model.Entities
{
    public partial class Organization
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }
        public bool? IsVip { get; set; }
        public string Title { get; set; }
    }
}
