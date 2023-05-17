using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GateController.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficeMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemDevices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDevices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemCards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeMemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemCards_OfficeMembers_OfficeMemberId",
                        column: x => x.OfficeMemberId,
                        principalTable: "OfficeMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemCards_OfficeMemberId",
                table: "SystemCards",
                column: "OfficeMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemCards");

            migrationBuilder.DropTable(
                name: "SystemDevices");

            migrationBuilder.DropTable(
                name: "OfficeMembers");
        }
    }
}
