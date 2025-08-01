using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class ddRoleTypeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_User_ApplicationUserId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_User_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_User_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_User_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_User_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_User_ApplicationUserID",
                table: "Discharges");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_User_ApplicationUserID",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_User_ApplicationUserId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_User_ApplicationUserId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_User_ApplicationUserId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicationScripts_User_ApplicationUserID",
                table: "PatientMedicationScripts");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionForwardings_User_ApplicationUserID",
                table: "PrescriptionForwardings");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionRejections_User_ApplicationUserID",
                table: "PrescriptionRejections");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_User_ApplicationUserID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_User_ApplicationUserID",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatVisits_User_ApplicationUserID",
                table: "TreatVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitSchedules_User_ApplicationUserID",
                table: "VisitSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Vitals_User_ApplicationUserID",
                table: "Vitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_User_ApplicationUserId",
                table: "Wards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "AspNetUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_AspNetUsers_ApplicationUserId",
                table: "Admissions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_AspNetUsers_ApplicationUserID",
                table: "Discharges",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_AspNetUsers_ApplicationUserID",
                table: "Instructions",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_AspNetUsers_ApplicationUserId",
                table: "MedicalHistories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_AspNetUsers_ApplicationUserId",
                table: "Movements",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_AspNetUsers_ApplicationUserId",
                table: "PatientAllergies",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicationScripts_AspNetUsers_ApplicationUserID",
                table: "PatientMedicationScripts",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionForwardings_AspNetUsers_ApplicationUserID",
                table: "PrescriptionForwardings",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionRejections_AspNetUsers_ApplicationUserID",
                table: "PrescriptionRejections",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_AspNetUsers_ApplicationUserID",
                table: "Prescriptions",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_AspNetUsers_ApplicationUserID",
                table: "Treatments",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatVisits_AspNetUsers_ApplicationUserID",
                table: "TreatVisits",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitSchedules_AspNetUsers_ApplicationUserID",
                table: "VisitSchedules",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vitals_AspNetUsers_ApplicationUserID",
                table: "Vitals",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_AspNetUsers_ApplicationUserId",
                table: "Wards",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admissions_AspNetUsers_ApplicationUserId",
                table: "Admissions");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_AspNetUsers_ApplicationUserID",
                table: "Discharges");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_AspNetUsers_ApplicationUserID",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_AspNetUsers_ApplicationUserId",
                table: "MedicalHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Movements_AspNetUsers_ApplicationUserId",
                table: "Movements");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_AspNetUsers_ApplicationUserId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedicationScripts_AspNetUsers_ApplicationUserID",
                table: "PatientMedicationScripts");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionForwardings_AspNetUsers_ApplicationUserID",
                table: "PrescriptionForwardings");

            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionRejections_AspNetUsers_ApplicationUserID",
                table: "PrescriptionRejections");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_AspNetUsers_ApplicationUserID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_AspNetUsers_ApplicationUserID",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatVisits_AspNetUsers_ApplicationUserID",
                table: "TreatVisits");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitSchedules_AspNetUsers_ApplicationUserID",
                table: "VisitSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Vitals_AspNetUsers_ApplicationUserID",
                table: "Vitals");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_AspNetUsers_ApplicationUserId",
                table: "Wards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Admissions_User_ApplicationUserId",
                table: "Admissions",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_User_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_User_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_User_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_User_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_User_ApplicationUserID",
                table: "Discharges",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_User_ApplicationUserID",
                table: "Instructions",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_User_ApplicationUserId",
                table: "MedicalHistories",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_User_ApplicationUserId",
                table: "Movements",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_User_ApplicationUserId",
                table: "PatientAllergies",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedicationScripts_User_ApplicationUserID",
                table: "PatientMedicationScripts",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionForwardings_User_ApplicationUserID",
                table: "PrescriptionForwardings",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionRejections_User_ApplicationUserID",
                table: "PrescriptionRejections",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_User_ApplicationUserID",
                table: "Prescriptions",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_User_ApplicationUserID",
                table: "Treatments",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatVisits_User_ApplicationUserID",
                table: "TreatVisits",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitSchedules_User_ApplicationUserID",
                table: "VisitSchedules",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vitals_User_ApplicationUserID",
                table: "Vitals",
                column: "ApplicationUserID",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_User_ApplicationUserId",
                table: "Wards",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
