using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DockDelivery.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CargoTypeId",
                table: "CargoSections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CargoSections_CargoTypeId",
                table: "CargoSections",
                column: "CargoTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CargoSections_DepartmentId",
                table: "CargoSections",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cargos_CargoSectionId",
                table: "Cargos",
                column: "CargoSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cargos_CargoSections_CargoSectionId",
                table: "Cargos",
                column: "CargoSectionId",
                principalTable: "CargoSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections",
                column: "CargoTypeId",
                principalTable: "CargoTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CargoSections_Departments_DepartmentId",
                table: "CargoSections",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cargos_CargoSections_CargoSectionId",
                table: "Cargos");

            migrationBuilder.DropForeignKey(
                name: "FK_CargoSections_CargoTypes_CargoTypeId",
                table: "CargoSections");

            migrationBuilder.DropForeignKey(
                name: "FK_CargoSections_Departments_DepartmentId",
                table: "CargoSections");

            migrationBuilder.DropIndex(
                name: "IX_CargoSections_CargoTypeId",
                table: "CargoSections");

            migrationBuilder.DropIndex(
                name: "IX_CargoSections_DepartmentId",
                table: "CargoSections");

            migrationBuilder.DropIndex(
                name: "IX_Cargos_CargoSectionId",
                table: "Cargos");

            migrationBuilder.DropColumn(
                name: "CargoTypeId",
                table: "CargoSections");
        }
    }
}
