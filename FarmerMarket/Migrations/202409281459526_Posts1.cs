namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Posts1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Description", c => c.String(nullable: false, maxLength: 80));
        }
    }
}
