using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matkaaja",
                columns: table => new
                {
                    idmatkaaja = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    etunimi = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    sukunimi = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    nimimerkki = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    paikkakunta = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    esittely = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    kuva = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    email = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idmatkaaja);
                });

            migrationBuilder.CreateTable(
                name: "matkakohde",
                columns: table => new
                {
                    idmatkakohde = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    kohdenimi = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    maa = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    paikkakunta = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true),
                    kuvausteksti = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    kuva = table.Column<string>(type: "TEXT", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idmatkakohde);
                });

            migrationBuilder.CreateTable(
                name: "matka",
                columns: table => new
                {
                    idmatka = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    alkupvm = table.Column<DateTime>(type: "date", nullable: true),
                    loppupvm = table.Column<DateTime>(type: "date", nullable: true),
                    yksityinen = table.Column<sbyte>(type: "INTEGER", nullable: true),
                    idmatkaaja = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idmatka);
                    table.ForeignKey(
                        name: "fk_matkaaja_has_matkakohde_matkaaja",
                        column: x => x.idmatkaaja,
                        principalTable: "matkaaja",
                        principalColumn: "idmatkaaja");
                });

            migrationBuilder.CreateTable(
                name: "tarina",
                columns: table => new
                {
                    idtarina = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pvm = table.Column<DateTime>(type: "date", nullable: false),
                    teksti = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    idmatka = table.Column<uint>(type: "INTEGER", nullable: false),
                    idmatkakohde = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idtarina);
                    table.ForeignKey(
                        name: "fk_tarina_matka1",
                        column: x => x.idmatka,
                        principalTable: "matka",
                        principalColumn: "idmatka");
                    table.ForeignKey(
                        name: "fk_tarina_matkakohde1",
                        column: x => x.idmatkakohde,
                        principalTable: "matkakohde",
                        principalColumn: "idmatkakohde");
                });

            migrationBuilder.CreateTable(
                name: "kuva",
                columns: table => new
                {
                    idkuva = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    kuva = table.Column<string>(type: "TEXT", maxLength: 45, nullable: false),
                    idtarina = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idkuva);
                    table.ForeignKey(
                        name: "fk_kuva_tarina1",
                        column: x => x.idtarina,
                        principalTable: "tarina",
                        principalColumn: "idtarina");
                });

            migrationBuilder.CreateIndex(
                name: "fk_kuva_tarina1_idx",
                table: "kuva",
                column: "idtarina");

            migrationBuilder.CreateIndex(
                name: "fk_matkaaja_has_matkakohde_matkaaja_idx",
                table: "matka",
                column: "idmatkaaja");

            migrationBuilder.CreateIndex(
                name: "fk_tarina_matka1_idx",
                table: "tarina",
                column: "idtarina");

            migrationBuilder.CreateIndex(
                name: "fk_tarina_matkakohde1_idx",
                table: "tarina",
                column: "idmatkakohde");

            migrationBuilder.CreateIndex(
                name: "IX_tarina_idmatka",
                table: "tarina",
                column: "idmatka");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kuva");

            migrationBuilder.DropTable(
                name: "tarina");

            migrationBuilder.DropTable(
                name: "matka");

            migrationBuilder.DropTable(
                name: "matkakohde");

            migrationBuilder.DropTable(
                name: "matkaaja");
        }
    }
}
