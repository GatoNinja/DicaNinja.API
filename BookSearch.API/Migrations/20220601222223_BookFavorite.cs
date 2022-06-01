using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookSearch.API.Migrations
{
    public partial class BookFavorite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "identifier",
                table: "favorites");

            migrationBuilder.DropColumn(
                name: "type",
                table: "favorites");

            migrationBuilder.AddColumn<Guid>(
                name: "book_id",
                table: "favorites",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_favorites_book_id",
                table: "favorites",
                column: "book_id");

            migrationBuilder.AddForeignKey(
                name: "fk_favorites_books_book_id",
                table: "favorites",
                column: "book_id",
                principalTable: "books",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_favorites_books_book_id",
                table: "favorites");

            migrationBuilder.DropIndex(
                name: "ix_favorites_book_id",
                table: "favorites");

            migrationBuilder.DropColumn(
                name: "book_id",
                table: "favorites");

            migrationBuilder.AddColumn<string>(
                name: "identifier",
                table: "favorites",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "favorites",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
