namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartTable1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Carts", "ProductId", "dbo.Products");
            DropIndex("dbo.Carts", new[] { "ProductId" });
            DropColumn("dbo.Carts", "Quantity");
            DropColumn("dbo.Carts", "TotalPrice");
            DropColumn("dbo.Carts", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Carts", "ProductId", c => c.Int(nullable: false));
            AddColumn("dbo.Carts", "TotalPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Carts", "Quantity", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "ProductId");
            AddForeignKey("dbo.Carts", "ProductId", "dbo.Products", "ProductId", cascadeDelete: true);
        }
    }
}
