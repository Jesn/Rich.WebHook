using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rich.WebHook.Migrations
{
    /// <inheritdoc />
    public partial class update_webhookTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "WebHookTemplates",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "WebHookTemplates",
                newName: "FileName");
        }
    }
}
