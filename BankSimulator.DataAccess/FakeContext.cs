using BankSimulator.Model;
using Bogus;
using System;
using System.Collections.Generic;
using Bogus.DataSets;


namespace BankSimulator.DataAccess
{
    public class FakeContext
    {
        public IEnumerable<Client> Clients;

        public FakeContext()
        {
            var testUsers = new Faker<BankSimulator.Model.Person>("ru")
                //Optional: Call for objects that have complex initialization

                //Basic rules using built-in generators
                .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
                .RuleFor(p => p.Name, (f, p) => f.Name.FullName((Name.Gender?)p.Gender));

            Clients = testUsers.Generate(10);
        }
    }
}
