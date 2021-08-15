using BankSimulator.Model;
using Bogus;
using Bogus.DataSets;
//using ForJasonConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ModuleClients.Services
{
    public class ClientsInMemoryDataProvider : IClientsRepository
    {
        Collection<Client> _clients = new Collection<Client>();
        private static HttpClient _httpClient = new HttpClient();

        public Task DleteCustomerAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Client> GetClientAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            int numberOfPeople = 20;

            var json = await _httpClient.GetStringAsync($"https://randomuser.me/api/?results={numberOfPeople}&inc=picture,gender");
            ForJasonConversion.RootObject person = JsonConvert.DeserializeObject<ForJasonConversion.RootObject>(json);
            //p.ImageSource = person.results[0].picture.medium;

            for (int i = 0; i < numberOfPeople; i++)
            {
                var testUser = new Faker<BankSimulator.Model.Person>("ru")
                .RuleFor(p => p.Sex, f => person.results[i].gender == "male" ? Gender.Male : Gender.Female)
                .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName((Name.Gender?)p.Sex))
                .RuleFor(p => p.LastName, (f, p) => f.Name.LastName((Name.Gender?)p.Sex));
                var client = testUser.Generate(1).FirstOrDefault();
                client.ImageSource = person.results[i].picture.large;
                _clients.Add(client);
            }
            
            return _clients.ToList();
        }

        public Task<Client> UpdateClient(string name)
        {
            throw new NotImplementedException();
        }
    }

    
}

namespace ForJasonConversion
{
    public class PersonAPIResponse
    {
        public string ImageSource { get; set; }
    }

    public class Picture
    {
        public string large { get; set; }
        public string medium { get; set; }
        public string thumbnail { get; set; }
    }

    public class Result
    {
        public Picture picture { get; set; }
        public string gender { get; set; }
    }

    public class Info
    {
        public string seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string version { get; set; }
    }

    public class RootObject
    {
        public List<Result> results { get; set; }
        public Info info { get; set; }
    }
}
