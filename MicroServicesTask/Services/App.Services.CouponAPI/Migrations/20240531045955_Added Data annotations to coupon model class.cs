using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Services.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedDataannotationstocouponmodelclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CouponCode",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "DiscountAmount", "MinAmount" },
                values: new object[] { 3, "30OFF", 30.0, 150 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CouponCode",
                table: "Coupons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
