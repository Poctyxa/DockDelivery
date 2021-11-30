using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DockDelivery.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "CargoSections");

            migrationBuilder.AlterColumn<Guid>(
                name: "CargoTypeId",
                table: "CargoSections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections",
                column: "CargoTypeId",
                principalTable: "CargoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections");

            migrationBuilder.AlterColumn<Guid>(
                name: "CargoTypeId",
                table: "CargoSections",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "CargoSections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections",
                column: "CargoTypeId",
                principalTable: "CargoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
