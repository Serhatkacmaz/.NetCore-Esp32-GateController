using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateController.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCardId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CardId",
                table: "GateLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardId",
                table: "GateLogs");
        }
    }
}
