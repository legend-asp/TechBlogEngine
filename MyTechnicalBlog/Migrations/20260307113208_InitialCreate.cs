using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyTechnicalBlog.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Пользователи",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Пользователи", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Роли",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Роли", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Теги",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Теги", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Статьи",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    AuthorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Статьи", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Статьи_Пользователи_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Пользователи",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Пользователи_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Пользователи",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Роли_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Роли",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticlesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticlesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Статьи_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Статьи",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Теги_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Теги",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Комментарии",
                columns: table => new
                {
                    Идентификатор = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Текст_Комментария = table.Column<string>(type: "TEXT", nullable: false),
                    Статья_Идентификатор = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Комментарии", x => x.Идентификатор);
                    table.ForeignKey(
                        name: "FK_Комментарии_Статьи_Статья_Идентификатор",
                        column: x => x.Статья_Идентификатор,
                        principalTable: "Статьи",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Пользователи",
                columns: new[] { "Id", "Username" },
                values: new object[,]
                {
                    { 1, "AdminUser" },
                    { 2, "ModeratorUser" },
                    { 3, "RegularUser" }
                });

            migrationBuilder.InsertData(
                table: "Роли",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Администратор" },
                    { 2, "Модератор" },
                    { 3, "Пользователь" }
                });

            migrationBuilder.InsertData(
                table: "RoleUser",
                columns: new[] { "RolesId", "UsersId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagsId",
                table: "ArticleTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Комментарии_Статья_Идентификатор",
                table: "Комментарии",
                column: "Статья_Идентификатор");

            migrationBuilder.CreateIndex(
                name: "IX_Статьи_AuthorId",
                table: "Статьи",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Комментарии");

            migrationBuilder.DropTable(
                name: "Теги");

            migrationBuilder.DropTable(
                name: "Роли");

            migrationBuilder.DropTable(
                name: "Статьи");

            migrationBuilder.DropTable(
                name: "Пользователи");
        }
    }
}
