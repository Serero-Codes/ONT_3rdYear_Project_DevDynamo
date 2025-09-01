using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "47ad81f4-dc76-4d75-b3f2-771d6e1958f1", "AQAAAAIAAYagAAAAEB0uLzkNSjHdVMl9uq10sGE3ZQOVlKytGhyniP3dBOSu90DdMeD9vXWOIM79hhXt6w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fbbc28e7-a4ee-495e-979d-eb81b9abe654", "AQAAAAIAAYagAAAAEDC8w7rJi1x6njYffv+qH9aiBu3VoTL5GEHAHUhzxF7IscK5HintPOBwuaT86CEZeQ==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 18, 19, 17, 56, 607, DateTimeKind.Local).AddTicks(3966));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

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
    }
}
