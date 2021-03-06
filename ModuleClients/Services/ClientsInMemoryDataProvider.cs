using BankSimulator.Model;
using Bogus;
using Bogus.DataSets;
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

        public async Task<List<Client>> GetClientsAsync()
        {
            Random random = new Random();

            int numberOfPeople = 20;
            int numberOfVIPPeople = 10;


        var json = await _httpClient.GetStringAsync($"https://randomuser.me/api/?results={numberOfPeople+numberOfVIPPeople}&inc=picture,gender");
            ForJasonConversion.RootObject person = JsonConvert.DeserializeObject<ForJasonConversion.RootObject>(json);

            for (int i = 0; i < numberOfPeople; i++)
            {
                var testUser = new Faker<BankSimulator.Model.Person>("ru")
                .RuleFor(p => p.Sex, f => person.results[i].gender == "male" ? Gender.Male : Gender.Female)
                .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName((Name.Gender?)p.Sex))
                .RuleFor(p => p.LastName, (f, p) => f.Name.LastName((Name.Gender?)p.Sex))
                .RuleFor(p => p.IsVIP, false);
                var client = testUser.Generate(1).FirstOrDefault();
                client.ImageSource = person.results[i].picture.large;
                client.Products.Add(new Product { Name = "Вклад 'Skillbox'", Percent = 7.5, Number = "123", OpenedDate = DateTime.Now, Amount = random.Next(10000, 500000) });
                client.Products.Add(new Product { Name = "Накопительный счёт", Percent = 5.5, Number = "543", OpenedDate = DateTime.Now, Amount = random.Next(10000, 500000) });
                client.Products.Add(new Product { Name = "Кредит", Percent = 10.5, Number = "543", OpenedDate = DateTime.Now, Amount = -random.Next(10000, 1000000) });
                _clients.Add(client);
            }


            for (int i = 0; i < numberOfVIPPeople; i++)
            {
                int j = numberOfPeople + i;
                var testUser = new Faker<BankSimulator.Model.Person>("ru")
                .RuleFor(p => p.Sex, f => person.results[j].gender == "male" ? Gender.Male : Gender.Female)
                .RuleFor(p => p.FirstName, (f, p) => f.Name.FirstName((Name.Gender?)p.Sex))
                .RuleFor(p => p.LastName, (f, p) => f.Name.LastName((Name.Gender?)p.Sex))
                .RuleFor(p => p.IsVIP, true);
                var client = testUser.Generate(1).FirstOrDefault();
                client.ImageSource = person.results[j].picture.large;
                client.Products.Add(new Product { Name = "Премиальный вклад", Percent = 11.5, Number = "123", OpenedDate = DateTime.Now, Amount = random.Next(3000000, 15000000) });
                client.Products.Add(new Product { Name = "Рассчетный счет", Percent = 5.5, Number = "1233", OpenedDate = DateTime.Now, Amount = random.Next(10000, 1000000) });
                client.Products.Add(new Product { Name = "Брокерский счет", Percent = 10.5, Number = "543", OpenedDate = DateTime.Now, Amount = random.Next(5000000, 10000000) });
                _clients.Add(client);
            }


            var company = new Organization
            {
                Title="Yandex", ImageSource= "https://forum.cs-cart.ru/uploads/default/original/2X/c/c9a4cabc0d580fccbafe6c2c6c4a69b3a6e333b5.png"
            };
            company.Products.Add(new Product { Amount =200000000, Name="Счет организации", OpenedDate=DateTime.Now, Percent=3.4, Number= "30232810400000000003" });
            _clients.Add(company);


            company = new Organization
            {
                Title = "Сбер",
                ImageSource = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9b/Sberbank_Logo_2020.svg/440px-Sberbank_Logo_2020.svg.png"
            };
            company.Products.Add(new Product { Amount = 400000000, Name = "Счет организации", OpenedDate = DateTime.Now, Percent = 6.4, Number= "30301810900006004000" });
            _clients.Add(company);

            return _clients.ToList();
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
