using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DockDelivery.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastSending",
                table: "Departments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextSending",
                table: "Departments",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSending",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "NextSending",
                table: "Departments");
        }
    }
}
