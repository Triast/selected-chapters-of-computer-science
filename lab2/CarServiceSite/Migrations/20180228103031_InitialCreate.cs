using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CarServiceSite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BodyNumber = table.Column<string>(nullable: true),
                    EngineNumber = table.Column<string>(nullable: true),
                    EngineVolume = table.Column<double>(nullable: false),
                    Mark = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    ReleaseYear = table.Column<int>(nullable: false),
                    StateNumber = table.Column<string>(nullable: true),
                    TechnicalPassport = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Inspectors",
                columns: table => new
                {
                    InspectorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<int>(nullable: false),
                    Subdivision = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspectors", x => x.InspectorId);
                });

            migrationBuilder.CreateTable(
                name: "CarTechStates",
                columns: table => new
                {
                    CarTechStateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdditionalEquipment = table.Column<string>(nullable: true),
                    BrakeSystem = table.Column<string>(nullable: true),
                    CarId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InspectorId = table.Column<int>(nullable: false),
                    Lightning = table.Column<string>(nullable: true),
                    MarkOnPassageOfServiceStation = table.Column<bool>(nullable: false),
                    Mileage = table.Column<double>(nullable: false),
                    Suspension = table.Column<string>(nullable: true),
                    Wheels = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTechStates", x => x.CarTechStateId);
                    table.ForeignKey(
                        name: "FK_CarTechStates_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTechStates_Inspectors_InspectorId",
                        column: x => x.InspectorId,
                        principalTable: "Inspectors",
                        principalColumn: "InspectorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarTechStates_CarId",
                table: "CarTechStates",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTechStates_InspectorId",
                table: "CarTechStates",
                column: "InspectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarTechStates");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Inspectors");
        }
    }
}
