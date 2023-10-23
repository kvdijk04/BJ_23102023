﻿// <auto-generated />
using System;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BJ.Persistence.Migrations
{
    [DbContext(typeof(BJContext))]
    [Migration("20231002013140_AddBoolActiveToSize")]
    partial class AddBoolActiveToSize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Product", (string)null);
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

            modelBuilder.Entity("BJ.Domain.Entities.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DateCreated")
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

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SubCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("SubCategorySpecificProduct", (string)null);
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

            modelBuilder.Entity("BJ.Domain.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("BJ.Domain.Entities.Product", b =>
                {
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
                });
#pragma warning restore 612, 618
        }
    }
}
