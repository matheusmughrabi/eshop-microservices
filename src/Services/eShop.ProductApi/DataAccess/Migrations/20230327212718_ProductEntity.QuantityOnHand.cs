using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.ProductApi.DataAccess.Migrations
{
    public partial class ProductEntityQuantityOnHand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityOnHand",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityOnHand",
                table: "Product");
        }
    }
}
