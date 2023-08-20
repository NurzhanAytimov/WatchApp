using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchApp.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationTypeForeignkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "Watches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Watches_ApplicationTypeId",
                table: "Watches",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Watches_ApplicationTypes_ApplicationTypeId",
                table: "Watches",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watches_ApplicationTypes_ApplicationTypeId",
                table: "Watches");

            migrationBuilder.DropIndex(
                name: "IX_Watches_ApplicationTypeId",
                table: "Watches");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Watches");
        }
    }
}
