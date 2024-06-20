using FluentMigrator;

namespace FluentMigratorTask.Migrations
{
    [Migration(3)]
    public class M0003_CreateSPGetAllUsers : Migration
    {
        public override void Down()
        {
            // string dropSPGetAllUsers = GetSqlScript(@"C:\Users\bharath.mokara\Desktop\FluentMigratorTask2\FluentMigratorTask\Scripts\DropSP_GetAllUsers.sql");
            Execute.EmbeddedScript("Scripts.DropSP_GetAllUsers.sql");
        }

        public override void Up()
        {
            // string createSpGetAllUsers = GetSqlScript(@"C:\Users\bharath.mokara\Desktop\FluentMigratorTask2\FluentMigratorTask\Scripts\CreateSP_GetAllUsers.sql");
            Execute.EmbeddedScript("Scripts.CreateSP_GetAllUsers.sql");
        }

        // private string GetSqlScript(string scriptName)
        // {
        //     // Specify the directory where your SQL scripts are located
        //     string scriptDirectory = @"C:\Users\bharath.mokara\Desktop\FluentMigratorTask2\FluentMigratorTask\Scripts\";

        //     try
        //     {
        //         // Read the contents of the SQL script file
        //         string scriptContent = System.IO.File.ReadAllText(System.IO.Path.Combine(scriptDirectory, scriptName));

        //         return scriptContent;
        //     }
        //     catch (System.IO.IOException ex)
        //     {
        //         // Handle IO exception
        //         Console.WriteLine($"Error reading SQL script file: {ex.Message}");
        //         throw;
        //     }
        // }
    }
}