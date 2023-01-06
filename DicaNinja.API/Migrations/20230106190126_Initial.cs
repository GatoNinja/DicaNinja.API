using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DicaNinja.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    title = table.Column<string>(type: "TEXT", nullable: false),
                    subtitle = table.Column<string>(type: "TEXT", nullable: true),
                    language = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    pagecount = table.Column<int>(name: "page_count", type: "INTEGER", nullable: true),
                    publisher = table.Column<string>(type: "TEXT", nullable: true),
                    publicationdate = table.Column<string>(name: "publication_date", type: "TEXT", nullable: true),
                    image = table.Column<string>(type: "TEXT", nullable: true),
                    averageratting = table.Column<double>(name: "average_ratting", type: "REAL", nullable: true),
                    previewlink = table.Column<string>(name: "preview_link", type: "TEXT", nullable: true),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    bookid = table.Column<Guid>(name: "book_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", maxLength: 48, nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    firstname = table.Column<string>(name: "first_name", type: "TEXT", maxLength: 48, nullable: false),
                    lastname = table.Column<string>(name: "last_name", type: "TEXT", maxLength: 48, nullable: false),
                    image = table.Column<string>(type: "TEXT", nullable: true),
                    description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorBook",
                columns: table => new
                {
                    AuthorsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BooksId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorBook", x => new { x.AuthorsId, x.BooksId });
                    table.ForeignKey(
                        name: "FK_AuthorBook_authors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorBook_books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "identifiers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    isbn = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<string>(type: "TEXT", nullable: false),
                    bookid = table.Column<Guid>(name: "book_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identifiers", x => x.id);
                    table.ForeignKey(
                        name: "FK_identifiers_books_book_id",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookCategory",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoriesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategory", x => new { x.BooksId, x.CategoriesId });
                    table.ForeignKey(
                        name: "FK_BookCategory_books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCategory_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bookmarks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "TEXT", nullable: false),
                    bookid = table.Column<Guid>(name: "book_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookmarks", x => x.id);
                    table.ForeignKey(
                        name: "FK_bookmarks_books_book_id",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookmarks_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "TEXT", nullable: false),
                    followerid = table.Column<Guid>(name: "follower_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followers", x => x.id);
                    table.ForeignKey(
                        name: "FK_followers_users_follower_id",
                        column: x => x.followerid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_followers_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "password_recoveries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    isactive = table.Column<bool>(name: "is_active", type: "INTEGER", nullable: false),
                    code = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "TEXT", nullable: false),
                    expiredate = table.Column<DateTimeOffset>(name: "expire_date", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_recoveries", x => x.id);
                    table.ForeignKey(
                        name: "FK_password_recoveries_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    value = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    refreshtokenexpirytime = table.Column<DateTimeOffset>(name: "refresh_token_expiry_time", type: "TEXT", nullable: false),
                    isactive = table.Column<bool>(name: "is_active", type: "INTEGER", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    text = table.Column<string>(type: "TEXT", maxLength: 2048, nullable: false),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    userid = table.Column<Guid>(name: "user_id", type: "TEXT", nullable: false),
                    bookid = table.Column<Guid>(name: "book_id", type: "TEXT", nullable: false),
                    created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.id);
                    table.ForeignKey(
                        name: "FK_reviews_books_book_id",
                        column: x => x.bookid,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_users_user_id",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorBook_BooksId",
                table: "AuthorBook",
                column: "BooksId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCategory_CategoriesId",
                table: "BookCategory",
                column: "CategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_book_id",
                table: "bookmarks",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_bookmarks_user_id",
                table: "bookmarks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_followers_follower_id",
                table: "followers",
                column: "follower_id");

            migrationBuilder.CreateIndex(
                name: "IX_followers_user_id_follower_id",
                table: "followers",
                columns: new[] { "user_id", "follower_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_identifiers_book_id",
                table: "identifiers",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_password_recoveries_user_id",
                table: "password_recoveries",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_book_id",
                table: "reviews",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_user_id",
                table: "reviews",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorBook");

            migrationBuilder.DropTable(
                name: "BookCategory");

            migrationBuilder.DropTable(
                name: "bookmarks");

            migrationBuilder.DropTable(
                name: "followers");

            migrationBuilder.DropTable(
                name: "identifiers");

            migrationBuilder.DropTable(
                name: "password_recoveries");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
