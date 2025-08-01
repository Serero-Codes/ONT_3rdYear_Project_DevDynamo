using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSeedPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientID", "Admitted", "ChronicMed", "DateOfBirth", "FirstName", "Gender", "LastName" },
                values: new object[] { 3, true, "Herpertension", new DateOnly(2003, 2, 28), "Thando", "Female", "Smith" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2b8cb733-b045-40a8-af6f-7b2ef9395d92", "AQAAAAIAAYagAAAAENoJ6Ze76iMYLzV0RhD5cIeKopzjPHW9C2w4sPW6r296+MEpdEM40Dg1MiLSRWdnMQ==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a8d430f2-5b95-436e-a145-7a5b198ecb11", "AQAAAAIAAYagAAAAEEf69SuCvzkMTgmFyHfps8/c7FE8QXFnlUH5AG4WKFWVKvjUn9+izMuNNTtUIYvVWw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "df8a1caf-a3ee-4822-a0b4-14869f4cda2a", "AQAAAAIAAYagAAAAEHMsrc164vPTerAR8fYEaFsbtboTYbgCwVH1cKfIbmVJPSy3Yjk0MloN2e6ie3mILw==" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d34d6ae6-18bd-4947-a11c-81ebfe0658e8", "AQAAAAIAAYagAAAAEKV+Y7h34VVZEBOS+hefb3ZlS+r8PNrky1JZaNBhjx+8A38oGHOpKdBT5kYw1Beoqw==" });
        }
    }
}
