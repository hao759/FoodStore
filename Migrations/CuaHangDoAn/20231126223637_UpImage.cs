using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuaHangDoAn.Migrations.CuaHangDoAn
{
    /// <inheritdoc />
    public partial class UpImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryProducts_CategoryProducts_CategoryProductId",
                table: "CategoryProducts");

            migrationBuilder.DropIndex(
                name: "IX_CategoryProducts_CategoryProductId",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "CategoryProductId",
                table: "CategoryProducts");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CategoryProductId",
                table: "CategoryProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryProducts_CategoryProductId",
                table: "CategoryProducts",
                column: "CategoryProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryProducts_CategoryProducts_CategoryProductId",
                table: "CategoryProducts",
                column: "CategoryProductId",
                principalTable: "CategoryProducts",
                principalColumn: "Id");
        }
    }
}
