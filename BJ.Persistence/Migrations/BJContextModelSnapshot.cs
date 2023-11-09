﻿// <auto-generated />
using System;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BJ.Persistence.Migrations
{
    [DbContext(typeof(BJContext))]
    partial class BJContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BJ.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HasedPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Popular")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Blog", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.BlogTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("BlogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("LanguageId");

                    b.ToTable("BlogTranslation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CatName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.CategoryTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CatName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("LanguageId");

                    b.ToTable("CategoryTranslation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.Language", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Language", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Home")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Popular")
                        .HasColumnType("bit");

                    b.Property<bool>("Promotion")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("News", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.NewsTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("NewsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ShortDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("NewsId");

                    b.ToTable("NewsTranslation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("BestSeller")
                        .HasColumnType("bit");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Discount")
                        .HasColumnType("int");

                    b.Property<bool>("HomeTag")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePathCup")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePathHero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePathIngredients")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.ProductTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MetaDesc")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDesc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductTranslation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Size", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.SizeSpecificEachProduct", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<bool>("ActiveNutri")
                        .HasColumnType("bit");

                    b.Property<bool>("ActiveSize")
                        .HasColumnType("bit");

                    b.Property<string>("Cal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Carbonhydrate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CarbonhydrateSugar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("DietaryFibre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Energy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fat")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FatSaturated")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Protein")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sodium")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId", "SizeId");

                    b.HasIndex("SizeId");

                    b.ToTable("SizeSpecificEachProduct", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.StoreLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Repaired")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("StoreLocation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubCatName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubCategory", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategorySpecificProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("SubCategorySpecificProduct", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategoryTranslation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SubCatName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LanguageId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("SubCategoryTranslation", (string)null);
                });

            modelBuilder.Entity("BJ.Domain.Entities.BlogTranslation", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Blog", "Blog")
                        .WithMany("BlogTranslations")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BJ.Domain.Entities.Language", "Language")
                        .WithMany("BlogTranslations")
                        .HasForeignKey("LanguageId");

                    b.Navigation("Blog");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("BJ.Domain.Entities.CategoryTranslation", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Category", "Category")
                        .WithMany("CategoryTranslations")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BJ.Domain.Entities.Language", "Language")
                        .WithMany("CategoryTranslations")
                        .HasForeignKey("LanguageId");

                    b.Navigation("Category");

                    b.Navigation("Language");
                });

            modelBuilder.Entity("BJ.Domain.Entities.NewsTranslation", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Language", "Language")
                        .WithMany("NewsTranslations")
                        .HasForeignKey("LanguageId");

                    b.HasOne("BJ.Domain.Entities.News", "News")
                        .WithMany("NewsTranslations")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("News");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Product", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BJ.Domain.Entities.ProductTranslation", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Language", "Language")
                        .WithMany("ProductTranslations")
                        .HasForeignKey("LanguageId");

                    b.HasOne("BJ.Domain.Entities.Product", "Product")
                        .WithMany("ProductTranslations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("BJ.Domain.Entities.SizeSpecificEachProduct", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Product", "Product")
                        .WithMany("SizeSpecificProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BJ.Domain.Entities.Size", "Size")
                        .WithMany("SizeSpecificProducts")
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategorySpecificProduct", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Product", "Product")
                        .WithMany("SubCategorySpecificProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BJ.Domain.Entities.SubCategory", "SubCategory")
                        .WithMany("SubCategorySpecificProducts")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategoryTranslation", b =>
                {
                    b.HasOne("BJ.Domain.Entities.Language", "Language")
                        .WithMany("SubCategoryTranslations")
                        .HasForeignKey("LanguageId");

                    b.HasOne("BJ.Domain.Entities.SubCategory", "SubCategory")
                        .WithMany("SubCategoryTranslations")
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Blog", b =>
                {
                    b.Navigation("BlogTranslations");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Category", b =>
                {
                    b.Navigation("CategoryTranslations");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Language", b =>
                {
                    b.Navigation("BlogTranslations");

                    b.Navigation("CategoryTranslations");

                    b.Navigation("NewsTranslations");

                    b.Navigation("ProductTranslations");

                    b.Navigation("SubCategoryTranslations");
                });

            modelBuilder.Entity("BJ.Domain.Entities.News", b =>
                {
                    b.Navigation("NewsTranslations");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Product", b =>
                {
                    b.Navigation("ProductTranslations");

                    b.Navigation("SizeSpecificProducts");

                    b.Navigation("SubCategorySpecificProducts");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Size", b =>
                {
                    b.Navigation("SizeSpecificProducts");
                });

            modelBuilder.Entity("BJ.Domain.Entities.SubCategory", b =>
                {
                    b.Navigation("SubCategorySpecificProducts");

                    b.Navigation("SubCategoryTranslations");
                });
#pragma warning restore 612, 618
        }
    }
}
