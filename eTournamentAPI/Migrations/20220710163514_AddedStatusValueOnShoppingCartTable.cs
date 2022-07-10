using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTournamentAPI.Migrations
{
    public partial class AddedStatusValueOnShoppingCartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ShoppingCartItemes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ShoppingCartItemes");
        }
    }
}
