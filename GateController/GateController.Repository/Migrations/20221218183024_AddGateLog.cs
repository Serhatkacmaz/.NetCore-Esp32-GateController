using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateController.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddGateLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GateLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeMemberId = table.Column<long>(type: "bigint", nullable: false),
                    LoginDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GateLogs_OfficeMembers_OfficeMemberId",
                        column: x => x.OfficeMemberId,
                        principalTable: "OfficeMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GateLogs_OfficeMemberId",
                table: "GateLogs",
                column: "OfficeMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GateLogs");
        }
    }
}
