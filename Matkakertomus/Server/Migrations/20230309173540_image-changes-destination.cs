using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class imagechangesdestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "kuva",
                table: "matkakohde",
                type: "BLOB",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 45,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "kuva",
                table: "matkakohde",
                type: "TEXT",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldMaxLength: 45,
                oldNullable: true);
        }
    }
}
