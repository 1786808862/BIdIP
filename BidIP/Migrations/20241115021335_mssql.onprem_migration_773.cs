using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidIP.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_773 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MachineCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MachineDetailInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSSYTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SsyCount = table.Column<int>(type: "int", nullable: false),
                    MachineCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineDetailInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineDetailInfo_MachineCategory_MachineCategoryId",
                        column: x => x.MachineCategoryId,
                        principalTable: "MachineCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineDetailInfo_MachineCategoryId",
                table: "MachineDetailInfo",
                column: "MachineCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MachineDetailInfo");

            migrationBuilder.DropTable(
                name: "MachineCategory");
        }
    }
}
