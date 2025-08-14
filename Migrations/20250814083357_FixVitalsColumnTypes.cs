using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class FixVitalsColumnTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bbdf19e1-22a2-4a92-8870-8b929d5ce64c", "AQAAAAIAAYagAAAAEBF45mMwQRlaJeshwcAELh/gKTAVD0jigECQrCC5hKMLm/EnMnrFgoU0wYXwP+Karw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9859e6f9-ccce-4e1a-93ef-254f940d73c0", "AQAAAAIAAYagAAAAEEYyIFt4sf8G7vVZA9DoCiZDZvec+odTvOfZ1FlAs+YPg1lD13wJ7luku5WQm3YuJA==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 14, 9, 59, 34, 396, DateTimeKind.Local).AddTicks(7016));
        }
    }
}
