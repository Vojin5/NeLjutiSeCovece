using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Back_End.Migrations
{
    /// <inheritdoc />
    public partial class V6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayedMatches_Matchers_MatchId",
                table: "PlayedMatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchers",
                table: "Matchers");

            migrationBuilder.RenameTable(
                name: "Matchers",
                newName: "Matches");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayedMatches_Matches_MatchId",
                table: "PlayedMatches",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayedMatches_Matches_MatchId",
                table: "PlayedMatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matchers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchers",
                table: "Matchers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayedMatches_Matchers_MatchId",
                table: "PlayedMatches",
                column: "MatchId",
                principalTable: "Matchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
