using System;
using MySql.Data.MySqlClient;

class WorldReports
{
    // Database connection string
    static string cs = "server=localhost;database=world;uid=worlduser;pwd=world123;";

static void Main()
{
    while (true)
    {
        Console.WriteLine("\n===== WORLD REPORTING SYSTEM =====");
        Console.WriteLine("1 - Countries: World   | 2 - Continent | 3 - Region");
        Console.WriteLine("4 - Cities: World      | 5 - Continent | 6 - Region | 7 - Country | 8 - District");
        Console.WriteLine("9 - Capitals: World    | 10 - Continent | 11 - Region");
        Console.WriteLine("12 - Total Pop: World  | 13 - Continent | 14 - Region | 15 - Country | 16 - District | 17 - City");
        Console.WriteLine("18 - Pop % (Urban/Rural): World | 19 - Continent | 20 - Region | 21 - Country");
        Console.WriteLine("22 - Language Report (Speakers) | 23 - Country Report | 24 - City Report | 25 - Capital Report");
        Console.WriteLine("0 - Exit");
        Console.Write("\nSelect option: ");
        
        string choice = Console.ReadLine();
        if (choice == "0") break;

        int? limit = null;
        // Apply "Top N" logic to all listing reports (1 through 11)
        if (int.TryParse(choice, out int c) && c >= 1 && c <= 11)
        {
            Console.Write("Enter number of results (Leave blank for ALL): ");
            string nInput = Console.ReadLine();
            if (int.TryParse(nInput, out int n)) limit = n;
        }

        switch (choice)
        {
            // --- COUNTRY LISTS ---
            case "1": GetReport("Country", null, null, limit); break;
            case "2": 
                ShowOptions("SELECT DISTINCT Continent FROM country");
                Console.Write("Enter Continent: ");
                GetReport("Country", "Continent = @val", Console.ReadLine(), limit); 
                break;
            case "3": 
                ShowOptions("SELECT DISTINCT Region FROM country");
                Console.Write("Enter Region: ");
                GetReport("Country", "Region = @val", Console.ReadLine(), limit); 
                break;

            // --- CITY LISTS ---
            case "4": GetReport("City", null, null, limit); break;
            case "5": 
                ShowOptions("SELECT DISTINCT Continent FROM country");
                Console.Write("Enter Continent: ");
                GetReport("City", "Continent = @val", Console.ReadLine(), limit); 
                break;
            case "6": 
                ShowOptions("SELECT DISTINCT Region FROM country");
                Console.Write("Enter Region: ");
                GetReport("City", "Region = @val", Console.ReadLine(), limit); 
                break;
            case "7": 
                Console.Write("Enter Country Name: ");
                GetReport("City", "Country = @val", Console.ReadLine(), limit); 
                break;
            case "8": 
                ShowOptions("SELECT DISTINCT District FROM city LIMIT 20");
                Console.Write("Enter District: ");
                GetReport("City", "District = @val", Console.ReadLine(), limit); 
                break;

            // --- CAPITAL LISTS ---
            case "9": GetReport("Capital", null, null, limit); break;
            case "10": 
                ShowOptions("SELECT DISTINCT Continent FROM country");
                Console.Write("Enter Continent: ");
                GetReport("Capital", "Continent = @val", Console.ReadLine(), limit); 
                break;
            case "11": 
                ShowOptions("SELECT DISTINCT Region FROM country");
                Console.Write("Enter Region: ");
                GetReport("Capital", "Region = @val", Console.ReadLine(), limit); 
                break;

            // --- POPULATION TOTALS ---
            case "12": Execute("SELECT SUM(CAST(Population AS SIGNED)) AS 'World Population' FROM country"); break;
            case "13":
                ShowOptions("SELECT DISTINCT Continent FROM country");
                Console.Write("Enter Continent: ");
                Execute("SELECT Continent, SUM(CAST(Population AS SIGNED)) AS Population FROM country WHERE Continent = @val GROUP BY Continent", Console.ReadLine());
                break;
            case "14":
                ShowOptions("SELECT DISTINCT Region FROM country");
                Console.Write("Enter Region: ");
                Execute("SELECT Region, SUM(CAST(Population AS SIGNED)) AS Population FROM country WHERE Region = @val GROUP BY Region", Console.ReadLine());
                break;
            case "15": 
                Console.Write("Enter Country Name: ");
                Execute("SELECT Name, Population FROM country WHERE Name = @val", Console.ReadLine()); 
                break;
            case "16":
                Console.Write("Enter District Name: ");
                Execute("SELECT District, SUM(Population) AS Population FROM city WHERE District = @val GROUP BY District", Console.ReadLine());
                break;
            case "17":
                Console.Write("Enter City Name: ");
                Execute("SELECT Name, Population FROM city WHERE Name = @val", Console.ReadLine());
                break;

            // --- URBAN / RURAL % REPORTS ---
            case "18": GetReport("PopPct", null, null); break; // World
            case "19": 
                ShowOptions("SELECT DISTINCT Continent FROM country");
                Console.Write("Enter Continent: ");
                GetReport("PopPct", "Continent = @val", Console.ReadLine()); 
                break;
            case "20":
                ShowOptions("SELECT DISTINCT Region FROM country");
                Console.Write("Enter Region: ");
                GetReport("PopPct", "Region = @val", Console.ReadLine());
                break;
            case "21":
                Console.Write("Enter Country Name: ");
                GetReport("PopPct", "Name = @val", Console.ReadLine());
                break;

            // --- SPECIAL REPORTS ---
            case "22":
                Console.Write("Enter Language (e.g. Spanish): ");
                GetReport("Language", "Language = @val", Console.ReadLine());
                break;
            case "23": GetReport("Country", null, null); break;
            case "24": GetReport("City", null, null); break;
            case "25": GetReport("Capital", null, null); break;

            default: Console.WriteLine("Invalid selection."); break;
        }
    }
}


    static void GetReport(string type, string condition, string value, int? limit = null)
    {
        string sql = "";
        switch (type)
        {
            case "Country":
                sql = "SELECT country.Code, country.Name, country.Continent, country.Region, country.Population, city.Name AS Capital FROM country LEFT JOIN city ON country.Capital = city.ID";
                break;
            case "City":
                sql = "SELECT city.Name, country.Name AS Country, city.District, city.Population FROM city JOIN country ON city.CountryCode = country.Code";
                break;
            case "Capital":
                sql = "SELECT city.Name, country.Name AS Country, city.Population FROM city JOIN country ON city.ID = country.Capital";
                break;
            case "PopPct":
                sql = @"SELECT country.Name, country.Continent, country.Region, country.Population AS TotalPop, 
                        COALESCE(city_sums.InCities, 0) AS UrbanPop, 
                        CONCAT(ROUND((COALESCE(city_sums.InCities, 0) / country.Population) * 100, 2), '%') AS 'Urban%', 
                        (country.Population - COALESCE(city_sums.InCities, 0)) AS RuralPop,
                        CONCAT(ROUND(((country.Population - COALESCE(city_sums.InCities, 0)) / country.Population) * 100, 2), '%') AS 'Rural%'
                        FROM country 
                        LEFT JOIN (SELECT CountryCode, SUM(Population) AS InCities FROM city GROUP BY CountryCode) city_sums 
                        ON country.Code = city_sums.CountryCode";
                break;
            case "Language":
                sql = @"SELECT country.Name AS Country, countrylanguage.Language, 
                        CONCAT(countrylanguage.Percentage, '%') AS 'Percent',
                        FLOOR((countrylanguage.Percentage / 100) * country.Population) AS TotalSpeakers
                        FROM countrylanguage JOIN country ON countrylanguage.CountryCode = country.Code";
                break;
        }

        // Wrap the base query to allow filtering by continent, region, or language
        if (!string.IsNullOrEmpty(condition)) sql = $"SELECT * FROM ({sql}) AS base WHERE {condition}";
        
        sql += " ORDER BY " + (type == "Language" ? "TotalSpeakers" : "Population") + " DESC";
        if (limit.HasValue) sql += $" LIMIT {limit.Value}";

        Execute(sql, value);
    }

    static void Execute(string query, string val = null)
    {
        try 
        {
            using (var con = new MySqlConnection(cs))
            {
                con.Open();
                using (var cmd = new MySqlCommand(query, con))
                {
                    if (val != null) cmd.Parameters.AddWithValue("@val", val);
                    using (var r = cmd.ExecuteReader())
                    {
                        for (int i = 0; i < r.FieldCount; i++) Console.Write($"{r.GetName(i),-18}");
                        Console.WriteLine("\n" + new string('-', r.FieldCount * 18));
                        while (r.Read()) {
                            for (int i = 0; i < r.FieldCount; i++) Console.Write($"{r[i],-18}");
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
    }

    static void ShowOptions(string query)
    {
        using (var con = new MySqlConnection(cs)) {
            con.Open();
            using (var cmd = new MySqlCommand(query, con))
            using (var r = cmd.ExecuteReader()) {
                Console.WriteLine("\nAvailable Options:");
                while (r.Read()) Console.WriteLine("- " + r[0]);
                Console.WriteLine();
            }
        }
    }
}
