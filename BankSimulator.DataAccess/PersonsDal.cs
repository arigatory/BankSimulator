using BankSimulator.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
#if PC
using System.Data.OleDb;
#endif
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;




namespace BankSimulator.DataAccess
{
    public class PersonsDal
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;

        public PersonsDal()
        {
            var (provider, connectionString) = GetProviderFromConfiguration();
            _connectionString = connectionString;
        }
        public PersonsDal(string connectionString)
            => _connectionString = connectionString;


        public async Task<List<Person>> GetAllPersons()
        {
            OpenConnection();
            // This will hold the records.
            List<Person> persons = new List<Person>();
            // Prep command object.
            string sql = @"SELECT p.Id, p.ImageSource, p.IsVIP, p.IsMale, p.FirstName, p.LastName FROM Persons p";
            using SqlCommand command = new SqlCommand(sql, _sqlConnection)
            {
                CommandType = CommandType.Text
            };
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                var person = new Person
                {
                    Id = (int)dataReader["Id"],
                    ImageSource = (string)dataReader["ImageSource"],
                    IsVIP = (bool)dataReader["IsVIP"],
                    FirstName = (string)dataReader["FirstName"],
                    LastName = (string)dataReader["LastName"]
                };
                bool male = (bool)dataReader["IsMale"];
                if (male)
                {
                    person.Sex = Gender.Male;
                }
                else
                {
                    person.Sex = Gender.Female;
                }
                person.Products = await GetAllProductsForClient(person);
                persons.Add(person);
            }
            dataReader.Close();
            return persons;
        }


        public async Task<List<Product>> GetAllProductsForClient(Client client)
        {
            string tableName = "";
            if (client is Person person)
            {
                tableName = "Products";
            }
            else if (client is Organization organization)
            {
                tableName = "OrganizationProducts";
            }
            OpenConnection();
            List<Product> products = new List<Product>();
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@ClientId",
                Value = client.Id,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input
            };
            string sql = $"SELECT p.Amount, p.Name, p.Number, p.ProductPercent, p.OpenedDate " +
                         $"FROM {tableName} p " +
                         $"WHERE p.ClientId = @ClientId";


            using SqlCommand command = new SqlCommand(sql, _sqlConnection)
            {
                CommandType = CommandType.Text
            };
            command.Parameters.Add(param);
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                products.Add(new Product
                {
                    ClientId = client.Id,
                    Name = (string)dataReader["Name"],
                    Number = (string)dataReader["Number"],
                    Percent = (double)dataReader["ProductPercent"],
                    OpenedDate = (DateTime)dataReader["OpenedDate"],
                    Amount = (long)dataReader["Amount"]
                });
            }
            dataReader.Close();
            return products;
        }


        public async Task<List<Organization>> GetAllOrganizations()
        {
            OpenConnection();
            List<Organization> organizations = new List<Organization>();
            string sql = @"SELECT o.Id, o.ImageSource, o.Title FROM Organizations o";
            using SqlCommand command = new SqlCommand(sql, _sqlConnection)
            {
                CommandType = CommandType.Text
            };
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                var organization = new Organization
                {
                    Id = (int)dataReader["Id"],
                    ImageSource = (string)dataReader["ImageSource"],
                    Title = (string)dataReader["Title"],
                };

                organization.Products = await GetAllProductsForClient(organization);
                organizations.Add(organization);
            }
            dataReader.Close();
            return organizations;
        }

        public void InsertClient(Client client)
        {
            OpenConnection();
            string sql = "";
            string productsTableName = "";
            if (client is Person person)
            {
                sql = "Insert Into Persons" +
                "(ImageSource, IsVIP, IsMale, FirstName, LastName) " +
                "output INSERTED.ID " +
                "Values (@ImageSource, @IsVIP, @IsMale, @FirstName, @LastName)";
                productsTableName = "Products";
            }
            else if (client is Organization organization)
            {
                sql = "Insert Into Organizations" +
                "(ImageSource, Title) " +
                "output INSERTED.ID " +
                "Values (@ImageSource, @Title)";
                productsTableName = "OrganizationProducts";
            }

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@ImageSource",
                    Value = client.ImageSource,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);


                if (client is Person p)
                {
                    parameter = new SqlParameter
                    {
                        ParameterName = "@IsVIP",
                        Value = p.IsVIP,
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter
                    {
                        ParameterName = "@IsMale",
                        Value = p.Sex == Gender.Male,
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter
                    {
                        ParameterName = "@FirstName",
                        Value = p.FirstName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter
                    {
                        ParameterName = "@Lastname",
                        Value = p.LastName,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                }
                else if (client is Organization o)
                {
                    parameter = new SqlParameter
                    {
                        ParameterName = "@Title",
                        Value = o.Title,
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                }
                int clientId = (int)command.ExecuteScalar();

                foreach (var prod in client.Products)
                {
                    InsertProductForClient(prod, clientId, productsTableName);
                }
                CloseConnection();
            }
        }

        public void InsertProductForClient(Product product, int clientId, string clientTable)
        {
            OpenConnection();
            string sql = $"Insert Into {clientTable}" +
                "(ClientId, Amount, Name, Number, ProductPercent, OpenedDate) " +
                "Values (@ClientId, @Amount, @Name, @Number, @ProductPercent, @OpenedDate)";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@ClientId",
                    Value = clientId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Amount",
                    Value = product.Amount,
                    SqlDbType = SqlDbType.BigInt,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter); parameter = new SqlParameter
                {
                    ParameterName = "@Name",
                    Value = product.Name,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Number",
                    Value = product.Number,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@ProductPercent",
                    Value = product.Percent,
                    SqlDbType = SqlDbType.Float,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@OpenedDate",
                    Value = product.OpenedDate,
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        private void OpenConnection()
        {
            _sqlConnection = new SqlConnection
            {
                ConnectionString = _connectionString
            };
            _sqlConnection.Open();
        }

        private void CloseConnection()
        {
            if (_sqlConnection?.State != ConnectionState.Closed)
            {
                _sqlConnection?.Close();
            }
        }

        bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _sqlConnection.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        static (DataProviderEnum Provider, string ConnectionString) GetProviderFromConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", true, true).Build();
            var providerName = config["ProviderName"]; 
            if (Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
            {
                return (provider, config[$"{providerName}:ConnectionString"]);
            };
            throw new Exception("Invalid data provider value supplied.");
        }

    }
}
