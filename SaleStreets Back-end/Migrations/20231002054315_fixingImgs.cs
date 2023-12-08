using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleStreets_Back_end.Migrations
{
    public partial class fixingImgs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageArray",
                table: "Images",
                newName: "Images");

            migrationBuilder.AddColumn<string>(
                name: "ImgExtension",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgExtension",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Images",
                table: "Images",
                newName: "ImageArray");
        }
    }
}
