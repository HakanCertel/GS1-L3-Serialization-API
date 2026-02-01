using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GS1L3.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg_005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Customers");
        }
    }
}
