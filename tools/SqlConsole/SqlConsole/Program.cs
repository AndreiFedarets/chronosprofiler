using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SqlConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            try
            {
               //Empty();
                Test();
            }
            catch (Exception e)
            {
                Console.WriteLine("Main_catch:" + e);
            }
            Console.ReadKey();
        }


        public static void SwitchTest()
        {
            int i = 3;
            switch (i)
            {
                case 1:
                    i += 1;
                    break;
                case 2:
                    i += 2;
                    break;
                case 3:
                    i += 3;
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Exception Empty()
        {
            //try
            //{
            //    Test22();
            int i = 0;
                try
                {
                    throw new NullReferenceException("Null-reference");
                }
                catch (Exception e)
                {
                    i = 5;
                    Console.WriteLine(e);
                }
                finally
                {
                    i = 10;
                    Console.WriteLine("Empty_finally");
                }
            //}
            //finally
            //{
            //    Test22();   
            //}
            return new Exception();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void Test()
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["Database"];
            using (SqlConnection connection = new SqlConnection(connectionString.ConnectionString))
            {
                connection.Open();
                const string commandText = "SELECT * FROM [dbo].[Table]";
                SqlCommand command = new SqlCommand(commandText, connection);
                Console.WriteLine(command.ToString());
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
    }
}
