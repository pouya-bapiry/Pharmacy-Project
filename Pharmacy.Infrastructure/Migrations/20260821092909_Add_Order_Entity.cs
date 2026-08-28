using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Order_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductColors_ProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "OrderPeriodTime",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "OrderDelivered",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderPaymentType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductColorId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductColorPrice",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductSelectedId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductSizePrice",
                table: "OrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderPeriodTime",
                table: "UserAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "UserAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderDelivered",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderPaymentType",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ProductColorId",
                table: "OrderDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductColorPrice",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "ProductSelectedId",
                table: "OrderDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProductSizeId",
                table: "OrderDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductSizePrice",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductColorId",
                table: "OrderDetails",
                column: "ProductColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductColors_ProductColorId",
                table: "OrderDetails",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
