namespace FarmerMarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class CreateOrdersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                {
                    OrderId = c.Int(nullable: false, identity: true),
                    OrderDate = c.DateTime(nullable: false),
                    TotalPrice = c.Double(nullable: false),
                    Quantity = c.Int(nullable: false),
                    PaymentMethod = c.String(nullable: false),
                    PaymentStatus = c.String(nullable: false),
                    DeliveryAddress = c.String(nullable: false),
                    OrderStatus = c.String(nullable: false),
                    ProductId = c.Int(nullable: false),
                    UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropTable("dbo.Orders");
        }
    }
}

