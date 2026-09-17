using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.asp.net.backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CatName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QCatId = table.Column<int>(type: "INTEGER", nullable: false),
                    QText = table.Column<string>(type: "TEXT", nullable: false),
                    QOptionA = table.Column<string>(type: "TEXT", nullable: false),
                    QOptionB = table.Column<string>(type: "TEXT", nullable: false),
                    QOptionC = table.Column<string>(type: "TEXT", nullable: false),
                    QOptionD = table.Column<string>(type: "TEXT", nullable: false),
                    QCorrectOption = table.Column<char>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TQuestions_TCategories_QCatId",
                        column: x => x.QCatId,
                        principalTable: "TCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SCatId = table.Column<int>(type: "INTEGER", nullable: false),
                    SPlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    SCorrect = table.Column<int>(type: "INTEGER", nullable: false),
                    STotal = table.Column<int>(type: "INTEGER", nullable: false),
                    SCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TScores_TCategories_SCatId",
                        column: x => x.SCatId,
                        principalTable: "TCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TQuestions_QCatId",
                table: "TQuestions",
                column: "QCatId");

            migrationBuilder.CreateIndex(
                name: "IX_TScores_SCatId",
                table: "TScores",
                column: "SCatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TQuestions");

            migrationBuilder.DropTable(
                name: "TScores");

            migrationBuilder.DropTable(
                name: "TCategories");
        }
    }
}
