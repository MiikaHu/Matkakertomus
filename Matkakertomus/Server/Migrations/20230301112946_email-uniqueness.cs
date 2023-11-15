using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matkakertomus.Server.Migrations
{
    /// <inheritdoc />
    public partial class emailuniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_matkaaja_email",
                table: "matkaaja",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_matkaaja_email",
                table: "matkaaja");
        }
    }
}
