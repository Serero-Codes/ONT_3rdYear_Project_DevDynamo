using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityToMedication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3845b0d7-e089-42e6-81e6-579592a940ff", "AQAAAAIAAYagAAAAEOqIBA/twpwCLmRzFH1CkbEbaT86ku7AnHXV5sO1MW9b4oXq0Q9Oi7D20nRvIh50Jg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "35a54372-faca-4bbe-b562-671ed3d3aea3", "AQAAAAIAAYagAAAAEFg5hRvSGUcjbBL3JAXDWnlNcw4WChkH4KB97zTINMEBNqlEvA//CJrZOlMgOrhdsg==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 14, 10, 39, 47, 610, DateTimeKind.Local).AddTicks(8136));

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 1,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 2,
                column: "Quantity",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Medications");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "df6e8e6f-c18b-4ce9-b35a-cd1011163c57", "AQAAAAIAAYagAAAAEKg06sboeX1lYLioVwz+b2/ph9HWD7f8DPpob0xx3FqhHEuUnZrEEtJAekzOkLhFBw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8ed86d59-2e1d-4bc9-aeca-2eac38844a52", "AQAAAAIAAYagAAAAEAowxTDkhJVYmnOe3llbONgSGjiAWP9GYxvzQSrxWj9fRVU63PfWQCepfkMWVJBRXw==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 14, 10, 33, 55, 592, DateTimeKind.Local).AddTicks(308));
        }
    }
}
