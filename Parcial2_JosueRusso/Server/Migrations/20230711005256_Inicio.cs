using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parcial2_JosueRusso.Server.Migrations
{
    /// <inheritdoc />
    public partial class Inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entradas",
                columns: table => new
                {
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Concepto = table.Column<string>(type: "TEXT", nullable: false),
                    PesoTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadProducida = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entradas", x => x.EntradaId);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Existencia = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ProductoId);
                });

            migrationBuilder.CreateTable(
                name: "EntradasDetalle",
                columns: table => new
                {
                    DetalleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntradaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductoId = table.Column<int>(type: "INTEGER", nullable: false),
                    CantidadUtilizada = table.Column<double>(type: "REAL", nullable: false),
                    EntradasDetalle = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradasDetalle", x => x.DetalleId);
                    table.ForeignKey(
                        name: "FK_EntradasDetalle_Entradas_EntradasDetalle",
                        column: x => x.EntradasDetalle,
                        principalTable: "Entradas",
                        principalColumn: "EntradaId");
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "ProductoId", "Descripcion", "Existencia", "Tipo" },
                values: new object[,]
                {
                    { 1, "Mani", 50, 0 },
                    { 2, "Pistacho", 600, 0 },
                    { 3, "Pasas", 500, 0 },
                    { 4, "Ciruelas", 700, 0 },
                    { 5, "Mixto MPP 0.5 lb", 0, 0 },
                    { 6, "Mixto MPC 0.5 lb", 0, 0 },
                    { 7, "Mixto MPP 0.2 lb", 0, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntradasDetalle_EntradasDetalle",
                table: "EntradasDetalle",
                column: "EntradasDetalle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntradasDetalle");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Entradas");
        }
    }
}
