using FluentMigrator;

namespace FluentMigratorTask.Migrations
{
    [Migration(2)]
    public class M0002_InsertUserDetails : Migration
    {
        public override void Down()
        {
            //Truncate the table
            //1: Either by fluent syntax (or)
            //2: By Execute.Sql() (or)
            //3: By Using Execute.EmbeddedScript(<filename>)

            Execute.Sql(@"Truncate table Users");

        }

        public override void Up()
        {
            // Insert seed data
            //1: Either by fluent syntax (or)
            //2: By Execute.Sql() (or)
            //3: By Using Execute.EmbeddedScript(<filename>)
            Insert.IntoTable("Users").Row(new { Username = "Bharath", Email = "bharath@example.com", Address = "abc" ,Age = 22});
            Insert.IntoTable("Users").Row(new { Username = "sam", Email = "sam@example.com", Address = "def" ,Age = 22 });
            Insert.IntoTable("Users").Row(new { Username = "siri", Email = "siri@example.com", Address = "ghi" ,Age = 21 });
        }
    }
}