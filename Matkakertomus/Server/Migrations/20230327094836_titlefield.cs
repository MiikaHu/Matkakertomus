using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class titlefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "matka",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<byte[]>(
                name: "kuva",
                table: "kuva",
                type: "BLOB",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 45);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "matka");

            migrationBuilder.AlterColumn<string>(
                name: "kuva",
                table: "kuva",
                type: "TEXT",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldMaxLength: 45);
        }
    }
}
