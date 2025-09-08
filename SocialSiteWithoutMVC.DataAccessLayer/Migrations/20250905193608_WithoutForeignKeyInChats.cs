using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialSiteWithoutMVC.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class WithoutForeignKeyInChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessagesId",
                table: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "MessagesId",
                table: "Chats",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }
    }
}
