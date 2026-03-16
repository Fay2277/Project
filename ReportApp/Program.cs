using System;
using MySql.Data.MySqlClient;
// use sql db in file
class WorldReports
{
    static string cs = "server=localhost;database=world;uid=worlduser;pwd=world123;"; //log in to sql to use db

    static void Main()
    {
        while (true) //loop to keep app running
        {
            Console.WriteLine("\n===== WORLD REPORTING SYSTEM =====");

            Console.WriteLine("1  - All Countries in World");
            Console.WriteLine("2  - Countries in Continent");
            Console.WriteLine("3  - Countries in Region");
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("4  - All Cities in World");
            Console.WriteLine("5  - Cities in Continent");
            Console.WriteLine("6  - Cities in Region");
            Console.WriteLine("7  - Cities in Country");
            Console.WriteLine("8  - Cities in District");
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("9  - All Capital Cities in World");
            Console.WriteLine("10 - Capital Cities in Continent");
            Console.WriteLine("11 - Capital Cities in Region");
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("12 - Population of World");
            Console.WriteLine("13 - Population of Continent");
            Console.WriteLine("14 - Population of Region");
            Console.WriteLine("15 - Population of Country");

            Console.WriteLine("16 - Population of District");
            Console.WriteLine("17 - Population of City");
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("18 - Population of Citizens not living in Cities in the World"); //in country region and continent, use a % for both in and out of cities
            Console.WriteLine("---------------------------------------------------");

            Console.WriteLine("22 - Country Report"); //code name continent population region capital
            Console.WriteLine("23 - City Report"); //name country district population
            Console.WriteLine("24 - Capital City Report");//name country population
            Console.WriteLine("---------------------------------------------------");
            //Top N populated, read the user input for number and limit the results to the number the user gave
            //example of select statement with limit: SELECT column FROM table LIMIT row_count;
            //add options for language 

            Console.WriteLine("0  - Exit");

            Console.Write("Select option: ");
            string choice = Console.ReadLine();

            switch (choice) //decision statement to get search db and select statements
            {
                case "1":
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

          ORDER BY Population DESC"); //selects all countries from world
                    break;

                case "2":
                    ShowOptions("SELECT DISTINCT Continent FROM country");
                    Console.Write("Enter Continent: ");
                    string continent = Console.ReadLine();
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
                    break; //selects all countries from continent

                case "3":
                    ShowOptions("SELECT DISTINCT Region FROM country");
                    Console.Write("Enter Region: ");
                    string region = Console.ReadLine();
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
                    break; //select all countries from region

                case "4":
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              ORDER BY city.Population DESC");
                    break; //select all cities in world

                case "5":
                    ShowOptions("SELECT DISTINCT Continent FROM country");
                    Console.Write("Enter Continent: ");
                    continent = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Continent = @value
                              ORDER BY city.Population DESC", continent);
                    break; //select all cities in continent

                case "6":
                    ShowOptions("SELECT DISTINCT Region FROM country");
                    Console.Write("Enter Region: ");
                    region = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Region = @value
                              ORDER BY city.Population DESC", region);
                    break; //select all cities in region

                case "7":
                    Console.Write("Enter Country Code: ");
                    string code = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE country.Code = @value
                              ORDER BY city.Population DESC", code);
                    break; //select all cities in country

                case "8":
                    ShowOptions("SELECT DISTINCT District FROM city");
                    Console.Write("Enter District: ");
                    string district = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.District,
                                     city.Population
                              FROM city
                              JOIN country ON city.CountryCode = country.Code
                              WHERE city.District = @value
                              ORDER BY city.Population DESC", district);
                    break; //select all cities in district

                case "9":
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              ORDER BY city.Population DESC");
                    break; //select all capital cities

                case "10":
                    ShowOptions("SELECT DISTINCT Continent FROM country");
                    Console.Write("Enter Continent: ");
                    continent = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              WHERE country.Continent = @value
                              ORDER BY city.Population DESC", continent);
                    break; //select all capital cities in continent

                case "11":
                    ShowOptions("SELECT DISTINCT Region FROM country");
                    Console.Write("Enter Region: ");
                    region = Console.ReadLine();
                    Execute(@"use world; SELECT city.Name,
                                     country.Name AS Country,
                                     city.Population
                              FROM city
                              JOIN country ON city.ID = country.Capital
                              WHERE country.Region = @value
                              ORDER BY city.Population DESC", region);
                    break; //select all capital cities in region
                case "12":
                    Execute(@"use world; SELECT 
                       //select population of world
                    break;   
                case "13":
                       //select population of continent
                    break;        
                case "14":
                       //select population of region
                    break;        
                case "15":
                       //select population of country
                    break;        
                case "16":
                       //select population of district
                    break;        
                case "17":
                       //select population of city
                    break;
                            
                case "18":
                    //population percentage of living in city and not in city
                    break;        
                case "19":
                    //population percentage for world   
                    break;        
                case "20":
                    //population percentage for country
                    break;        
                case "21":
                    //population percentage for region 
                    break;        
                case "22":
                    //population percentage for continent        
                    break;

                case "23":
                    //23-25 country, city and capital city reports
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static void Execute(string query, string value = null) //executes the select statement that has been chosen
    {
        MySqlConnection con = new MySqlConnection(cs);
        con.Open(); //new sql object to open the connection

        MySqlCommand cmd = new MySqlCommand(query, con);

        if (value != null)
        {
            cmd.Parameters.AddWithValue("@value", value);
        }

        MySqlDataReader reader = cmd.ExecuteReader();

        bool found = false;

        while (reader.Read())
        {
            found = true;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write(reader[i] + " | ");
            }

            Console.WriteLine();
        }

        if (!found) //if data is unavailable
        {
            Console.WriteLine("No data found. Please check your input.");
        }

        con.Close();
    }

    static void ShowOptions(string query) //to show options the user can choose from
    {
        MySqlConnection con = new MySqlConnection(cs);
        con.Open();

        MySqlCommand cmd = new MySqlCommand(query, con);
        MySqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("\nAvailable options:");

        while (reader.Read())
        {
            Console.WriteLine("- " + reader[0]);
        }

        con.Close();
    }
}
