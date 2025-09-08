using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialSiteWithoutMVC.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "MessagesId",
                table: "Chats",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "UserLogins",
                table: "Chats",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "UserLogins",
                table: "Chats");
        }
    }
}
