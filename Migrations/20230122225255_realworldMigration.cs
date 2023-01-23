using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace realworldProject.Migrations
{
    /// <inheritdoc />
    public partial class realworldMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticlesFavoriets",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesFavoriets", x => new { x.Username, x.Slug });
                });

            migrationBuilder.CreateTable(
                name: "ArticlesTags",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Tagname = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticlesTags", x => new { x.Slug, x.Tagname });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    Bio = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Slug = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Body = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AuthorName = table.Column<string>(type: "TEXT", nullable: false),
                    UserModelUsername = table.Column<string>(type: "TEXT", nullable: true),
                    UserModelUsername1 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Slug);
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserModelUsername",
                        column: x => x.UserModelUsername,
                        principalTable: "Users",
                        principalColumn: "Username");
                    table.ForeignKey(
                        name: "FK_Articles_Users_UserModelUsername1",
                        column: x => x.UserModelUsername1,
                        principalTable: "Users",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "UserFollowing",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FollowingName = table.Column<string>(type: "TEXT", nullable: false),
                    UserModelUsername = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollowing", x => new { x.Username, x.FollowingName });
                    table.ForeignKey(
                        name: "FK_UserFollowing_Users_UserModelUsername",
                        column: x => x.UserModelUsername,
                        principalTable: "Users",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserModelUsername",
                table: "Articles",
                column: "UserModelUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_UserModelUsername1",
                table: "Articles",
                column: "UserModelUsername1");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollowing_UserModelUsername",
                table: "UserFollowing",
                column: "UserModelUsername");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ArticlesFavoriets");

            migrationBuilder.DropTable(
                name: "ArticlesTags");

            migrationBuilder.DropTable(
                name: "UserFollowing");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
