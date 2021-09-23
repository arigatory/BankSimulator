using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace BankSimulator.DataAccess
{
    public class DBContext
    {
        private SqlConnection con;
        private DataTable dt;
        private SqlDataAdapter da;
        
        void Setup(DataProviderEnum provider)
        {
            IDbConnection myConnection = GetConnection(provider);
            
            //Console.WriteLine("\n\n\n------------------->" + myConnection?.GetType().Name ?? "unrecognized type");
        }

        private IDbConnection GetConnection(DataProviderEnum dataProvider)
        {
            return dataProvider switch
            {
                DataProviderEnum.SqlServer => new SqlConnection(),
#if PC
                DataProviderEnum.OleDb => new OleDbConnection(),
#endif
                DataProviderEnum.Odbc => new OdbcConnection(),
                _ => null,
            };
        }

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
                    Setup(DataProviderEnum.SqlServer);

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
