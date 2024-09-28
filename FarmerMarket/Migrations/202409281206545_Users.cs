namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Int(nullable: false, identity: true),
                    UserName = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    Role = c.String(nullable: false),
                })
                .PrimaryKey(t => t.UserId);

        }

        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}