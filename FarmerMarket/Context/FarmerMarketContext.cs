using FarmerMarket.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Xml;

namespace FarmerMarket.Context
{
    public class FarmerMarketContext : DbContext
    {
        // Your context has been configured to use a 'FarmerMarketContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'FarmerMarket.Context.FarmerMarketContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'FarmerMarketContext' 
        // connection string in the application configuration file.
        public FarmerMarketContext()
            : base("name=FarmerMarketContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<RequestProduct> RequestProducts { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}