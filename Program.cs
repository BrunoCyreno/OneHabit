using Microsoft.Data.Sqlite;
using System.Globalization;

namespace OneHabit
{
    internal class Program
    {
        private static string connectionString = "Data Source=OneHabit.db";

        private static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS drinking_water (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    Quantity INTERGER
                    )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            GetUserInput();
        }

        private static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("*****************************************************************\n");
                Console.WriteLine("\n\n MAIN MENU");
                Console.WriteLine("*****************************************************************\n");
                Console.WriteLine("\n What would you like to do?");
                Console.WriteLine("\n[0] Close app");
                Console.WriteLine("[1] View all records");
                Console.WriteLine("[2] Insert Record");
                Console.WriteLine("[3] Delete Record");
                Console.WriteLine("[4] Update Record");
                Console.WriteLine("*****************************************************************\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nExit application? [y/n]\n");

                        var QuitOption = Console.ReadLine();

                        if (QuitOption.Trim().Equals("y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine("Goodbye!");
                            closeApp = true;
                            Environment.Exit(0);
                        }
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

                    /*case 4:
                        Update();
                        break;*/

                    default:
                        Console.WriteLine($"\n ERROR: Please choose a valid option...\n");
                        break;
                }
            }
        }

        private static void Insert()
        {
            string date = GetDateInput();
            int quantity = GetNumberInput("\n\n Water Drunk: \n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM drinking_water";

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
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();

                Console.WriteLine("*****************************************************************\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
                }
                Console.WriteLine("press any key to return to main menu...");
                Console.ReadLine();
                Console.WriteLine("*****************************************************************\n");
            }
        }

        private static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\n Type the ID of the record you want to delete or press [0] to return to the main menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from drinking_water WHERE Id = {recordId}";
                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    Delete();
                }
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} successfully deleted\n\n");
            GetUserInput();
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("\n\n Insert date (format: dd-mm-yy). Press [0] to return");
            string dateInput = Console.ReadLine();
            if (dateInput == "0") GetUserInput();
            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
}