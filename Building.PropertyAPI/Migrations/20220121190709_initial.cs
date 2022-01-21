using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Building.PropertyAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    IdProperty = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Price = table.Column<long>(type: "INTEGER", nullable: false),
                    CodeInternal = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    IdOwner = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.IdProperty);
                });

            migrationBuilder.CreateTable(
                name: "PropertyImage",
                columns: table => new
                {
                    IdPropertyImage = table.Column<Guid>(type: "TEXT", nullable: false),
                    File = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IdProperty = table.Column<Guid>(type: "TEXT", nullable: false),
                    PropertyIdProperty = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyImage", x => x.IdPropertyImage);
                    table.ForeignKey(
                        name: "FK_PropertyImage_Property_PropertyIdProperty",
                        column: x => x.PropertyIdProperty,
                        principalTable: "Property",
                        principalColumn: "IdProperty",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImage_PropertyIdProperty",
                table: "PropertyImage",
                column: "PropertyIdProperty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyImage");

            migrationBuilder.DropTable(
                name: "Property");
        }
    }
}
