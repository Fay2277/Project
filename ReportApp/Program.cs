using System;
using MySql.Data.MySqlClient;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        string connectionString =    
            "server=localhost;database=world;user=worlduser;password=world123;";

        Console.WriteLine("Attempting to connect to MySQL...");

        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected to MySQL!");

                string query = "SELECT ID, Name, CountryCode, Population FROM city LIMIT 10";

                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("ID | Name | CountryCode | Population");
                    Console.WriteLine("------------------------------------------");

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["ID"]} | {reader["Name"]} | {reader["CountryCode"]} | {reader["Population"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
