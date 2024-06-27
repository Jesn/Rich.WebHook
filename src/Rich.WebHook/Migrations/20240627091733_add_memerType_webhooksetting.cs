using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rich.WebHook.Migrations
{
    /// <inheritdoc />
    public partial class add_memerType_webhooksetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberType",
                table: "WebHookReceivers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberType",
                table: "WebHookReceivers");
        }
    }
}
