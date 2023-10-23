using BJ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BJ.Persistence.ApplicationContext
{
    public partial class BJContext : DbContext
    {
        public BJContext() { }
        public BJContext(DbContextOptions<BJContext> options) : base(options) { }
        public class BloggingContextFactory : IDesignTimeDbContextFactory<BJContext>
        {
            public BJContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BJContext>();
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BJ2023;TrustServerCertificate=True;User ID=sa;Password=12345",
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

                return new BJContext(optionsBuilder.Options);
            }
        }
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }

        public virtual DbSet<SubCategory> SubCategories { get; set; }

        public virtual DbSet<SizeSpecificEachProduct> SizeSpecificEachProduct { get; set; }
        public virtual DbSet<SubCategorySpecificProduct> SubCategorySpecificProducts { get; set; }
        public virtual DbSet<StoreLocation> StoreLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SizeSpecificEachProduct>().ToTable("SizeSpecificEachProduct").HasKey(sc => new { sc.ProductId, sc.SizeId });
            modelBuilder.Entity<Size>().ToTable("Size").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubCategory>().ToTable("SubCategory").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubCategorySpecificProduct>().ToTable("SubCategorySpecificProduct");
            modelBuilder.Entity<Account>().ToTable("Account");


            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<StoreLocation>().ToTable("StoreLocation").Property(sc => sc.Id).ValueGeneratedOnAdd();

        }

    }
}
