using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Mcc99 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_M_Employee",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Employee", x => x.NIK);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_Universitas",
                columns: table => new
                {
                    IdUniversity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamaUniversity = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Universitas", x => x.IdUniversity);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Tr_Akun",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Tr_Akun", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_Tb_Tr_Akun_Tb_M_Employee_NIK",
                        column: x => x.NIK,
                        principalTable: "Tb_M_Employee",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_M_Education",
                columns: table => new
                {
                    IdEducation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUniversity = table.Column<int>(type: "int", nullable: false),
                    UniversityIdUniversity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_M_Education", x => x.IdEducation);
                    table.ForeignKey(
                        name: "FK_Tb_M_Education_Tb_M_Universitas_UniversityIdUniversity",
                        column: x => x.UniversityIdUniversity,
                        principalTable: "Tb_M_Universitas",
                        principalColumn: "IdUniversity",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Tr_Profiling",
                columns: table => new
                {
                    NIK = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdEducation = table.Column<int>(type: "int", nullable: false),
                    EducationIdEducation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Tr_Profiling", x => x.NIK);
                    table.ForeignKey(
                        name: "FK_Tb_Tr_Profiling_Tb_M_Education_EducationIdEducation",
                        column: x => x.EducationIdEducation,
                        principalTable: "Tb_M_Education",
                        principalColumn: "IdEducation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tb_Tr_Profiling_Tb_Tr_Akun_NIK",
                        column: x => x.NIK,
                        principalTable: "Tb_Tr_Akun",
                        principalColumn: "NIK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_M_Education_UniversityIdUniversity",
                table: "Tb_M_Education",
                column: "UniversityIdUniversity");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Tr_Profiling_EducationIdEducation",
                table: "Tb_Tr_Profiling",
                column: "EducationIdEducation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tb_Tr_Profiling");

            migrationBuilder.DropTable(
                name: "Tb_M_Education");

            migrationBuilder.DropTable(
                name: "Tb_Tr_Akun");

            migrationBuilder.DropTable(
                name: "Tb_M_Universitas");

            migrationBuilder.DropTable(
                name: "Tb_M_Employee");
        }
    }
}
