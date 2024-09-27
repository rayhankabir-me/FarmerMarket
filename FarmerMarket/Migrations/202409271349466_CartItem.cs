namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CartItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CartId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.Carts", t => t.CartId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.ProductId)
                .Index(t => t.CartId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CartItems", "CartId", "dbo.Carts");
            DropIndex("dbo.CartItems", new[] { "CartId" });
            DropIndex("dbo.CartItems", new[] { "ProductId" });
            DropTable("dbo.CartItems");
        }
    }
}
