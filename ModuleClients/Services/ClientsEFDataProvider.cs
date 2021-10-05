using BankSimulator.DataAccess;
using BankSimulator.DataAccess.EfStructures;
using BankSimulator.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleClients.Services
{

    public class ClientsEFDataProvider : IClientsRepository
    {
        public async Task<List<Client>> GetClientsAsync()
        {
            var clients = new List<Client>();
            using (var ctx = new ApplicationDbContext())
            {
                var persons = await ctx.Persons.ToListAsync();
                var products = await ctx.Products.ToListAsync();
                var organizations = await ctx.Organizations.ToListAsync();
                var organizationProducts = await ctx.OrganizationProducts.ToListAsync();



                foreach (var pers in persons)
                {
                    var person = new Person
                    {
                        Id = pers.Id,
                        FirstName = pers.FirstName,
                        ImageSource = pers.ImageSource,
                        IsVIP = pers.IsVip,
                        LastName = pers.LastName,
                        MiddleName = pers.MiddleName
                    };
                    person.Products = new List<Product>();
                    var persProducts = products.Where(p => p.ClientId == person.Id).ToList();
                    foreach (var p in persProducts)
                    {
                        person.Products.Add(new Product
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            ClientId = p.ClientId,
                            Name = p.Name,
                            Number = p.Number,
                            Percent = p.ProductPercent
                        });
                    }
                    clients.Add(person);
                }


                foreach (var org in organizations)
                {
                    var organization = new Organization { Id = org.Id, ImageSource = org.ImageSource,Title=org.Title};

                    var orgProducts = products.Where(p => p.ClientId == org.Id).ToList();
                    foreach (var p in orgProducts)
                    {
                        organization.Products.Add(new Product
                        {
                            Id = p.Id,
                            Amount = p.Amount,
                            ClientId = p.ClientId,
                            Name = p.Name,
                            Number = p.Number,
                            Percent = p.ProductPercent
                        });
                    }
                    clients.Add(organization);
                }

                
            }
            return clients;
        }
    }
}
