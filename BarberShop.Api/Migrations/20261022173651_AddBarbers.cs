using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBarbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barbers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barbers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BarberBarberService",
                columns: table => new
                {
                    BarberID = table.Column<int>(type: "int", nullable: false),
                    ServicesID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarberBarberService", x => new { x.BarberID, x.ServicesID });
                    table.ForeignKey(
                        name: "FK_BarberBarberService_Barbers_BarberID",
                        column: x => x.BarberID,
                        principalTable: "Barbers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BarberBarberService_Services_ServicesID",
                        column: x => x.ServicesID,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BarberBarberService_ServicesID",
                table: "BarberBarberService",
                column: "ServicesID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BarberBarberService");

            migrationBuilder.DropTable(
                name: "Barbers");
        }
    }
}
