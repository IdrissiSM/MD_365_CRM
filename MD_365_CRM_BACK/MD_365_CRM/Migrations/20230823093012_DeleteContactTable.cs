using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MD_365_CRM.Migrations
{
    /// <inheritdoc />
    public partial class DeleteContactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.AddColumn<bool>(
                name: "IsSynchronized",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSynchronized",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    contactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSynchronized = table.Column<bool>(type: "bit", nullable: false),
                    emailaddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gendercode = table.Column<int>(type: "int", nullable: true),
                    jobtitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secret = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statuscode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.contactId);
                });
        }
    }
}
