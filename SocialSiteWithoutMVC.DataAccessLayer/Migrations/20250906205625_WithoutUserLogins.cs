using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialSiteWithoutMVC.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class WithoutUserLogins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLogins",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "UserLogins",
                table: "Chats",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }
    }
}
