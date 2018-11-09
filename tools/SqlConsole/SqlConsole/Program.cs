using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SqlConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["Database"];
                using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
                {
                    connection.Open();
                    const string commandText = "SELECT * FROM [dbo].[Table]";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    int count = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count++;
                        }
                    }
                    Console.Write(count);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
