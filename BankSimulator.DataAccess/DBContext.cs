using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BankSimulator.DataAccess
{
    public class DBContext
    {
        private SqlConnection con;
        private DataTable dt;
        private SqlDataAdapter da;

        public void Connect()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "BankSimulatorDB"
            };


            try
            {
                using (SqlConnection c = new SqlConnection(connectionStringBuilder.ConnectionString))
                {
                    c.Open();
                    dt = new DataTable();
                    da = new SqlDataAdapter();
                    var sql = @"SELECT * FROM Persons Order By Persons.Id";
                    SqlCommand command = new SqlCommand(sql, con);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
