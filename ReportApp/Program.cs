using System;
using MySql.Data.MySqlClient;
// import MySQL library so C# can talk to the database

// main class for the program
class WorldReports
{
    // connection string used to connect to the MySQL database
    static string cs = "server=localhost;database=world;uid=worlduser;pwd=world123;";

    static void Main()
    {
        // infinite loop so the menu keeps showing until user exits
        while (true)
        {
            Console.WriteLine("\n===== WORLD REPORTING SYSTEM =====");

            // menu options the user can choose
            Console.WriteLine("1  - All Countries in World");
            Console.WriteLine("2  - Countries in Continent");
            Console.WriteLine("3  - Countries in Region");

            Console.WriteLine("4  - All Cities in World");
            Console.WriteLine("5  - Cities in Continent");
            Console.WriteLine("6  - Cities in Region");
            Console.WriteLine("7  - Cities in Country");
            Console.WriteLine("8  - Cities in District");

            Console.WriteLine("9  - All Capital Cities in World");
            Console.WriteLine("10 - Capital Cities in Continent");
            Console.WriteLine("11 - Capital Cities in Region");

            Console.WriteLine("12 - Population of World");
            Console.WriteLine("13 - Population of Continent");
            Console.WriteLine("14 - Population of Region");
            Console.WriteLine("15 - Population of Country");

            Console.WriteLine("16 - Population of District");
            Console.WriteLine("17 - Population of City");
            Console.WriteLine("18 - Population of Citizens not living in Cities");

            Console.WriteLine("19 - Country Report");
            Console.WriteLine("20 - City Report");

            Console.WriteLine("0  - Exit");

            // ask the user to select an option
            Console.Write("Select option: ");
            string? choice = Console.ReadLine();

            // switch statement decides what query to run based on user choice
            switch (choice)
            {
                case "1":
                    // show all countries in the world with their capital
                    Execute(@"use world;SELECT country.Code,
                 country.Name,
                 country.Continent,
                 country.Region,
                 country.Population,
                 city.Name AS Capital
          FROM country
          LEFT JOIN city ON country.Capital = city.ID

          UNION

          SELECT country.Code,
                 country.Name,
                 country.Continent,
                 country.Region,
                 country.Population,
                 city.Name AS Capital
          FROM country
          RIGHT JOIN city ON country.Capital = city.ID
          WHERE country.Code IS NOT NULL

          ORDER BY Population DESC");
                    break;

                case "2":
                    // show available continents first
                    ShowOptions("SELECT DISTINCT Continent FROM country");

                    // ask user to enter continent
                    Console.Write("Enter Continent: ");
                    string? continent = Console.ReadLine();

                    // query countries in the selected continent
                    Execute(@"use world; SELECT country.Code,
                                     country.Name,
                                     country.Continent,
                                     country.Region,
                                     country.Population,
                                     city.Name AS Capital
                              FROM country
                              LEFT JOIN city ON country.Capital = city.ID
                              WHERE country.Continent = @value
                              ORDER BY country.Population DESC", continent);
                    break;

                case "3":
                    // show available regions
                    ShowOptions("SELECT DISTINCT Region FROM country");

                    Console.Write("Enter Region: ");
                    string? region = Console.ReadLine();

                    // query countries in the selected region
                    Execute(@"use world; SELECT country.Code,
                                     country.Name,
                                     country.Continent,
                                     country.Region,
                                     country.Population,
                                     city.Name AS Capital
                              FROM country
                              LEFT JOIN city ON country.Capital = city.ID
                              WHERE country.Region = @value
                              ORDER BY country.Population DESC", region);
                    break;

                case "4":
                    // show all cities in the world
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              ORDER BY city.Population DESC");
                    break;

                case "5":
                    // show continent options
                    ShowOptions("SELECT DISTINCT Continent FROM country");

                    Console.Write("Enter Continent: ");
                    continent = Console.ReadLine();

                    // show cities in the chosen continent
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Continent = @value
                              ORDER BY city.Population DESC", continent);
                    break;

                case "6":
                    // show region options
                    ShowOptions("SELECT DISTINCT Region FROM country");

                    Console.Write("Enter Region: ");
                    region = Console.ReadLine();

                    // show cities in the chosen region
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Region = @value
                              ORDER BY city.Population DESC", region);
                    break;

                case "7":
                    // ask user to type a country code
                    Console.Write("Enter Country Code: ");
                    string? code = Console.ReadLine();

                    // show cities in that country
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Code = @value
                              ORDER BY city.Population DESC", code);
                    break;

                case "8":
                    // show district options
                    ShowOptions("SELECT DISTINCT District FROM city");

                    Console.Write("Enter District: ");
                    string? district = Console.ReadLine();

                    // show cities in selected district
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE city.District = @value
                              ORDER BY city.Population DESC", district);
                    break;

                case "9":
                    // show all capital cities in the world
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              ORDER BY city.Population DESC");
                    break;

                case "10":
                    // show continent options
                    ShowOptions("SELECT DISTINCT Continent FROM country");

                    Console.Write("Enter Continent: ");
                    continent = Console.ReadLine();

                    // show capital cities in the continent
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              WHERE country.Continent = @value
                              ORDER BY city.Population DESC", continent);
                    break;

                case "11":
                    // show region options
                    ShowOptions("SELECT DISTINCT Region FROM country");

                    Console.Write("Enter Region: ");
                    region = Console.ReadLine();

                    // show capital cities in the region
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              WHERE country.Region = @value
                              ORDER BY city.Population DESC", region);
                    break;

                case "12":
                    // show total population of the world
                    Execute(@"use world; SELECT SUM(Population) AS WorldPopulation FROM country");
                    break;

                case "13":
                    // choose continent
                    ShowOptions("SELECT DISTINCT Continent FROM country");

                    Console.Write("Enter Continent: ");
                    continent = Console.ReadLine();

                    // show population of that continent
                    Execute(@"use world; SELECT Continent, SUM(Population)
                              FROM country
                              WHERE Continent = @value
                              GROUP BY Continent", continent);
                    break;

                case "14":
                    // choose region
                    ShowOptions("SELECT DISTINCT Region FROM country");

                    Console.Write("Enter Region: ");
                    region = Console.ReadLine();

                    // show population of region
                    Execute(@"use world; SELECT Region, SUM(Population)
                              FROM country
                              WHERE Region = @value
                              GROUP BY Region", region);
                    break;

                case "15":
                    // choose country
                    ShowOptions("SELECT Name FROM country");

                    Console.Write("Enter Country: ");
                    string? country = Console.ReadLine();

                    // show population of that country
                    Execute(@"use world; SELECT Name, Population
                              FROM country
                              WHERE Name = @value", country);
                    break;

                case "16":
                    // choose district
                    ShowOptions("SELECT DISTINCT District FROM city");

                    Console.Write("Enter District: ");
                    district = Console.ReadLine();

                    // show population of district
                    Execute(@"use world; SELECT District, SUM(Population)
                              FROM city
                              WHERE District = @value
                              GROUP BY District", district);
                    break;

                case "17":
                    // choose city
                    ShowOptions("SELECT Name FROM city");

                    Console.Write("Enter City: ");
                    string? city = Console.ReadLine();

                    // show population of city
                    Execute(@"use world; SELECT Name, Population
                              FROM city
                              WHERE Name = @value", city);
                    break;

                case "18":
                    // show population not living in cities for each country
                    Execute(@"use world; SELECT 
                                     country.Name,
                                     country.Population AS TotalPopulation,
                                     SUM(city.Population) AS CityPopulation,
                                     country.Population - SUM(city.Population) AS NotInCities
                              FROM country
                              LEFT JOIN city ON country.Code = city.CountryCode
                              GROUP BY country.Name");
                    break;

                case "19":
                    // full country report
                    Execute(@"use world; SELECT 
                                     country.Code,
                                     country.Name,
                                     country.Continent,
                                     country.Region,
                                     country.Population,
                                     city.Name AS Capital
                              FROM country
                              LEFT JOIN city ON country.Capital = city.ID
                              ORDER BY country.Population DESC");
                    break;

                case "20":
                    // full city report
                    Execute(@"use world; SELECT 
                                     city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              ORDER BY city.Population DESC");
                    break;

                case "0":
                    // exit program
                    return;

                default:
                    // if user types wrong option
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    // method that runs a query and prints the results
    static void Execute(string query, string? value = null)
    {
        // create connection to database
        MySqlConnection con = new MySqlConnection(cs);
        con.Open();

        // create command using query
        MySqlCommand cmd = new MySqlCommand(query, con);

        // if query needs a parameter add it
        if (value != null)
        {
            cmd.Parameters.AddWithValue("@value", value);
        }

        // run the query and get results
        MySqlDataReader reader = cmd.ExecuteReader();

        bool found = false;
        int width = 25; // width used for formatting output

        // print column headers
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string header = reader.GetName(i);

            if (header.Length > width)
                header = header.Substring(0, width);

            Console.Write(header.PadRight(width));
        }

        Console.WriteLine();

        // line under headers
        Console.WriteLine(new string('-', reader.FieldCount * width));

        // read each row from database
        while (reader.Read())
        {
            found = true;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string valueText = reader[i].ToString() ?? "";

                if (valueText.Length > width)
                    valueText = valueText.Substring(0, width);

                Console.Write(valueText.PadRight(width));
            }

            Console.WriteLine();
        }

        // if no data was returned
        if (!found)
        {
            Console.WriteLine("No data found. Please check your input.");
        }

        // close database connection
        con.Close();
    }

    // method that shows possible options to the user
    static void ShowOptions(string query)
    {
        // connect to database
        MySqlConnection con = new MySqlConnection(cs);
        con.Open();

        // run the query
        MySqlCommand cmd = new MySqlCommand(query, con);
        MySqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\nAvailable options:");

        // print each option
        while (reader.Read())
        {
            Console.WriteLine("- " + reader[0]);
        }

        // close connection
        con.Close();
    }
}