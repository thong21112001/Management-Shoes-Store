using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoesStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteForProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products",
                table: "ProductVariants");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategories_Products",
                table: "ProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products",
                table: "ProductVariants");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategories_Products",
                table: "ProductCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
