using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P02_FootballBetting.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_Games_GameId",
                table: "PlayerStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_Players_PlayerId",
                table: "PlayerStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStatistics",
                table: "PlayerStatistics");

            migrationBuilder.RenameTable(
                name: "PlayerStatistics",
                newName: "PlayersStatistics");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerStatistics_PlayerId",
                table: "PlayersStatistics",
                newName: "IX_PlayersStatistics_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersStatistics",
                table: "PlayersStatistics",
                columns: new[] { "GameId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersStatistics_Games_GameId",
                table: "PlayersStatistics",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersStatistics_Players_PlayerId",
                table: "PlayersStatistics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersStatistics_Games_GameId",
                table: "PlayersStatistics");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayersStatistics_Players_PlayerId",
                table: "PlayersStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersStatistics",
                table: "PlayersStatistics");

            migrationBuilder.RenameTable(
                name: "PlayersStatistics",
                newName: "PlayerStatistics");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_PlayersStatistics_PlayerId",
                table: "PlayerStatistics",
                newName: "IX_PlayerStatistics_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStatistics",
                table: "PlayerStatistics",
                columns: new[] { "GameId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_Games_GameId",
                table: "PlayerStatistics",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_Players_PlayerId",
                table: "PlayerStatistics",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
