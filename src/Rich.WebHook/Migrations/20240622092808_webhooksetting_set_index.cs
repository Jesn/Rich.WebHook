using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rich.WebHook.Migrations
{
    /// <inheritdoc />
    public partial class webhooksetting_set_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "WebHookSettings",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WebHookSettings_TemplateId",
                table: "WebHookSettings",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_WebHookSettings_Token",
                table: "WebHookSettings",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebHookSettings_UserId",
                table: "WebHookSettings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebHookSettings_TemplateId",
                table: "WebHookSettings");

            migrationBuilder.DropIndex(
                name: "IX_WebHookSettings_Token",
                table: "WebHookSettings");

            migrationBuilder.DropIndex(
                name: "IX_WebHookSettings_UserId",
                table: "WebHookSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "WebHookSettings",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
