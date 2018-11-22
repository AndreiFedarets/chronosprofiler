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
        [DllImport("advapi32.dll")]
        private static extern void Test22();

        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            try
            {
                Empty();
                //Test();
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
        public static void Empty()
        {
            //throw new Exception();
            try
            {
                throw new NullReferenceException("Null-reference");
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                Console.WriteLine("Empty_finally");
            }
            //int i = 0;
            //int j = 0;
            //Random random = new Random(999);
            //i = random.Next();
            //j = random.Next();
            //Console.WriteLine(i);
            //Console.WriteLine(j);
            //Test22();
            //Console.WriteLine("Hello");
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
