using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VinylStore.Api.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteInstructions_Albums_AlbumId",
                table: "RouteInstructions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RouteInstructions",
                table: "RouteInstructions");

            migrationBuilder.RenameTable(
                name: "RouteInstructions",
                newName: "Tracks");

            migrationBuilder.RenameIndex(
                name: "IX_RouteInstructions_AlbumId",
                table: "Tracks",
                newName: "IX_Tracks_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tracks",
                table: "Tracks");

            migrationBuilder.RenameTable(
                name: "Tracks",
                newName: "RouteInstructions");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_AlbumId",
                table: "RouteInstructions",
                newName: "IX_RouteInstructions_AlbumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RouteInstructions",
                table: "RouteInstructions",
                column: "TrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteInstructions_Albums_AlbumId",
                table: "RouteInstructions",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "AlbumId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
