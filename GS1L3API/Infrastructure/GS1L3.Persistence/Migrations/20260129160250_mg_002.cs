using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GS1L3.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg_002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SerialNumbers_WorkOrders_WorkOrderId",
                table: "SerialNumbers");

            migrationBuilder.AddForeignKey(
                name: "FK_SerialNumbers_WorkOrders_WorkOrderId",
                table: "SerialNumbers",
                column: "WorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SerialNumbers_WorkOrders_WorkOrderId",
                table: "SerialNumbers");

            migrationBuilder.AddForeignKey(
                name: "FK_SerialNumbers_WorkOrders_WorkOrderId",
                table: "SerialNumbers",
                column: "WorkOrderId",
                principalTable: "WorkOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
