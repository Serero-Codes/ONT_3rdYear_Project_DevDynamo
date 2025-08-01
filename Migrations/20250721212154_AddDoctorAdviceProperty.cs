using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAdviceProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorAdvice",
                table: "Instructions");

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
        }
    }
}
