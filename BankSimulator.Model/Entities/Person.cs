using System;
using System.Collections.Generic;

#nullable disable

namespace BankSimulator.Model.Entities
{
    public partial class Person
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }
        public bool IsVip { get; set; }
        public bool IsMale { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
