using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockID",
                table: "WardConsumables");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockID",
                table: "WardConsumables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d7be20d6-1774-491b-a2f6-f739b5703dcd", "AQAAAAIAAYagAAAAEKja79inF50Jb+k7SWFy5/tN/pan8YQYeIs1lqF5i4ypZL01RP1/pt3dVlZXb6CjPw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9788826f-d363-45fe-9646-e6260cdcdefd", "AQAAAAIAAYagAAAAEFlznK3GN0mWOrXF9Ordni0gxVYuJxdLYfnBdt/eET4fZiohztOGlAFl+bMWjg4FUA==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 18, 1, 15, 53, 612, DateTimeKind.Local).AddTicks(5568));
        }
    }
}
