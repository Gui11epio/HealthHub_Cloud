using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GS_USUARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailCorporativo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GS_QUESTIONARIO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataPreenchimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NivelEstresse = table.Column<int>(type: "int", nullable: false),
                    QualidadeSono = table.Column<int>(type: "int", nullable: false),
                    Ansiedade = table.Column<int>(type: "int", nullable: false),
                    Sobrecarga = table.Column<int>(type: "int", nullable: false),
                    Avaliacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GS_QUESTIONARIO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GS_QUESTIONARIO_GS_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "GS_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GS_QUESTIONARIO_UsuarioId",
                table: "GS_QUESTIONARIO",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GS_QUESTIONARIO");

            migrationBuilder.DropTable(
                name: "GS_USUARIO");
        }
    }
}
