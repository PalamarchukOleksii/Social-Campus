using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePublication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Publications");

            migrationBuilder.AddColumn<string>(
                name: "Base64ImageData",
                table: "Publications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64ImageData",
                table: "Publications");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Publications",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
