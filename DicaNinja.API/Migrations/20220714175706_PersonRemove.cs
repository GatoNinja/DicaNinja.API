using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DicaNinja.API.Migrations
{
    public partial class PersonRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "users",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "users",
                type: "character varying(48)",
                maxLength: 48,
                nullable: true,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE users
                SET description = (select description from people where people.user_id = users.id),
                    first_name = (select first_name from people where people.user_id = users.id),
                    image = (select image from people where people.user_id = users.id),
                    last_name = (select last_name from people where people.user_id = users.id)");

            migrationBuilder.Sql(@"
                ALTER TABLE users ALTER COLUMN first_name SET NOT NULL");

            migrationBuilder.Sql(@"
                ALTER TABLE users ALTER COLUMN last_name SET NOT NULL");

            migrationBuilder.DropTable(
                name: "people");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "users");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "image",
                table: "users");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "users");

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    first_name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    image = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_people", x => x.id);
                    table.ForeignKey(
                        name: "fk_people_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_people_user_id",
                table: "people",
                column: "user_id",
                unique: true);
        }
    }
}
