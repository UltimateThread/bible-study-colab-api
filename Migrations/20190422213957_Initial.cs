using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BibleStudyColabApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BibleVersions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Abbreviation = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    InfoUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BibleVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BibleVersionId = table.Column<string>(nullable: true),
                    Book = table.Column<string>(nullable: true),
                    Testament = table.Column<string>(nullable: true),
                    ChapterNumber = table.Column<int>(nullable: false),
                    VerseNumber = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verses_BibleVersions_BibleVersionId",
                        column: x => x.BibleVersionId,
                        principalTable: "BibleVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Verses_BibleVersionId",
                table: "Verses",
                column: "BibleVersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Verses");

            migrationBuilder.DropTable(
                name: "BibleVersions");
        }
    }
}
