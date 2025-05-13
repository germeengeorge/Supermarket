using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Data
{
    public class AppDbContext:IdentityDbContext
    {
       public DbSet<Category> categories {  get; set; }
       public DbSet<Product> Products { get; set; }
       public DbSet<ShoppingCart> shoppingCarts { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    var Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        //    var ConnectionString = Config.GetSection("ConnectionString").Value;
        //    optionsBuilder.UseSqlServer(ConnectionString);
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>().HasData(
                new Category { id = 1, Name = "Fruits", DisplayOrder = 1, logoUrl = "fruits.jpeg" },
                new Category { id = 2, Name = "Vegetables", DisplayOrder = 3, logoUrl = "vegetables.jpeg" },
                new Category { id = 3, Name = "Drinks", DisplayOrder = 3, logoUrl = "drinks.png" }
            );

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Tomato", Description = "", imgURL = "", price = 60, CategoryId = 2 }
                );
 
        }

    }
}
