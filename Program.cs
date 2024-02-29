using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace OneHabit
{
     class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source = OneHabit.db";

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
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("*****************************************************************\n");
                Console.WriteLine("\n\n MAIN MENU");
                Console.WriteLine("*****************************************************************\n");
                Console.WriteLine("\n What would you like to do?");
                Console.WriteLine("\n [0] Close app");
                Console.WriteLine("[1] View all records");
                Console.WriteLine("[2] Insert Record");
                Console.WriteLine("[3] Delete Record");
                Console.WriteLine("[4] Update Record");
                Console.WriteLine("*****************************************************************\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case 0:
                        Console.WriteLine("\nExit application? [y/n]\n");

                        var QuitOption = Console.ReadLine();

                        if (QuitOption.Trim().Equals("y", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Console.WriteLine("Goodbye!");
                            closeApp = true;
                            Environment.Exit(0);
                        }
                        break;

                    case 1:
                        GetAllRecords();
                        break;

                    case 2:
                        Insert();
                        break;

                    case 3:
                        Delete();
                        break;

                    case 4:
                        Update();
                        break;

                    default:
                        Console.WriteLine($"\n ERROR: Please choose a valid option...\n");
                        break;
                }
            }
        }

    }
}
