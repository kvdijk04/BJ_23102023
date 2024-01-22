using BJ.Contract;
using BJ.Contract.Enum;
using BJ.Domain;
using BJ.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Identity.Client;
using System;
using System.Text;

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
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BJ2023;TrustServerCertificate=True;User ID=sa;Password=12345");

                return new BJContext(optionsBuilder.Options);
            }
        }
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductTranslation> ProductTranslations { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<BlogTranslation> BlogTranslations { get; set; }

        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsTranslation> NewsTranslations { get; set; }


        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryTranslation> CategoryTranslations { get; set; }

        public virtual DbSet<Size> Sizes { get; set; }

        public virtual DbSet<SubCategory> SubCategories { get; set; }

        public virtual DbSet<SizeSpecificEachProduct> SizeSpecificEachProduct { get; set; }

        public virtual DbSet<SubCategorySpecificProduct> SubCategorySpecificProducts { get; set; }
        public virtual DbSet<SubCategoryTranslation> SubCategoryTranslations { get; set; }

        public virtual DbSet<StoreLocation> StoreLocations { get; set; }
        public virtual DbSet<StoreLocationOpenHour> StoreLocationOpenHours { get; set; }

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<VisitorCounter> VisitorCounters { get; set; }
        public virtual DbSet<ConfigWebsite> ConfigWebs { get; set; }
        public virtual DbSet<DetailConfigWebsite> DetailConfigWebsites { get; set; }

        public virtual DbSet<DetailConfigWebsiteTranslation> DetailConfigWebsiteTranslations { get; set; }
        public virtual DbSet<Audit> AuditLogs { get; set; }
        public virtual DbSet<StoreLocationTranslation> StoreLocationTranslations { get; set; }

        public virtual async Task<int> SaveChangesAsync(string userId)
        {
            OnBeforeSaveChanges(userId);
            CancellationToken cancellationToken = default(CancellationToken);
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SizeSpecificEachProduct>().ToTable("SizeSpecificEachProduct").HasKey(sc => new { sc.ProductId, sc.SizeId });
            modelBuilder.Entity<Size>().ToTable("Size").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubCategory>().ToTable("SubCategory").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SubCategorySpecificProduct>().ToTable("SubCategorySpecificProduct");
            modelBuilder.Entity<Account>().ToTable("Account")
                                            .Property(e => e.AuthorizeRole)
                                            .HasConversion(
                                            v => v.ToString(),
                                            v => (AuthorizeRoles)Enum.Parse(typeof(AuthorizeRoles), v));
            modelBuilder.Entity<Blog>().ToTable("Blog");
            modelBuilder.Entity<News>().ToTable("News");


            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Category>().ToTable("Category").HasMany<Size>(x => x.Sizes).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId)
                                                        .OnDelete(DeleteBehavior.ClientSetNull); ;
            modelBuilder.Entity<CategoryTranslation>().ToTable("CategoryTranslation");
            modelBuilder.Entity<ProductTranslation>().ToTable("ProductTranslation");
            modelBuilder.Entity<SubCategoryTranslation>().ToTable("SubCategoryTranslation");
            modelBuilder.Entity<BlogTranslation>().ToTable("BlogTranslation");
            modelBuilder.Entity<NewsTranslation>().ToTable("NewsTranslation");
            modelBuilder.Entity<DetailConfigWebsite>().ToTable("DetailConfigWebsite");
            modelBuilder.Entity<ConfigWebsite>().ToTable("ConfigWebsite").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DetailConfigWebsiteTranslation>().ToTable("DetailConfigWebsiteTranslation");

            modelBuilder.Entity<Language>().ToTable("Language");
            modelBuilder.Entity<StoreLocation>().ToTable("StoreLocation").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StoreLocationOpenHour>().ToTable("StoreLocationOpenHour").Property(sc => sc.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<StoreLocationTranslation>().ToTable("StoreLocationTranslation");

            modelBuilder.Entity<VisitorCounter>().ToTable("VisitorCounter");
                //.HasData(
                //new VisitorCounter
                //{
                //    Id = Guid.NewGuid(),
                //    DayCount = 0,
                //    MonthCount = 0,
                //    YearCount = 0,
                //    Day = DateTime.Now.Day,
                //    Month = DateTime.Now.Month,
                //    Year = DateTime.Now.Year,
                //}
                //);

        }

    }
}
