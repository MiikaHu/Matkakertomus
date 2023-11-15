using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class addpwhashandsalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "matkaaja");

            migrationBuilder.AddColumn<byte[]>(
                name: "passwordSalt",
                table: "matkaaja",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "passwordhash",
                table: "matkaaja",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passwordSalt",
                table: "matkaaja");

            migrationBuilder.DropColumn(
                name: "passwordhash",
                table: "matkaaja");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "matkaaja",
                type: "TEXT",
                maxLength: 45,
                nullable: false,
                defaultValue: "");
        }
    }
}
