using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt.Migrations
{
    /// <inheritdoc />
    public partial class Initialize1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Orders_OrdersId",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_OrdersId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "ShoppingCartItems");

            migrationBuilder.CreateTable(
                name: "OrderedItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingCartId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    SelectedColor = table.Column<string>(type: "text", nullable: true),
                    SelectedSize = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedItems_OrderId",
                table: "OrderedItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedItems");

            migrationBuilder.AddColumn<Guid>(
                name: "OrdersId",
                table: "ShoppingCartItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_OrdersId",
                table: "ShoppingCartItems",
                column: "OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Orders_OrdersId",
                table: "ShoppingCartItems",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
