using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MD_365_CRM.Migrations
{
    /// <inheritdoc />
    public partial class synchronization_incidents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactDTO",
                columns: table => new
                {
                    emailaddress1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDTO", x => x.emailaddress1);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    incidentid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    servicestage = table.Column<int>(type: "int", nullable: true),
                    resolveby = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    statecode = table.Column<int>(type: "int", nullable: false),
                    caseorigincode = table.Column<int>(type: "int", nullable: true),
                    casetypecode = table.Column<int>(type: "int", nullable: true),
                    productserialnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdon = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    customercontacted = table.Column<bool>(type: "bit", nullable: true),
                    _contactid_value = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _productid_value = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    _customerid_value = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticketnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deactivatedon = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    routecase = table.Column<bool>(type: "bit", nullable: true),
                    prioritycode = table.Column<int>(type: "int", nullable: true),
                    customerid_contactemailaddress1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.incidentid);
                    table.ForeignKey(
                        name: "FK_Incidents_ContactDTO_customerid_contactemailaddress1",
                        column: x => x.customerid_contactemailaddress1,
                        principalTable: "ContactDTO",
                        principalColumn: "emailaddress1");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_customerid_contactemailaddress1",
                table: "Incidents",
                column: "customerid_contactemailaddress1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "ContactDTO");
        }
    }
}
