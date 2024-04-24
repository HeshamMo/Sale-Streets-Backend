using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaleStreets_Back_end.Migrations
{
    public partial class productUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "Price",
               table: "Products",
               type: "int",              
               nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
             name: "Price",
             table: "Products"); ;
        }
    }
}
