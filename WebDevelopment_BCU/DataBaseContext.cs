using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDevelopment_BCU.Models;

namespace WebDevelopment_BCU
{
    public class DataBaseContext : IdentityDbContext<User>
    {

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<About> About { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<UserBasket> UserBasket { get; set; }
        public virtual DbSet<UserOrders> UserOrders { get; set; }
        public virtual DbSet<UserOrderDetails> UserOrderDetails { get; set; }
        public virtual DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }

    }
}
