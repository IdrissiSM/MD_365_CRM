﻿// <auto-generated />
using System;
using MD_365_CRM.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MD_365_CRM.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230824132034_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MD_365_CRM.Models.Account", b =>
                {
                    b.Property<Guid>("accountid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("emailaddress1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("entityimage_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("revenue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("statecode")
                        .HasColumnType("int");

                    b.HasKey("accountid");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MD_365_CRM.Models.BlacklistedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BlacklistedUsers");
                });

            modelBuilder.Entity("MD_365_CRM.Models.Incident", b =>
                {
                    b.Property<Guid>("incidentid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<Guid?>("_contactid_value")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("_customerid_value")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("_productid_value")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("caseorigincode")
                        .HasColumnType("int");

                    b.Property<int?>("casetypecode")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("createdon")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool?>("customercontacted")
                        .HasColumnType("bit");

                    b.Property<string>("customerid_contactemailaddress1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset?>("deactivatedon")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("prioritycode")
                        .HasColumnType("int");

                    b.Property<string>("productserialnumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("resolveby")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool?>("routecase")
                        .HasColumnType("bit");

                    b.Property<int?>("servicestage")
                        .HasColumnType("int");

                    b.Property<int>("statecode")
                        .HasColumnType("int");

                    b.Property<string>("ticketnumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("incidentid");

                    b.HasIndex("customerid_contactemailaddress1");

                    b.ToTable("Incidents");
                });

            modelBuilder.Entity("MD_365_CRM.Models.Opportunity", b =>
                {
                    b.Property<Guid>("OpportunityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ActualCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("ActualValue")
                        .HasColumnType("float");

                    b.Property<int?>("CloseProbability")
                        .HasColumnType("int");

                    b.Property<bool?>("CompleteInternalReview")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrentSituation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerNeed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EstimatedCloseDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("EstimatedValue")
                        .HasColumnType("float");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OpportunityRatingCode")
                        .HasColumnType("int");

                    b.Property<string>("ProposedSolution")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StateCode")
                        .HasColumnType("int");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("StepName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("_parentAccountId_value")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("_parentContactId_value")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OpportunityId");

                    b.ToTable("Opportunities");
                });

            modelBuilder.Entity("MD_365_CRM.Models.Otp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Otps");
                });

            modelBuilder.Entity("MD_365_CRM.Models.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrentCost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentCost_Base")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExchangeRate")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price_Base")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductStructure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductTypeCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuantityOnHand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StandardCost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StandardCost_Base")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StateCode")
                        .HasColumnType("int");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int");

                    b.Property<string>("StockVolume")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ValidFromDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidToDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VersionNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MD_365_CRM.Models.ProductOpportunity", b =>
                {
                    b.Property<Guid>("OpportunityProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OpportunityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double?>("PricePerUnit")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Quantity")
                        .HasColumnType("float");

                    b.Property<Guid>("_opportunityId_value")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("_productId_value")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OpportunityProductId");

                    b.HasIndex("OpportunityId");

                    b.ToTable("ProductOpportunities");
                });

            modelBuilder.Entity("MD_365_CRM.Models.ProfileImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasMaxLength(4194304)
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("ProfileImage");
                });

            modelBuilder.Entity("MD_365_CRM.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gendercode")
                        .HasColumnType("int");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("bit");

                    b.Property<string>("Jobtitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Statecode")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MD_365_CRM.Requests.ContactDTO", b =>
                {
                    b.Property<string>("emailaddress1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("firstname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("emailaddress1");

                    b.ToTable("ContactDTO");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MD_365_CRM.Models.Incident", b =>
                {
                    b.HasOne("MD_365_CRM.Requests.ContactDTO", "customerid_contact")
                        .WithMany()
                        .HasForeignKey("customerid_contactemailaddress1");

                    b.Navigation("customerid_contact");
                });

            modelBuilder.Entity("MD_365_CRM.Models.ProductOpportunity", b =>
                {
                    b.HasOne("MD_365_CRM.Models.Opportunity", null)
                        .WithMany("Product_opportunities")
                        .HasForeignKey("OpportunityId");
                });

            modelBuilder.Entity("MD_365_CRM.Models.User", b =>
                {
                    b.HasOne("MD_365_CRM.Models.ProfileImage", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MD_365_CRM.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MD_365_CRM.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MD_365_CRM.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MD_365_CRM.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MD_365_CRM.Models.Opportunity", b =>
                {
                    b.Navigation("Product_opportunities");
                });
#pragma warning restore 612, 618
        }
    }
}
