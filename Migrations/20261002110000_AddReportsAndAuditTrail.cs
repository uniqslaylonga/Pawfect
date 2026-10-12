using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pawfect.Data;

#nullable disable

namespace Pawfect.Migrations
{
    [DbContext(typeof(PawfectDbContext))]
    [Migration("20261002110000_AddReportsAndAuditTrail")]
    public partial class AddReportsAndAuditTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category", table: "RetailSales", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Notes", table: "RetailSales", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ProductName", table: "RetailSales", type: "text", nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "Quantity", table: "RetailSales", type: "integer", nullable: false, defaultValue: 1);
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNo", table: "RetailSales", type: "text", nullable: false, defaultValue: "");
            migrationBuilder.AddColumn<string>(
                name: "StaffName", table: "RetailSales", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Status", table: "RetailSales", type: "text", nullable: false, defaultValue: "Pending");
            migrationBuilder.AddColumn<string>(
                name: "TransactionType", table: "RetailSales", type: "text", nullable: false, defaultValue: "Sale");

            migrationBuilder.CreateTable(
                name: "InventoryMovements",
                columns: table => new
                {
                    InventoryMovementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    BatchNumber = table.Column<string>(type: "text", nullable: true),
                    MovementType = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMovements", x => x.InventoryMovementId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "InventoryMovements");

            migrationBuilder.DropColumn(name: "Category", table: "RetailSales");
            migrationBuilder.DropColumn(name: "Notes", table: "RetailSales");
            migrationBuilder.DropColumn(name: "ProductName", table: "RetailSales");
            migrationBuilder.DropColumn(name: "Quantity", table: "RetailSales");
            migrationBuilder.DropColumn(name: "ReferenceNo", table: "RetailSales");
            migrationBuilder.DropColumn(name: "StaffName", table: "RetailSales");
            migrationBuilder.DropColumn(name: "Status", table: "RetailSales");
            migrationBuilder.DropColumn(name: "TransactionType", table: "RetailSales");
        }
    }
}
