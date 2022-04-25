using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E_poles.Dal.Migrations
{
    public partial class updateby_updateat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Poles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Poles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Poles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Poles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Poles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Poles_UsersId",
                table: "Poles",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Poles_Users_UsersId",
                table: "Poles",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Poles_Users_UsersId",
                table: "Poles");

            migrationBuilder.DropIndex(
                name: "IX_Poles_UsersId",
                table: "Poles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Poles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Poles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Poles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Poles");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Poles");
        }
    }
}
