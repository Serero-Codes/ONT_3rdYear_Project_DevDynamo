using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddRespondedAtToInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VisitID",
                table: "Instructions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "RespondedAt",
                table: "Instructions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "852b574b-7e59-4da5-b886-3ed0758ccf7d", "AQAAAAIAAYagAAAAEK40yKvyK4AkEXlQkR9c95MWUmBV69w8BhwTfEpICS0aRdQfipnztqmgjr5lH8TtEw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "73bb3dd1-4391-4bbf-8df1-222103ae2e8b", "AQAAAAIAAYagAAAAEFpNsMZ3vt97zzwoYcQOdsD6sKHwGbbq32BNFoHaZIQHH5u+8w4Elf0rtS1z9WrJQg==" });

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 1,
                column: "RespondedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 2,
                column: "RespondedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespondedAt",
                table: "Instructions");

            migrationBuilder.AlterColumn<int>(
                name: "VisitID",
                table: "Instructions",
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
                values: new object[] { "15cded3e-42a1-4a66-a9ae-b486994595b1", "AQAAAAIAAYagAAAAEO1GLckJRphOyxKJ8TlLircwnivZDTJbs6OM/rlA2XJUDdrU/2EqzqubNdgeiXI7DA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "5e90a9b9-bb8b-4504-856d-67fa5ad7f59d", "AQAAAAIAAYagAAAAEN5WHvcuilTqseYIZ3ECuvWaFImiwwKqvQ+OYB60m5gtuqarPMPntAAmvKQ/08nhiA==" });
        }
    }
}
