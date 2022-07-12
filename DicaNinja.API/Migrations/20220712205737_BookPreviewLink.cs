using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DicaNinja.API.Migrations
{
    public partial class BookPreviewLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "preview_link",
                table: "books",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preview_link",
                table: "books");
        }
    }
}
