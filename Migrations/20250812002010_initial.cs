using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ONT_3rdyear_Project.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    AllergyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.AllergyId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    MedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.MedicationId);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChronicIllness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admitted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacies",
                columns: table => new
                {
                    PharmacyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacies", x => x.PharmacyID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuppliedDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardID);
                    table.ForeignKey(
                        name: "FK_Wards_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Discharges",
                columns: table => new
                {
                    DischargeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DischargeInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDischarged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discharges", x => x.DischargeID);
                    table.ForeignKey(
                        name: "FK_Discharges_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discharges_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "MedicalHistories",
                columns: table => new
                {
                    MedHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ChronicCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PastSurgicalHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecorderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConditonSeverity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistories", x => x.MedHistoryID);
                    table.ForeignKey(
                        name: "FK_MedicalHistories_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalHistories_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    MovementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    FromLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.MovementID);
                    table.ForeignKey(
                        name: "FK_Movements_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Movements_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergies",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    AllergyId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergies", x => new { x.PatientId, x.AllergyId });
                    table.ForeignKey(
                        name: "FK_PatientAllergies_Allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "Allergies",
                        principalColumn: "AllergyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientAllergies_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientAllergies_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DateIssued = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrescriptionInstruction = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptions_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescriptions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreatVisits",
                columns: table => new
                {
                    TreatVisitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: true),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatVisits", x => x.TreatVisitID);
                    table.ForeignKey(
                        name: "FK_TreatVisits_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatVisits_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID");
                });

            migrationBuilder.CreateTable(
                name: "VisitSchedules",
                columns: table => new
                {
                    VisitID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitSchedules", x => x.VisitID);
                    table.ForeignKey(
                        name: "FK_VisitSchedules_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitSchedules_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmacyMedications",
                columns: table => new
                {
                    PhamarcyMedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhamarcyID = table.Column<int>(type: "int", nullable: false),
                    MedicationID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Schedule = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedications", x => x.PhamarcyMedId);
                    table.ForeignKey(
                        name: "FK_PharmacyMedications_Medications_MedicationID",
                        column: x => x.MedicationID,
                        principalTable: "Medications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PharmacyMedications_Pharmacies_PhamarcyID",
                        column: x => x.PhamarcyID,
                        principalTable: "Pharmacies",
                        principalColumn: "PharmacyID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Beds",
                columns: table => new
                {
                    BedId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    BedNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beds", x => x.BedId);
                    table.ForeignKey(
                        name: "FK_Beds_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consumables",
                columns: table => new
                {
                    ConsumableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumables", x => x.ConsumableId);
                    table.ForeignKey(
                        name: "FK_Consumables_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderedBy = table.Column<int>(type: "int", nullable: false),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderQuantity = table.Column<int>(type: "int", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_OrderedBy",
                        column: x => x.OrderedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTakes",
                columns: table => new
                {
                    StockTakeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    StockTakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TakenBy = table.Column<int>(type: "int", nullable: false),
                    TakenByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakes", x => x.StockTakeID);
                    table.ForeignKey(
                        name: "FK_StockTakes_AspNetUsers_TakenByUserId",
                        column: x => x.TakenByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTakes_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrescribeMedications",
                columns: table => new
                {
                    PrescribedMedicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescribeMedications", x => x.PrescribedMedicationId);
                    table.ForeignKey(
                        name: "FK_PrescribeMedications_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrescribeMedications_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionForwardings",
                columns: table => new
                {
                    ForwardingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: true),
                    ForwardedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionForwardings", x => x.ForwardingID);
                    table.ForeignKey(
                        name: "FK_PrescriptionForwardings_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrescriptionForwardings_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionRejections",
                columns: table => new
                {
                    RejectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionID = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionRejections", x => x.RejectionID);
                    table.ForeignKey(
                        name: "FK_PrescriptionRejections_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrescriptionRejections_Prescriptions_PrescriptionID",
                        column: x => x.PrescriptionID,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Instructions",
                columns: table => new
                {
                    InstructionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    TreatVisitID = table.Column<int>(type: "int", nullable: true),
                    VisitID = table.Column<int>(type: "int", nullable: true),
                    NurseRequest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructions", x => x.InstructionID);
                    table.ForeignKey(
                        name: "FK_Instructions_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Instructions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instructions_TreatVisits_TreatVisitID",
                        column: x => x.TreatVisitID,
                        principalTable: "TreatVisits",
                        principalColumn: "TreatVisitID");
                    table.ForeignKey(
                        name: "FK_Instructions_VisitSchedules_VisitID",
                        column: x => x.VisitID,
                        principalTable: "VisitSchedules",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientMedicationScripts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    MedicationId = table.Column<int>(type: "int", nullable: false),
                    VisitID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PrescriptionId = table.Column<int>(type: "int", nullable: true),
                    AdministeredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientMedicationScripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientMedicationScripts_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientMedicationScripts_Medications_MedicationId",
                        column: x => x.MedicationId,
                        principalTable: "Medications",
                        principalColumn: "MedicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientMedicationScripts_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientMedicationScripts_Prescriptions_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptions",
                        principalColumn: "PrescriptionId");
                    table.ForeignKey(
                        name: "FK_PatientMedicationScripts_VisitSchedules_VisitID",
                        column: x => x.VisitID,
                        principalTable: "VisitSchedules",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Treat_VisitID = table.Column<int>(type: "int", nullable: true),
                    TreatmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TreatVisitID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK_Treatments_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Treatments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Treatments_TreatVisits_TreatVisitID",
                        column: x => x.TreatVisitID,
                        principalTable: "TreatVisits",
                        principalColumn: "TreatVisitID");
                    table.ForeignKey(
                        name: "FK_Treatments_VisitSchedules_VisitID",
                        column: x => x.VisitID,
                        principalTable: "VisitSchedules",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vitals",
                columns: table => new
                {
                    VitalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitID = table.Column<int>(type: "int", nullable: true),
                    ApplicationUserID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    BP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    SugarLevel = table.Column<double>(type: "float", nullable: false),
                    PulseRate = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TreatVisitID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vitals", x => x.VitalID);
                    table.ForeignKey(
                        name: "FK_Vitals_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vitals_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vitals_TreatVisits_TreatVisitID",
                        column: x => x.TreatVisitID,
                        principalTable: "TreatVisits",
                        principalColumn: "TreatVisitID");
                    table.ForeignKey(
                        name: "FK_Vitals_VisitSchedules_VisitID",
                        column: x => x.VisitID,
                        principalTable: "VisitSchedules",
                        principalColumn: "VisitID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    AdmisionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    WardID = table.Column<int>(type: "int", nullable: false),
                    BedID = table.Column<int>(type: "int", nullable: false),
                    AdmissionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DischargeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReasonForAdmission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.AdmisionID);
                    table.ForeignKey(
                        name: "FK_Admissions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Admissions_Beds_BedID",
                        column: x => x.BedID,
                        principalTable: "Beds",
                        principalColumn: "BedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Admissions_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierItems",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    SupplierItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityOnHand = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierItems", x => new { x.SupplierID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_SupplierItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierItems_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WardConsumables",
                columns: table => new
                {
                    WardID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    StockID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardConsumables", x => new { x.WardID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_WardConsumables_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WardConsumables_Wards_WardID",
                        column: x => x.WardID,
                        principalTable: "Wards",
                        principalColumn: "WardID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsumableOrders",
                columns: table => new
                {
                    ConsumableId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ConsumableOrderId = table.Column<int>(type: "int", nullable: false),
                    QuantityRequested = table.Column<int>(type: "int", nullable: false),
                    QuantityApproved = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumableOrders", x => new { x.ConsumableId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_ConsumableOrders_Consumables_ConsumableId",
                        column: x => x.ConsumableId,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumableOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredBy = table.Column<int>(type: "int", nullable: false),
                    DeliveredByUserId = table.Column<int>(type: "int", nullable: true),
                    RecievedBy = table.Column<int>(type: "int", nullable: false),
                    RecievedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryID);
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_DeliveredByUserId",
                        column: x => x.DeliveredByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_AspNetUsers_RecievedByUserId",
                        column: x => x.RecievedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Deliveries_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTakeItems",
                columns: table => new
                {
                    StockTakeID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    TakenItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityCounted = table.Column<int>(type: "int", nullable: false),
                    SystemQuantity = table.Column<int>(type: "int", nullable: false),
                    Discrepancy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakeItems", x => new { x.StockTakeID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_StockTakeItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTakeItems_StockTakes_StockTakeID",
                        column: x => x.StockTakeID,
                        principalTable: "StockTakes",
                        principalColumn: "StockTakeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryItems",
                columns: table => new
                {
                    DeliveryID = table.Column<int>(type: "int", nullable: false),
                    ConsumableID = table.Column<int>(type: "int", nullable: false),
                    DeliveryItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityDelivered = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryItems", x => new { x.DeliveryID, x.ConsumableID });
                    table.ForeignKey(
                        name: "FK_DeliveryItems_Consumables_ConsumableID",
                        column: x => x.ConsumableID,
                        principalTable: "Consumables",
                        principalColumn: "ConsumableId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryItems_Deliveries_DeliveryID",
                        column: x => x.DeliveryID,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleType", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "8d706dd5-8f3f-4a93-81bd-74177db899ea", "doctor@hospital.com", true, "Dr. John Doe", false, false, null, "DOCTOR@HOSPITAL.COM", "DOCTOR@HOSPITAL.COM", "AQAAAAIAAYagAAAAEMq9sXCxqVjYw+SGQOpBIPQtO9NYMRwxUizHOiIX7wqgrhnHQrLDku540gvvc3KlFQ==", null, false, "Doctor", null, false, "doctor@hospital.com" },
                    { 2, 0, "b8bd5f0c-b6eb-46a3-b588-53186268d1e2", "nurse@hospital.com", true, "Nurse Thabo", false, false, null, "NURSE@HOSPITAL.COM", "NURSE@HOSPITAL.COM", "AQAAAAIAAYagAAAAEFff+V3f7FIhrIvBb9yHp91/EqyQFysikeP0Po+CkZV6Bm4lHziy8wPmhoBfrPUYiA==", null, false, "Nurse", null, false, "nurse@hospital.com" }
                });

            migrationBuilder.InsertData(
                table: "HospitalInfo",
                columns: new[] { "HospitalInfoId", "Address", "Description", "DirectorName", "EmailAddress", "LastUpdated", "Logo", "Name", "Phone", "Website" },
                values: new object[] { 1, "123 Health Avenue, Cape Town, Western Cape, 8000", "Sunrise Medical Centre is a state-of-the-art healthcare facility offering comprehensive care, modern technology, and highly qualified staff.", "Dr. Lindiwe Mokoena", "info@sunrisemedical.co.za", new DateTime(2025, 8, 12, 2, 20, 7, 617, DateTimeKind.Local).AddTicks(6738), null, "Sunrise Medical centre", "+27 21 555 1234", "https://www.sunrisemedical.co.za" });

            migrationBuilder.InsertData(
                table: "Medications",
                columns: new[] { "MedicationId", "ExpiryDate", "IsDeleted", "Name", "Schedule" },
                values: new object[,]
                {
                    { 1, new DateOnly(2026, 1, 1), false, "Paracetamol", 1 },
                    { 2, new DateOnly(2025, 12, 1), false, "Insulin", 4 }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientID", "Admitted", "ChronicIllness", "DateOfBirth", "FirstName", "Gender", "IsDeleted", "LastName" },
                values: new object[,]
                {
                    { 1, false, "Hypertension", new DateOnly(2000, 1, 15), "Naledi", "Female", false, "Kgomo" },
                    { 2, true, "Diabetes", new DateOnly(1995, 6, 21), "Tshepo", "Male", false, "Mabasa" },
                    { 3, true, "Herpertension", new DateOnly(2003, 2, 28), "Thando", "Female", false, "Smith" }
                });

            migrationBuilder.InsertData(
                table: "Wards",
                columns: new[] { "WardID", "ApplicationUserId", "Capacity", "IsActive", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, null, 10, true, false, "General Ward" },
                    { 2, null, 10, true, false, "ICU" }
                });

            migrationBuilder.InsertData(
                table: "Beds",
                columns: new[] { "BedId", "BedNo", "IsDeleted", "IsOccupied", "WardID" },
                values: new object[,]
                {
                    { 1, "G1", false, false, 1 },
                    { 2, "G2", false, true, 1 }
                });

            migrationBuilder.InsertData(
                table: "TreatVisits",
                columns: new[] { "TreatVisitID", "ApplicationUserID", "IsCompleted", "Notes", "PatientID", "VisitDate" },
                values: new object[,]
                {
                    { 1, 1, false, "Initial wound dressing and IV fluid administered.", 1, new DateTime(2025, 9, 10, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, true, "Follow-up visit to monitor fever.", 2, new DateTime(2025, 8, 11, 10, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "VisitSchedules",
                columns: new[] { "VisitID", "ApplicationUserID", "Feedback", "IsActive", "NextVisit", "PatientID", "VisitDate" },
                values: new object[,]
                {
                    { 1, 1, "Initial checkup - stable condition.", true, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 9, 10, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Follow-up visit required medication adjustment.", true, new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Vitals",
                columns: new[] { "VitalID", "ApplicationUserID", "BP", "Date", "IsActive", "PatientID", "PulseRate", "SugarLevel", "Temperature", "TreatVisitID", "VisitID" },
                values: new object[,]
                {
                    { 1, 1, "120/80", new DateTime(2025, 7, 3, 9, 30, 0, 0, DateTimeKind.Utc), true, 1, 72, 5.5, 38.270000000000003, null, null },
                    { 2, 2, "130/85", new DateTime(2025, 7, 3, 10, 30, 0, 0, DateTimeKind.Utc), true, 2, 60, 7.2000000000000002, 37.119999999999997, null, null }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "InstructionID", "ApplicationUserID", "DateRecorded", "Instructions", "NurseRequest", "PatientID", "RespondedAt", "TreatVisitID", "VisitID" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), "Monitor vitls every 4 hours", "Please advise on wound management.", 1, null, null, 1 },
                    { 2, 2, new DateTime(2025, 11, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), "Administer antibiotics as prescribed.", "Please advise on wound management.", 2, null, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "PatientMedicationScripts",
                columns: new[] { "Id", "AdministeredDate", "ApplicationUserID", "Dosage", "MedicationId", "PatientId", "PrescriptionId", "VisitID", "isActive" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 14, 10, 30, 0, 0, DateTimeKind.Unspecified), 2, "2 tablets twice a day", 1, 1, null, 1, true },
                    { 2, new DateTime(2025, 7, 14, 11, 15, 0, 0, DateTimeKind.Unspecified), 2, "1 tablet daily after meal", 2, 2, null, 2, true }
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "TreatmentID", "ApplicationUserID", "IsActive", "PatientID", "TreatVisitID", "Treat_VisitID", "TreatmentDate", "TreatmentType", "VisitID" },
                values: new object[,]
                {
                    { 1, 2, true, 1, null, null, new DateTime(2025, 8, 8, 10, 0, 0, 0, DateTimeKind.Unspecified), "IV Drip setup", 1 },
                    { 2, 2, true, 2, null, null, new DateTime(2025, 10, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), "Wound Dressing", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ApplicationUserId",
                table: "Admissions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_BedID",
                table: "Admissions",
                column: "BedID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientID",
                table: "Admissions",
                column: "PatientID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_WardID",
                table: "Admissions",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Beds_WardID",
                table: "Beds",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableOrders_OrderId",
                table: "ConsumableOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumables_WardID",
                table: "Consumables",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DeliveredByUserId",
                table: "Deliveries",
                column: "DeliveredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_OrderID",
                table: "Deliveries",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_RecievedByUserId",
                table: "Deliveries",
                column: "RecievedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryItems_ConsumableID",
                table: "DeliveryItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_ApplicationUserID",
                table: "Discharges",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_PatientID",
                table: "Discharges",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignment_ApplicationUserId",
                table: "DoctorAssignment",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAssignment_PatientID",
                table: "DoctorAssignment",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_ApplicationUserID",
                table: "Instructions",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_PatientID",
                table: "Instructions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_TreatVisitID",
                table: "Instructions",
                column: "TreatVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_VisitID",
                table: "Instructions",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_ApplicationUserId",
                table: "MedicalHistories",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_ApplicationUserId",
                table: "Movements",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Movements_PatientID",
                table: "Movements",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedBy",
                table: "Orders",
                column: "OrderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SupplierID",
                table: "Orders",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WardID",
                table: "Orders",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_AllergyId",
                table: "PatientAllergies",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_ApplicationUserId",
                table: "PatientAllergies",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicationScripts_ApplicationUserID",
                table: "PatientMedicationScripts",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicationScripts_MedicationId",
                table: "PatientMedicationScripts",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicationScripts_PatientId",
                table: "PatientMedicationScripts",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicationScripts_PrescriptionId",
                table: "PatientMedicationScripts",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientMedicationScripts_VisitID",
                table: "PatientMedicationScripts",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedications_MedicationID",
                table: "PharmacyMedications",
                column: "MedicationID");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedications_PhamarcyID",
                table: "PharmacyMedications",
                column: "PhamarcyID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescribeMedications_MedicationId",
                table: "PrescribeMedications",
                column: "MedicationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescribeMedications_PrescriptionId",
                table: "PrescribeMedications",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionForwardings_ApplicationUserID",
                table: "PrescriptionForwardings",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionForwardings_PrescriptionID",
                table: "PrescriptionForwardings",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRejections_ApplicationUserID",
                table: "PrescriptionRejections",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionRejections_PrescriptionID",
                table: "PrescriptionRejections",
                column: "PrescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_ApplicationUserID",
                table: "Prescriptions",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_PatientId",
                table: "Prescriptions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakeItems_ConsumableID",
                table: "StockTakeItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakes_TakenByUserId",
                table: "StockTakes",
                column: "TakenByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakes_WardID",
                table: "StockTakes",
                column: "WardID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierItems_ConsumableID",
                table: "SupplierItems",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_ApplicationUserID",
                table: "Treatments",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientID",
                table: "Treatments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_TreatVisitID",
                table: "Treatments",
                column: "TreatVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_VisitID",
                table: "Treatments",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatVisits_ApplicationUserID",
                table: "TreatVisits",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_TreatVisits_PatientID",
                table: "TreatVisits",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitSchedules_ApplicationUserID",
                table: "VisitSchedules",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_VisitSchedules_PatientID",
                table: "VisitSchedules",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_ApplicationUserID",
                table: "Vitals",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_PatientID",
                table: "Vitals",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_TreatVisitID",
                table: "Vitals",
                column: "TreatVisitID");

            migrationBuilder.CreateIndex(
                name: "IX_Vitals_VisitID",
                table: "Vitals",
                column: "VisitID");

            migrationBuilder.CreateIndex(
                name: "IX_WardConsumables_ConsumableID",
                table: "WardConsumables",
                column: "ConsumableID");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_ApplicationUserId",
                table: "Wards",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admissions");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ConsumableOrders");

            migrationBuilder.DropTable(
                name: "DeliveryItems");

            migrationBuilder.DropTable(
                name: "Discharges");

            migrationBuilder.DropTable(
                name: "DoctorAssignment");

            migrationBuilder.DropTable(
                name: "HospitalInfo");

            migrationBuilder.DropTable(
                name: "Instructions");

            migrationBuilder.DropTable(
                name: "MedicalHistories");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientMedicationScripts");

            migrationBuilder.DropTable(
                name: "PharmacyMedications");

            migrationBuilder.DropTable(
                name: "PrescribeMedications");

            migrationBuilder.DropTable(
                name: "PrescriptionForwardings");

            migrationBuilder.DropTable(
                name: "PrescriptionRejections");

            migrationBuilder.DropTable(
                name: "StockTakeItems");

            migrationBuilder.DropTable(
                name: "SupplierItems");

            migrationBuilder.DropTable(
                name: "Treatments");

            migrationBuilder.DropTable(
                name: "Vitals");

            migrationBuilder.DropTable(
                name: "WardConsumables");

            migrationBuilder.DropTable(
                name: "Beds");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Pharmacies");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "StockTakes");

            migrationBuilder.DropTable(
                name: "TreatVisits");

            migrationBuilder.DropTable(
                name: "VisitSchedules");

            migrationBuilder.DropTable(
                name: "Consumables");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
