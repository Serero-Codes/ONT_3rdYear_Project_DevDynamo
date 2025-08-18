using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class Keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierItemID",
                table: "SupplierItems");

            migrationBuilder.DropColumn(
                name: "TakenItemID",
                table: "StockTakeItems");

            migrationBuilder.DropColumn(
                name: "DeliveryItemID",
                table: "DeliveryItems");

            migrationBuilder.DropColumn(
                name: "ConsumableOrderId",
                table: "ConsumableOrders");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "32cd475d-07fb-4065-9b73-57c4348c70e3", "AQAAAAIAAYagAAAAECTwpx5zgRgWGPfKH3u4TNrTxXmPd+GPpzEgghxTyNsJb9+xvs/WdixXoNorC8/sEQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "aa8bb80b-342d-4b93-a744-d93210d41782", "AQAAAAIAAYagAAAAEDcsjVI3LehpoLS7XPtspjbY04cAIGTbElRYvyH1C5tw+oOo37tmGkPygqS7bku8Tg==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 18, 7, 40, 45, 647, DateTimeKind.Local).AddTicks(7781));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierItemID",
                table: "SupplierItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakenItemID",
                table: "StockTakeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryItemID",
                table: "DeliveryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConsumableOrderId",
                table: "ConsumableOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c4cf7247-460a-4b8f-914f-a0aec3f74d94", "AQAAAAIAAYagAAAAEBS2vFvz0L3uZs31TlwEbt5THi2EAIADo74nCVWk8FTSU9P6vPreoh+ZLjYvOyBi3w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "786b151d-0f52-4ab8-a843-71c4bff36bdd", "AQAAAAIAAYagAAAAEIdpgB3HcEmtfF0YitZg2M+3aen+j5A6rGGKNMu57sq7wSrB1+g2PYGuKnys85ttAQ==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 18, 7, 5, 31, 268, DateTimeKind.Local).AddTicks(2466));
        }
    }
}
