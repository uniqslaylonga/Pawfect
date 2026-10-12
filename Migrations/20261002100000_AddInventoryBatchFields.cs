using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Pawfect.Data;

#nullable disable

namespace Pawfect.Migrations
{
    [DbContext(typeof(PawfectDbContext))]
    [Migration("20261002100000_AddInventoryBatchFields")]
    public partial class AddInventoryBatchFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatchNumber", table: "InventoryItems", type: "text", nullable: false, defaultValue: "");
            migrationBuilder.AddColumn<string>(
                name: "Category", table: "InventoryItems", type: "text", nullable: false, defaultValue: "Supplies");
            migrationBuilder.AddColumn<DateOnly>(
                name: "ReceivedDate", table: "InventoryItems", type: "date", nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Supplier", table: "InventoryItems", type: "text", nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Unit", table: "InventoryItems", type: "text", nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "BatchNumber", table: "InventoryItems");
            migrationBuilder.DropColumn(name: "Category", table: "InventoryItems");
            migrationBuilder.DropColumn(name: "ReceivedDate", table: "InventoryItems");
            migrationBuilder.DropColumn(name: "Supplier", table: "InventoryItems");
            migrationBuilder.DropColumn(name: "Unit", table: "InventoryItems");
        }
    }
}
