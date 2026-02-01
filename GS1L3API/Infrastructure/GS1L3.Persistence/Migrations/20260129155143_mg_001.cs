using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GS1L3.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg_001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GLN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GTIN = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LotNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetQuantity = table.Column<int>(type: "int", nullable: false),
                    ProducedQuantity = table.Column<int>(type: "int", nullable: false),
                    SerialStartValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SSCCs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SSCCCode = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Level = table.Column<byte>(type: "tinyint", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentSSCCId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSCCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SSCCs_SSCCs_ParentSSCCId",
                        column: x => x.ParentSSCCId,
                        principalTable: "SSCCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SSCCs_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SerialNumbers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SSCCId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SerialNumbers_SSCCs_SSCCId",
                        column: x => x.SSCCId,
                        principalTable: "SSCCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SerialNumbers_WorkOrders_WorkOrderId",
                        column: x => x.WorkOrderId,
                        principalTable: "WorkOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CustomerId",
                table: "Products",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_GTIN",
                table: "Products",
                column: "GTIN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumbers_SN",
                table: "SerialNumbers",
                column: "SN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumbers_SSCCId",
                table: "SerialNumbers",
                column: "SSCCId");

            migrationBuilder.CreateIndex(
                name: "IX_SerialNumbers_WorkOrderId",
                table: "SerialNumbers",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SSCCs_ParentSSCCId",
                table: "SSCCs",
                column: "ParentSSCCId");

            migrationBuilder.CreateIndex(
                name: "IX_SSCCs_SSCCCode",
                table: "SSCCs",
                column: "SSCCCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SSCCs_WorkOrderId",
                table: "SSCCs",
                column: "WorkOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductId",
                table: "WorkOrders",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialNumbers");

            migrationBuilder.DropTable(
                name: "SSCCs");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
