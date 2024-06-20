using FluentMigrator;

namespace FluentMigratorTask.Migrations
{
    [Migration(1)]
    public class M0001_CreateUserTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Users");
        }

        public override void Up()
        {
            Create.Table("Users")
            .WithColumn("UserID").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserName").AsString().NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Address").AsString().NotNullable()
            .WithColumn("Age").AsInt32().NotNullable();
        }
    }
}