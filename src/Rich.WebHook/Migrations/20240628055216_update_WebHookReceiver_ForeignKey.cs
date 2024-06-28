using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rich.WebHook.Migrations
{
    /// <inheritdoc />
    public partial class update_WebHookReceiver_ForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WebHookReceivers_WebHookId",
                table: "WebHookReceivers",
                column: "WebHookId");

            migrationBuilder.AddForeignKey(
                name: "FK_WebHookReceivers_WebHookSettings_WebHookId",
                table: "WebHookReceivers",
                column: "WebHookId",
                principalTable: "WebHookSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebHookReceivers_WebHookSettings_WebHookId",
                table: "WebHookReceivers");

            migrationBuilder.DropIndex(
                name: "IX_WebHookReceivers_WebHookId",
                table: "WebHookReceivers");
        }
    }
}
