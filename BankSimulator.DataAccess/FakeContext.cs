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
                .RuleFor(p => p.Sex, f => f.PickRandom<Gender>())
                .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName((Name.Gender?)p.Sex))
                .RuleFor(p => p.LastName, (f, p) => f.Name.LastName((Name.Gender?)p.Sex));

            Clients = testUsers.Generate(10);

            foreach (var c in Clients)
            {
                c.ImageSource = @"https://randomuser.me/api/portraits/med/men/75.jpg";
            }
        }
    }
}
