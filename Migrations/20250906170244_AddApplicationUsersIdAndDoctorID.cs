using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUsersIdAndDoctorID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_AspNetUsers_ApplicationUserId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Consumables_Wards_WardID",
                table: "Consumables");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorAssignment_AspNetUsers_ApplicationUserId",
                table: "DoctorAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorAssignment_Patients_PatientID",
                table: "DoctorAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Patients_PatientID",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Consumables_WardID",
                table: "Consumables");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_ApplicationUserId",
                table: "Admissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorAssignment",
                table: "DoctorAssignment");

            migrationBuilder.DropIndex(
                name: "IX_DoctorAssignment_ApplicationUserId",
                table: "DoctorAssignment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "StockID",
                table: "WardConsumables");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TreatVisits");

            migrationBuilder.DropColumn(
                name: "SupplierItemID",
                table: "SupplierItems");

            migrationBuilder.DropColumn(
                name: "TakenItemID",
                table: "StockTakeItems");

            migrationBuilder.DropColumn(
                name: "PrescriptionInstruction",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "DeliveryItemID",
                table: "DeliveryItems");

            migrationBuilder.DropColumn(
                name: "WardID",
                table: "Consumables");

            migrationBuilder.DropColumn(
                name: "ConsumableOrderId",
                table: "ConsumableOrders");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Admissions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DoctorAssignment");

            migrationBuilder.RenameTable(
                name: "DoctorAssignment",
                newName: "DoctorAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorAssignment_PatientID",
                table: "DoctorAssignments",
                newName: "IX_DoctorAssignments_PatientID");

            migrationBuilder.AddColumn<string>(
                name: "ReasonForVisit",
                table: "TreatVisits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUsersId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorID",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdmissionId",
                table: "PatientAllergies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientAllergyId",
                table: "PatientAllergies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BedID",
                table: "Movements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WardID",
                table: "Movements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdmissionId",
                table: "MedicalHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Admissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorAssignments",
                table: "DoctorAssignments",
                column: "AssignmentID");

            migrationBuilder.InsertData(
                table: "Admissions",
                columns: new[] { "AdmisionID", "AdmissionDate", "BedID", "DischargeDate", "DoctorId", "Notes", "PatientID", "ReasonForAdmission", "WardID" },
                values: new object[] { 2, new DateOnly(2025, 9, 4), 2, null, 1, null, 3, "Patient was admitted for having severe migraine", 1 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d2435721-09e2-41cf-ac36-b98a6ad10763", "AQAAAAIAAYagAAAAEDZA9UqQWW2lt+DPL53onqRQ4JwCQUNJIPz0dTpYWGzkhsKdmQ+8RcmZFbyXJFTOKw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "aa2807dd-0944-459c-9d0c-86c94cbefb6a", "AQAAAAIAAYagAAAAEL13O/U8Z+nUI3ZyyuP+/Nupmz+r8fX01MfqEIGVWJ4dkadC59GD1kwx/rMCztnqtg==" });

            migrationBuilder.InsertData(
                table: "Beds",
                columns: new[] { "BedId", "BedNo", "IsDeleted", "IsOccupied", "WardID" },
                values: new object[,]
                {
                    { 3, "G3", false, false, 1 },
                    { 4, "C1", false, true, 2 }
                });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 9, 6, 19, 2, 42, 733, DateTimeKind.Local).AddTicks(7960));

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 1,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Medications",
                keyColumn: "MedicationId",
                keyValue: 2,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 1,
                columns: new[] { "ApplicationUsersId", "DoctorID" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 2,
                columns: new[] { "ApplicationUsersId", "DoctorID" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "Patients",
                keyColumn: "PatientID",
                keyValue: 3,
                columns: new[] { "ApplicationUsersId", "DoctorID" },
                values: new object[] { null, 0 });

            migrationBuilder.UpdateData(
                table: "TreatVisits",
                keyColumn: "TreatVisitID",
                keyValue: 1,
                column: "ReasonForVisit",
                value: "monitor patient recovery process");

            migrationBuilder.UpdateData(
                table: "TreatVisits",
                keyColumn: "TreatVisitID",
                keyValue: 2,
                column: "ReasonForVisit",
                value: "monitor patient temperature");

            migrationBuilder.InsertData(
                table: "Admissions",
                columns: new[] { "AdmisionID", "AdmissionDate", "BedID", "DischargeDate", "DoctorId", "Notes", "PatientID", "ReasonForAdmission", "WardID" },
                values: new object[] { 1, new DateOnly(2025, 9, 4), 4, null, 1, null, 2, "Surgery", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ApplicationUsersId",
                table: "Patients",
                column: "ApplicationUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_AdmissionId",
                table: "PatientAllergies",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_BedID",
                table: "Movements",
                column: "BedID");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_WardID",
                table: "Movements",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_AdmissionId",
                table: "MedicalHistories",
                column: "AdmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignments_DoctorID",
                table: "DoctorAssignments",
                column: "DoctorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_DoctorId",
                table: "Admissions",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorAssignments_AspNetUsers_DoctorID",
                table: "DoctorAssignments",
                column: "DoctorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorAssignments_Patients_PatientID",
                table: "DoctorAssignments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Admissions_AdmissionId",
                table: "MedicalHistories",
                column: "AdmissionId",
                principalTable: "Admissions",
                principalColumn: "AdmisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Beds_BedID",
                table: "Movements",
                column: "BedID",
                principalTable: "Beds",
                principalColumn: "BedId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Patients_PatientID",
                table: "Movements",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Wards_WardID",
                table: "Movements",
                column: "WardID",
                principalTable: "Wards",
                principalColumn: "WardID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Admissions_AdmissionId",
                table: "PatientAllergies",
                column: "AdmissionId",
                principalTable: "Admissions",
                principalColumn: "AdmisionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_ApplicationUsersId",
                table: "Patients",
                column: "ApplicationUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_AspNetUsers_DoctorId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorAssignments_AspNetUsers_DoctorID",
                table: "DoctorAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorAssignments_Patients_PatientID",
                table: "DoctorAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Admissions_AdmissionId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Beds_BedID",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Patients_PatientID",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Wards_WardID",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Admissions_AdmissionId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_ApplicationUsersId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ApplicationUsersId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_PatientAllergies_AdmissionId",
                table: "PatientAllergies");

            migrationBuilder.DropIndex(
                name: "IX_Movements_BedID",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_Movements_WardID",
                table: "Movements");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_AdmissionId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_Admissions_DoctorId",
                table: "Admissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorAssignments",
                table: "DoctorAssignments");

            migrationBuilder.DropIndex(
                name: "IX_DoctorAssignments_DoctorID",
                table: "DoctorAssignments");

            migrationBuilder.DeleteData(
                table: "Admissions",
                keyColumn: "AdmisionID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Admissions",
                keyColumn: "AdmisionID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "BedId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Beds",
                keyColumn: "BedId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ReasonForVisit",
                table: "TreatVisits");

            migrationBuilder.DropColumn(
                name: "ApplicationUsersId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DoctorID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AdmissionId",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "PatientAllergyId",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BedID",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "WardID",
                table: "Movements");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "AdmissionId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Admissions");

            migrationBuilder.RenameTable(
                name: "DoctorAssignments",
                newName: "DoctorAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorAssignments_PatientID",
                table: "DoctorAssignment",
                newName: "IX_DoctorAssignment_PatientID");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Wards",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockID",
                table: "WardConsumables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TreatVisits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SupplierItemID",
                table: "SupplierItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TakenItemID",
                table: "StockTakeItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionInstruction",
                table: "Prescriptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryItemID",
                table: "DeliveryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WardID",
                table: "Consumables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ConsumableOrderId",
                table: "ConsumableOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Admissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "DoctorAssignment",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorAssignment",
                table: "DoctorAssignment",
                column: "AssignmentID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8d706dd5-8f3f-4a93-81bd-74177db899ea", "AQAAAAIAAYagAAAAEMq9sXCxqVjYw+SGQOpBIPQtO9NYMRwxUizHOiIX7wqgrhnHQrLDku540gvvc3KlFQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b8bd5f0c-b6eb-46a3-b588-53186268d1e2", "AQAAAAIAAYagAAAAEFff+V3f7FIhrIvBb9yHp91/EqyQFysikeP0Po+CkZV6Bm4lHziy8wPmhoBfrPUYiA==" });

            migrationBuilder.UpdateData(
                table: "HospitalInfo",
                keyColumn: "HospitalInfoId",
                keyValue: 1,
                column: "LastUpdated",
                value: new DateTime(2025, 8, 12, 2, 20, 7, 617, DateTimeKind.Local).AddTicks(6738));

            migrationBuilder.UpdateData(
                table: "TreatVisits",
                keyColumn: "TreatVisitID",
                keyValue: 1,
                column: "IsCompleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "TreatVisits",
                keyColumn: "TreatVisitID",
                keyValue: 2,
                column: "IsCompleted",
                value: true);

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
                name: "IX_Consumables_WardID",
                table: "Consumables",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ApplicationUserId",
                table: "Admissions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignment_ApplicationUserId",
                table: "DoctorAssignment",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_ApplicationUserId",
                table: "Admissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumables_Wards_WardID",
                table: "Consumables",
                column: "WardID",
                principalTable: "Wards",
                principalColumn: "WardID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorAssignment_AspNetUsers_ApplicationUserId",
                table: "DoctorAssignment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorAssignment_Patients_PatientID",
                table: "DoctorAssignment",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Patients_PatientID",
                table: "Movements",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
