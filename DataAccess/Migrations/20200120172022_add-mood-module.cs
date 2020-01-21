using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DataAccess.Migrations
{
    public partial class addmoodmodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mood",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mood", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMood",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    MoodId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMood", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMood_Mood_MoodId",
                        column: x => x.MoodId,
                        principalTable: "Mood",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMood_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMood_MoodId",
                table: "UserMood",
                column: "MoodId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMood_UserId",
                table: "UserMood",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMood");

            migrationBuilder.DropTable(
                name: "Mood");
        }
    }
}
