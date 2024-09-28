namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequestProducts",
                c => new
                    {
                        ReqId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 20),
                        phone = c.String(nullable: false, maxLength: 11),
                        Time = c.DateTime(nullable: false),
                        Massage = c.String(maxLength: 25),
                    })
                .PrimaryKey(t => t.ReqId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RequestProducts");
        }
    }
}
