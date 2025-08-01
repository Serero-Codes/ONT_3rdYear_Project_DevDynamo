using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class makevisitnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisitID",
                table: "Treatments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "15cded3e-42a1-4a66-a9ae-b486994595b1", "AQAAAAIAAYagAAAAEO1GLckJRphOyxKJ8TlLircwnivZDTJbs6OM/rlA2XJUDdrU/2EqzqubNdgeiXI7DA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5e90a9b9-bb8b-4504-856d-67fa5ad7f59d", "AQAAAAIAAYagAAAAEN5WHvcuilTqseYIZ3ECuvWaFImiwwKqvQ+OYB60m5gtuqarPMPntAAmvKQ/08nhiA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisitID",
                table: "Treatments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c2635863-8c0c-4062-86b3-bbf73cd8e6dc", "AQAAAAIAAYagAAAAEP1P8FLHGix8YVkBJjzV99JUHcJu1JpiN/4AXFqrc7YUC5H3Ojmi9lHOLzKKwQ62QQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3e221ca1-f254-4299-8847-5f16c1148af2", "AQAAAAIAAYagAAAAEHcy3HZ63Pfllp18MZXHDS7Imal5pinZ5C45En9aHRRBjP4QBIzcYoBLEu6RROGM/A==" });
        }
    }
}
