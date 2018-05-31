using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Migrations
{
    public partial class FullNameAndEngineVolumeTypesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "CarTechStates");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Inspectors",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "EngineVolume",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FullName",
                table: "Inspectors",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "CarTechStates",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EngineVolume",
                table: "Cars",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
