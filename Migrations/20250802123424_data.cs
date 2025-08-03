using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameColumn(
                name: "ChronicMed",
                table: "Patients",
                newName: "ChronicIllness");

            migrationBuilder.AddColumn<string>(
                name: "PulseRate",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderedBy",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WardID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityApproved",
                table: "ConsumableOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityRequested",
                table: "ConsumableOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ConsumableOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReasonForAdmission",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredBy = table.Column<int>(type: "int", nullable: false),
                    DeliveredByUserId = table.Column<int>(type: "int", nullable: true),
                    RecievedBy = table.Column<int>(type: "int", nullable: false),
                    RecievedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryID);
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_DeliveredByUserId",
                        column: x => x.DeliveredByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_RecievedByUserId",
                        column: x => x.RecievedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTakes",
                columns: table => new
                {
                    StockTakeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    StockTakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TakenBy = table.Column<int>(type: "int", nullable: false),
                    TakenByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakes", x => x.StockTakeID);
                    table.ForeignKey(
                        name: "FK_StockTakes_AspNetUsers_TakenByUserId",
                        column: x => x.TakenByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTakes_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierItems",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    SupplierItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierItems", x => new { x.SupplierID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_SupplierItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierItems_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WardConsumables",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardConsumables", x => new { x.WardID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_WardConsumables_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WardConsumables_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryItems",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    DeliveryItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityDelivered = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryItems", x => new { x.DeliveryID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_DeliveryItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryItems_Deliveries_DeliveryID",
                        column: x => x.DeliveryID,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTakeItems",
                columns: table => new
                {
                    StockTakeID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    TakenItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityCounted = table.Column<int>(type: "int", nullable: false),
                    SystemQuantity = table.Column<int>(type: "int", nullable: false),
                    Discrepancy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakeItems", x => new { x.StockTakeID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_StockTakeItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTakeItems_StockTakes_StockTakeID",
                        column: x => x.StockTakeID,
                        principalTable: "StockTakes",
                        principalColumn: "StockTakeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "227a7c73-abfc-40e5-ba72-712dd6420b9a", "AQAAAAIAAYagAAAAECH524j/47Ep51NOS3ZhBC7uNtnFktgdKG4MYrF6+9xxc7kQwodxHy5J29olPNRCUg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e4ebc0b9-cb64-44fa-9118-f2df99ee29f2", "AQAAAAIAAYagAAAAECHVsnc4H+XMpSkc6vtk1aX3Z0wvKR8dJLJ/ZekAH6qxqSIWSWEhclst2KbWYqN2KQ==" });

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 1,
                column: "PulseRate",
                value: "72bpm");

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 2,
                column: "PulseRate",
                value: "60bpm");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedBy",
                table: "Orders",
                column: "OrderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WardID",
                table: "Orders",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveredByUserId",
                table: "Deliveries",
                column: "DeliveredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderID",
                table: "Deliveries",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_RecievedByUserId",
                table: "Deliveries",
                column: "RecievedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryItems_ConsumableID",
                table: "DeliveryItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakeItems_ConsumableID",
                table: "StockTakeItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakes_TakenByUserId",
                table: "StockTakes",
                column: "TakenByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakes_WardID",
                table: "StockTakes",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierItems_ConsumableID",
                table: "SupplierItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_WardConsumables_ConsumableID",
                table: "WardConsumables",
                column: "ConsumableID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_OrderedBy",
                table: "Orders",
                column: "OrderedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Wards_WardID",
                table: "Orders",
                column: "WardID",
                principalTable: "Wards",
                principalColumn: "WardID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_OrderedBy",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Wards_WardID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliveryItems");

            migrationBuilder.DropTable(
                name: "StockTakeItems");

            migrationBuilder.DropTable(
                name: "SupplierItems");

            migrationBuilder.DropTable(
                name: "WardConsumables");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "StockTakes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderedBy",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WardID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PulseRate",
                table: "Vitals");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "OrderedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WardID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QuantityApproved",
                table: "ConsumableOrders");

            migrationBuilder.DropColumn(
                name: "QuantityRequested",
                table: "ConsumableOrders");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ConsumableOrders");

            migrationBuilder.DropColumn(
                name: "ReasonForAdmission",
                table: "Admissions");

            migrationBuilder.RenameColumn(
                name: "ChronicIllness",
                table: "Patients",
                newName: "ChronicMed");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Admissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "Admin", "ADMIN" },
                    { 2, null, "Doctor", "DOCTOR" },
                    { 3, null, "Nurse", "NURSE" },
                    { 4, null, "Sister", "SISTER" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "485af8da-774a-433d-98c6-b4d77837946c", "AQAAAAIAAYagAAAAENL+JqZKyYc8ubzU5F//4/cw6daAWpmi3+xvpaXb2IF1jGjJ+TLyXQNBdRJ0PXPtXA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "768d9181-548f-4395-a8c2-7797c52e187f", "AQAAAAIAAYagAAAAEFnAq0igzpJJZ9zGL/+Dua1c7yYJspzsW7MMn0KzTWR9enG7b1Gw6YfKrYCLJZZgFw==" });
        }
    }
}
