using System;
class input
{
    public int inp1;
    public void read()
    {
        Console.WriteLine("What would you like to search?" + Environment.NewLine +
            "1. Country" + Environment.NewLine +
            "2. Region" + Environment.NewLine +
            "3. Continent" + Environment.NewLine +
            "4. City" + Environment.NewLine +
            "5. Population" + Environment.NewLine +
            "6. Language" + Environment.NewLine +
            "7. Area" + Environment.NewLine );
        inp1 = Convert.ToInt32(Console.ReadLine());
    }
    public void search()
    {
        //insert connection to database and print results
        Console.WriteLine("Thank you.");
    }
}
public class runapp
{
    public static void Main()
    {
        input input = new input();
        input.read();
        input.search();
    }
}