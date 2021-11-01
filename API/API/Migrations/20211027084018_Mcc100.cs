using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Mcc100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Tr_Role",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Tr_Role", x => x.IdRole);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Tr_AccRole",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Tr_AccRole", x => new { x.NIK, x.IdRole });
                    table.ForeignKey(
                        name: "FK_Tb_Tr_AccRole_Tb_Tr_Akun_NIK",
                        column: x => x.NIK,
                        principalTable: "Tb_Tr_Akun",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tb_Tr_AccRole_Tb_Tr_Role_IdRole",
                        column: x => x.IdRole,
                        principalTable: "Tb_Tr_Role",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Tr_AccRole_IdRole",
                table: "Tb_Tr_AccRole",
                column: "IdRole");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Tr_AccRole");

            migrationBuilder.DropTable(
                name: "Tb_Tr_Role");
        }
    }
}
