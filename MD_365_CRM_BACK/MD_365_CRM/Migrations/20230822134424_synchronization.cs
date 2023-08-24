using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MD_365_CRM.Migrations
{
    /// <inheritdoc />
    public partial class synchronization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    accountid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emailaddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    entityimage_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    statecode = table.Column<int>(type: "int", nullable: true),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.accountid);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    contactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailaddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jobtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gendercode = table.Column<int>(type: "int", nullable: true),
                    statuscode = table.Column<int>(type: "int", nullable: true),
                    secret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.contactId);
                });

            migrationBuilder.CreateTable(
                name: "Opportunities",
                columns: table => new
                {
                    OpportunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedValue = table.Column<double>(type: "float", nullable: true),
                    ActualValue = table.Column<double>(type: "float", nullable: true),
                    EstimatedCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CloseProbability = table.Column<int>(type: "int", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: true),
                    StateCode = table.Column<int>(type: "int", nullable: true),
                    OpportunityRatingCode = table.Column<int>(type: "int", nullable: true),
                    CompleteInternalReview = table.Column<bool>(type: "bit", nullable: true),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedSolution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerNeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentSituation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _parentAccountId_value = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _parentContactId_value = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opportunities", x => x.OpportunityId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductStructure = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price_Base = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductTypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityOnHand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandardCost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentCost_Base = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandardCost_Base = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateCode = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    StockVolume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExchangeRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VersionNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidFromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false),
                    ValidToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductOpportunities",
                columns: table => new
                {
                    OpportunityProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerUnit = table.Column<double>(type: "float", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _opportunityId_value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false),
                    _productId_value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpportunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOpportunities", x => x.OpportunityProductId);
                    table.ForeignKey(
                        name: "FK_ProductOpportunities_Opportunities_OpportunityId",
                        column: x => x.OpportunityId,
                        principalTable: "Opportunities",
                        principalColumn: "OpportunityId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOpportunities_OpportunityId",
                table: "ProductOpportunities",
                column: "OpportunityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductOpportunities");

            migrationBuilder.DropTable(
                name: "Opportunities");
        }
    }
}
