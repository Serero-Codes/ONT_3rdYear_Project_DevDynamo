using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class changeTemperatureDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Vitals",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

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

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 1,
                column: "Temperature",
                value: 38.270000000000003);

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 2,
                column: "Temperature",
                value: 37.119999999999997);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "Vitals",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 1,
                column: "Temperature",
                value: 38m);

            migrationBuilder.UpdateData(
                table: "Vitals",
                keyColumn: "VitalID",
                keyValue: 2,
                column: "Temperature",
                value: 37m);
        }
    }
}
