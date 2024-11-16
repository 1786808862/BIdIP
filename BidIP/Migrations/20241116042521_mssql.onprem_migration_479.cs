using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BidIP.Migrations
{
    /// <inheritdoc />
    public partial class mssqlonprem_migration_479 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MachineCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MachineCategory");
        }
    }
}
