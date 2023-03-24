using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eShop.ProductApi.DataAccess.Migrations
{
    public partial class CategoryGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryGroupId",
                table: "Category",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "CategoryGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryGroup", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryGroupId",
                table: "Category",
                column: "CategoryGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_CategoryGroup_CategoryGroupId",
                table: "Category",
                column: "CategoryGroupId",
                principalTable: "CategoryGroup",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_CategoryGroup_CategoryGroupId",
                table: "Category");

            migrationBuilder.DropTable(
                name: "CategoryGroup");

            migrationBuilder.DropIndex(
                name: "IX_Category_CategoryGroupId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryGroupId",
                table: "Category");
        }
    }
}
