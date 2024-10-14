// See https://aka.ms/new-console-template for more information


using System.Globalization;
using Microsoft.Data.Sqlite;
public class Program
{

    static string connectionString = @"Data Source=habit-Tracker.db";

    private static void Main(string[] args)
    {
        string connectionString = @"Data Source=habit-Tracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS drinking_water (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TEXT,
            Quantity INTEGER
            )";


            tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        GetUserInput();

    }

    static void GetUserInput()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("Press 0 to close the App");
            Console.WriteLine("Type 1 to view all Records");
            Console.WriteLine("Type 2 to Insert");
            Console.WriteLine("Type 3 to Delete");
            Console.WriteLine("Type 4 to Update");
            Console.WriteLine(":\n");

            string userInput = Console.ReadLine();


            switch (userInput)
            {
                case "0":
                    Console.WriteLine("closing...");
                    closeApp = true;
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                    // case 4:
                    //     Update();
                    //     break;
                     default:
                        Console.WriteLine("\nINVALID INPUT\n");
                        break;
            }
        }
    }

    static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM drinking_water ";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        }); ;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("------------------------------------------\n");
            }
        }

    static void Insert()
    {

        string date = GetDateInput();
        int quantity = GetNumberInput("\n\n INSERT NUMBER OF GLASSES YOU DRANK TODAY (NO DECIMALS)");


        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO drinking_water(date, quantity) VALUES('{date}',{quantity} )";
            tableCmd.ExecuteNonQuery();

            connection.Close();

        }

    }
    static void Delete()
    {
        Console.Clear();
        GetAllRecords();

        var recordId = GetNumberInput("TYPE the ID of record u wanna delete");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from drinking_water Where ID = '{recordId}' ";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                System.Console.WriteLine($"\n Record with ID OF {recordId} doesn't exist");
                Delete();
            }

        }
    }

    static string GetDateInput()
    {
        Console.WriteLine("INSERT DATE: (dd--mm-yy)" + "or 0 to return to main menu");

        string dateInput = Console.ReadLine();
        return dateInput;

        if (dateInput == "0")
        {
            GetUserInput();
        }

        return dateInput;
    }

    static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") GetUserInput();

        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;

    }
}
public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}