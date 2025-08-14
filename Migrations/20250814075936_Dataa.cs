using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class Dataa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Wards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Consumables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Beds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Allergies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DischargeDate",
                table: "Admissions",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DoctorAssignment",
                columns: table => new
                {
                    AssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    AssignmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UnassignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAssignment", x => x.AssignmentID);
                    table.ForeignKey(
                        name: "FK_DoctorAssignment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DoctorAssignment_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HospitalInfo",
                columns: table => new
                {
                    HospitalInfoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalInfo", x => x.HospitalInfoId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "IsDeleted", "PasswordHash" },
                values: new object[] { "bbdf19e1-22a2-4a92-8870-8b929d5ce64c", false, "AQAAAAIAAYagAAAAEBF45mMwQRlaJeshwcAELh/gKTAVD0jigECQrCC5hKMLm/EnMnrFgoU0wYXwP+Karw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "IsDeleted", "PasswordHash" },
                values: new object[] { "9859e6f9-ccce-4e1a-93ef-254f940d73c0", false, "AQAAAAIAAYagAAAAEEYyIFt4sf8G7vVZA9DoCiZDZvec+odTvOfZ1FlAs+YPg1lD13wJ7luku5WQm3YuJA==" });

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "BedId",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Beds",
                keyColumn: "BedId",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.InsertData(
                table: "HospitalInfo",
                columns: new[] { "HospitalInfoId", "Address", "Description", "DirectorName", "EmailAddress", "LastUpdated", "Logo", "Name", "Phone", "Website" },
                values: new object[] { 1, "123 Health Avenue, Cape Town, Western Cape, 8000", "Sunrise Medical Centre is a state-of-the-art healthcare facility offering comprehensive care, modern technology, and highly qualified staff.", "Dr. Lindiwe Mokoena", "info@sunrisemedical.co.za", new DateTime(2025, 8, 14, 9, 59, 34, 396, DateTimeKind.Local).AddTicks(7016), null, "Sunrise Medical centre", "+27 21 555 1234", "https://www.sunrisemedical.co.za" });

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 3,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "WardID",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "WardID",
                keyValue: 2,
                column: "IsDeleted",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignment_ApplicationUserId",
                table: "DoctorAssignment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignment_PatientID",
                table: "DoctorAssignment",
                column: "PatientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAssignment");

            migrationBuilder.DropTable(
                name: "HospitalInfo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Consumables");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Beds");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Allergies");

            migrationBuilder.DropColumn(
                name: "DischargeDate",
                table: "Admissions");

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
        }
    }
}
