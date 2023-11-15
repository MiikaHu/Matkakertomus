using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class cascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_kuva_tarina1",
                table: "kuva");

            migrationBuilder.DropForeignKey(
                name: "fk_tarina_matka1",
                table: "tarina");

            migrationBuilder.AlterColumn<DateTime>(
                name: "pvm",
                table: "tarina",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "fk_kuva_tarina1",
                table: "kuva",
                column: "idtarina",
                principalTable: "tarina",
                principalColumn: "idtarina",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tarina_matka1",
                table: "tarina",
                column: "idmatka",
                principalTable: "matka",
                principalColumn: "idmatka",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_kuva_tarina1",
                table: "kuva");

            migrationBuilder.DropForeignKey(
                name: "fk_tarina_matka1",
                table: "tarina");

            migrationBuilder.AlterColumn<DateTime>(
                name: "pvm",
                table: "tarina",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_kuva_tarina1",
                table: "kuva",
                column: "idtarina",
                principalTable: "tarina",
                principalColumn: "idtarina");

            migrationBuilder.AddForeignKey(
                name: "fk_tarina_matka1",
                table: "tarina",
                column: "idmatka",
                principalTable: "matka",
                principalColumn: "idmatka");
        }
    }
}
