using BankSimulator.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace BankSimulator.DataAccess
{
    public class PersonsDal
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;

        public PersonsDal() : this("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=BankSimulatorDB")
        {
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
                person.Products = await GetAllProductsForPerson(person.Id);
                persons.Add(person);
            }
            dataReader.Close();
            return persons;
        }


        public async Task<List<Product>> GetAllProductsForPerson(int personId)
        {
            OpenConnection();
            List<Product> products = new List<Product>();
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@PersonId",
                Value = personId,
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Input
            };
            string sql = @"SELECT p.Amount, p.Name, p.Number, p.ProductPercent, p.OpenedDate
                           FROM Products p 
                           WHERE p.PersonId = @PersonId";
            using SqlCommand command = new SqlCommand(sql, _sqlConnection)
            {
                CommandType = CommandType.Text
            };
            command.Parameters.Add(param);
            SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (dataReader.Read())
            {
                products.Add(new Product{
                    PersonId = personId,
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

        public void InsertPerson(Person person)
        {
            OpenConnection();
            // Note the "placeholders" in the SQL query.
            string sql = "Insert Into Persons" +
                "(ImageSource, IsVIP, IsMale, FirstName, LastName) " +
                "output INSERTED.ID " +
                "Values (@ImageSource, @IsVIP, @IsMale, @FirstName, @LastName)";
            // This command will have internal parameters.
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                // Fill params collection.
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@ImageSource",
                    Value = person.ImageSource,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 200,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter); 
                parameter = new SqlParameter
                {
                    ParameterName = "@IsVIP",
                    Value = person.IsVIP,
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter); parameter = new SqlParameter
                {
                    ParameterName = "@IsMale",
                    Value = person.Sex == Gender.Male,
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@FirstName",
                    Value = person.FirstName,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                parameter = new SqlParameter
                {
                    ParameterName = "@Lastname",
                    Value = person.LastName,
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);
                int personId = (int)command.ExecuteScalar();
                foreach (var prod in person.Products)
                {
                    InsertProduct(prod, personId);
                }
                CloseConnection();
            }
        }

        public void InsertProduct(Product product, int personId)
        {
            OpenConnection();
            string sql = "Insert Into Products" +
                "(PersonId, Amount, Name, Number, ProductPercent, OpenedDate) " +
                "Values (@PersonId, @Amount, @Name, @Number, @ProductPercent, @OpenedDate)";
            
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@PersonId",
                    Value = personId,
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

    }
}
