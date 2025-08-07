using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class changePulseandSugarType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SugarLevel",
                table: "Vitals",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PulseRate",
                table: "Vitals",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e5bd254d-c3f0-4a80-8984-6269c8f97df7", "AQAAAAIAAYagAAAAEJJn/ewR2YLCwugGhaZ2NarWxaDpj5XvibfIGY5GJNvzQ4qGAsJ8314dIE22bRP7Yw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "20811d85-dae9-437f-972e-f2bb64c50fc2", "AQAAAAIAAYagAAAAEPzag/ol9o7n+fjiwNddmoC8/5c7j7GB+Gew5F59r+0cmuhjwx6hpcuUlgqywC9ehg==" });

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 1,
                columns: new[] { "PulseRate", "SugarLevel" },
                values: new object[] { 72, 5.5 });

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 2,
                columns: new[] { "PulseRate", "SugarLevel" },
                values: new object[] { 60, 7.2000000000000002 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SugarLevel",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "PulseRate",
                table: "Vitals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

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
                columns: new[] { "PulseRate", "SugarLevel" },
                values: new object[] { "72bpm", "72/80" });

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 2,
                columns: new[] { "PulseRate", "SugarLevel" },
                values: new object[] { "60bpm", "78/89" });
        }
    }
}
