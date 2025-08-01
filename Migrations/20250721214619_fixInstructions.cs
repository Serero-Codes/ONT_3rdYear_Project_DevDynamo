using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class fixInstructions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorAdvice",
                table: "Instructions");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NurseRequest",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "95108f9b-c84f-4b8d-a27f-257cf79fb79d", "AQAAAAIAAYagAAAAEM8wfH9vXnroLnKYABH1e4/vI8PuVrrcCd+jGD67DiuW32eAlXTdg8ePUgVpDH4YPQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "70e30c36-d47b-4ee1-a270-471971227941", "AQAAAAIAAYagAAAAEOBNKLmf9055w0+svSczbYyDX+rn0lOCJ5hmMzzTaLPRgG7jOmVYATtnuYreUYlGhA==" });

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 1,
                column: "NurseRequest",
                value: "Please advise on wound management.");

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 2,
                column: "NurseRequest",
                value: "Please advise on wound management.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NurseRequest",
                table: "Instructions");

            migrationBuilder.AlterColumn<string>(
                name: "Instructions",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorAdvice",
                table: "Instructions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a27a95d6-a16d-41e3-ab36-dff5ec2458a1", "AQAAAAIAAYagAAAAECS48BcxeFtJ5HXhNYRyt71VvrkQVB4B1inh70g6P5LsjPsz1lhMekKoivy6QATJ6Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "129e85a1-aa8a-4ec2-a72a-5bbf08d1486f", "AQAAAAIAAYagAAAAEESbak1CkSgNnJs+5BwPR8suNwf2UriXzhIQnWebnBv+2PsrEfvnY4+T2vOod2w/pQ==" });

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 1,
                column: "DoctorAdvice",
                value: null);

            migrationBuilder.UpdateData(
                table: "Instructions",
                keyColumn: "InstructionID",
                keyValue: 2,
                column: "DoctorAdvice",
                value: null);
        }
    }
}
