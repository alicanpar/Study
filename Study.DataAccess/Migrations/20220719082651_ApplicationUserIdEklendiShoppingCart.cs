using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study.DataAccess.Migrations
{
    public partial class ApplicationUserIdEklendiShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ShoppingCarts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ShoppingCarts");
        }
    }
}
