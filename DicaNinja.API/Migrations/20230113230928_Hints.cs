using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DicaNinja.API.Migrations
{
    /// <inheritdoc />
    public partial class Hints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hints",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "uuid", nullable: false),
                    bookid = table.Column<Guid>(name: "book_id", type: "uuid", nullable: false),
                    liked = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hints", x => x.id);
                    table.ForeignKey(
                        name: "fk_hints_books_book_id",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_hints_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hints_book_id",
                table: "hints",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "ix_hints_user_id",
                table: "hints",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hints");
        }
    }
}
