using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;
using System.Security.Claims;

namespace ONT_3rdyear_Project.Controllers
{
    [Authorize(Roles = "WardAdmin")]
    public class WardAdminController : Controller
    {
            private readonly ApplicationDbContext _context;

            public WardAdminController(ApplicationDbContext context)
            {
                _context = context;
            }
        public IActionResult Index()
        {
            return View();
        }
        // Search for patients
        [HttpPost]
        public async Task<IActionResult> Search(string query)
        {
            var patients = await _context.Patients
                .Where(p => !p.IsDeleted &&
                           (p.FirstName.Contains(query) || p.LastName.Contains(query)))
                .Select(p => new
                {
                    p.PatientID,
                    FullName = p.FirstName + " " + p.LastName,
                    p.Gender,
                    p.DateOfBirth,
                    IsAdmitted = p.Admissions != null && p.Admissions.DischargeDate == null
                })
                .OrderBy(p => p.FullName)
                .ToListAsync();

            return View("SearchResults", patients);
        }

        // Get All patients
        public async Task<IActionResult> AllPatients()
            {
                var patients = await _context.Patients
                    .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                    .Include(p => p.DoctorAssignments.Where(da => da.IsActive))
                    .ThenInclude(da => da.ApplicationUser)
                    .ToListAsync();

                return View(patients);
            }

            // Create patient info
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Patient added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // Edit patient info
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null || patient.IsDeleted)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Update(patient);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Patient edited successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // Soft delete
        public async Task<IActionResult> SoftDelete(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost, ActionName("SoftDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            patient.IsDeleted = true;
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Patient deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // View patient details
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Admissions)
                .ThenInclude(a => a.Bed)
                .ThenInclude(b => b.Ward)
                .Include(p => p.DoctorAssignments.Where(da => da.IsActive))
                .ThenInclude(da => da.ApplicationUser)
                .Include(p => p.MedicalHistories)
                .Include(p => p.PatientAllergies)
                .ThenInclude(pa => pa.Allergy)
                .FirstOrDefaultAsync(p => p.PatientID == id);

            if (patient == null || patient.IsDeleted)
            {
                return NotFound();
            }

            return View(patient);
        }

            // Get admit patient page
            public IActionResult AdmitPatient()
            {
                ViewBag.Patients = _context.Patients
                    .Where(p => !p.IsDeleted && (p.Admissions == null || p.Admissions.DischargeDate != null))
                    .Select(p => new { p.PatientID, FullName = p.FirstName + " " + p.LastName })
                    .ToList();

                ViewBag.Beds = _context.Beds
                    .Where(b => !b.IsOccupied)
                    .Select(b => new { b.BedId, BedName = b.BedNo + " - Ward " + b.Ward.Name })
                    .ToList();
                ViewBag.Wards = _context.Wards
                    .Where(w => w.IsActive)
                    .Select(w => new { w.WardID, WardName = w.Name})
                    .ToList();

                ViewBag.Doctors = _context.Users
                    .Where(u => u.RoleType == "Doctor")
                    .Select(d => new { d.Id, d.FullName })
                    .ToList();

                return View();
            }

            // Admit Patient
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AdmitPatient(Patient patient, int bedId, int doctorId, string reason, string Notes)
            {
                if (ModelState.IsValid)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {               
                        // Assign bed
                        var bed = await _context.Beds.FindAsync(bedId);
                        if (bed == null || bed.IsOccupied)
                        {
                            ModelState.AddModelError("", "Bed is already occupied or invalid");
                            ViewBag.Beds = _context.Beds.Where(b => !b.IsOccupied).Include(b => b.Ward).ToList();
                            ViewBag.Doctors = _context.Users.Where(u => u.RoleType == "Doctor").ToList();
                            return View(patient);
                        }

                        // Validate doctor
                        var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == doctorId && u.RoleType == "Doctor");
                        if (doctor == null)
                        {
                            ModelState.AddModelError("", "Invalid doctor selected");
                            ViewBag.Beds = _context.Beds.Where(b => !b.IsOccupied).Include(b => b.Ward).ToList();
                            ViewBag.Doctors = _context.Users.Where(u => u.RoleType == "Doctor").ToList();
                            return View(patient);
                        }

                        // Add patient
                        _context.Add(patient);
                        await _context.SaveChangesAsync();

                        // Create admission record
                        var admission = new Admission
                        {
                            PatientID = patient.PatientID,
                            BedID = bedId,
                            WardID = bed.WardID,
                            AdmissionDate = DateOnly.FromDateTime(DateTime.Now),
                            ReasonForAdmission = reason,
                            Notes = Notes
                        };

                        // Create doctor assignment
                        var doctorAssignment = new DoctorAssignment
                        {
                            DoctorID = doctorId,
                            PatientID = patient.PatientID,
                            AssignmentDate = DateTime.Now,
                            IsActive = true
                        };

                        // Update bed status
                        bed.IsOccupied = true;

                        _context.Add(admission);
                        _context.Add(doctorAssignment);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        TempData["Success"] = "Patient admitted successfully";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", $"Error admitting patient: {ex.Message}");
                    }
                }

                ViewBag.Beds = _context.Beds.Where(b => !b.IsOccupied).Include(b => b.Ward).ToList();
                ViewBag.Doctors = _context.Users.Where(u => u.RoleType == "Doctor").ToList();
                return View(patient);
            }

        public async Task<IActionResult> ActiveAdmission()
        {
            var admissions = await _context.Admissions
                .Include(a => a.ApplicationUser)
                .Include(a => a.Patient)
                .Include(a => a.Bed)                
                .ThenInclude(b => b.Ward)
                .Where(a => a.DischargeDate == null)
                .OrderByDescending(a => a.AdmissionDate)
                .ToListAsync();

            return View(admissions);
        }

        public async Task<IActionResult> PatientAdmission(int patientId)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.PatientID == patientId);

            if (patient == null || patient.IsDeleted)
            {
                return NotFound();
            }

            var admissions = await _context.Admissions
                .Include(a => a.Bed)
                .ThenInclude(b => b.Ward)
                .Where(a => a.PatientID == patientId)
                .OrderByDescending(a => a.AdmissionDate)
                .ToListAsync();

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            return View(admissions);
        }

        [Authorize(Roles = "WardAdmin")]
        public async Task<IActionResult> DischargePatientList()
        {
            var admittedPatients = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Bed)
                .Include(a => a.Ward)
                .Where(a => a.DischargeDate == null)
                .ToListAsync();

            return View(admittedPatients);
        }

        // Discharge patient
        [HttpPost]
            [Authorize(Roles = "WardAdmin")]
            public async Task<IActionResult> Discharge(int patientId, string instructions)
            {
                if (string.IsNullOrWhiteSpace(instructions))
                {
                    TempData["Error"] = "Discharge instructions are required";
                    return RedirectToAction("DischargePatientList");
                }

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var admission = await _context.Admissions
                        .Include(a => a.Bed)
                        .FirstOrDefaultAsync(a => a.PatientID == patientId);

                    if (admission == null)
                    {
                        TempData["Error"] = "Patient is not currently admitted";
                        return RedirectToAction("DischargePatientList");
                    }

                    // Get current user ID
                    var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                    // Update admission
                    admission.DischargeDate = DateOnly.FromDateTime(DateTime.Now);

                    // Create discharge record
                    var dischargeRecord = new Discharge
                    {
                        PatientID = patientId,
                        ApplicationUserID = currentUserId,
                        DischargeDate = DateTime.Now,
                        DischargeInstructions = instructions,
                        IsDischarged = true
                    };

                    // Free bed
                    admission.Bed.IsOccupied = false;

                    // Deactivate doctor assignments
                    var activeAssignments = await _context.Set<DoctorAssignment>()
                        .Where(da => da.PatientID == patientId && da.IsActive)
                        .ToListAsync();

                    foreach (var assignment in activeAssignments)
                    {
                        assignment.IsActive = false;
                        assignment.UnassignedDate = DateTime.Now;
                    }

                    _context.Add(dischargeRecord);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["Success"] = "Patient discharged successfully";
                    return RedirectToAction("DischargePatientList");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = $"Error discharging patient: {ex.Message}";
                    return RedirectToAction("DischargePatientList");
                }
            }

        public async Task<IActionResult> ViewPatients()
        {
            var admittedPatients = await _context.Admissions
               .Include(a => a.Patient)
               .Include(a => a.Ward)
               .Include(a => a.Bed)
               .Where(a => a.DischargeDate == null) // only active admissions
               .ToListAsync();

            return View(admittedPatients);
        }
        //show patient movement log form
        public async Task<IActionResult> LogMovement(int patientId)
        {
            var admission = await _context.Admissions
                .Include(a=>a.Patient)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .FirstOrDefaultAsync(a => a.PatientID == patientId && a.DischargeDate == null);

            if (admission == null)
            {
                return NotFound();
            }

            ViewBag.PatientID = admission.PatientID;
            ViewBag.PatientName = $"{admission.Patient.FirstName} {admission.Patient.LastName}";
            ViewBag.CurrentLocation = $"Ward: {admission.Ward.Name}, Bed: {admission.Bed.BedNo}";
            return View();
        }
        // Record patient movement
        [HttpPost]
            public async Task<IActionResult> LogMovement(Movement model)
            {
                if ( string.IsNullOrWhiteSpace(model.ToLocation) || string.IsNullOrWhiteSpace(model.FromLocation))
                {
                    return Json(new { success = false, message = "From and To locations are required" });
                }

                try
                {
                    var admission = await _context.Admissions
                                                .Include(a => a.Ward)
                                                .Include(a => a.Bed)
                                                .FirstOrDefaultAsync(a => a.PatientID == model.PatientID && a.DischargeDate == null);
                    if (admission == null)
                    {
                        return Json(new { success = false, message = "Patient is not currently admitted" });
                    }
                    var fromLocation = $" ward: {admission.Ward.Name} - Bed: {admission.Bed.BedNo}";
                    var movement = new Movement
                    {
                        PatientID = model.PatientID,
                        WardID = admission.WardID,
                        BedID = admission.BedID,
                        FromLocation = fromLocation,
                        ToLocation = model.ToLocation,
                        TimeStamp = model.TimeStamp == default ? DateTime.Now : model.TimeStamp,
                    };

                    _context.Add(movement);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Movement logged successfully";
                return RedirectToAction("MovementHistory", new {patientId = model.PatientID});
                }
                catch (Exception ex)
                {
                    TempData["Error Message"] = $"Error logging movement: {ex.Message}";
                    return RedirectToAction("LogMovement", new { patientId = model.PatientID });
                }
               
            }

            // View movement history
            public async Task<IActionResult> MovementHistory(int patientId)
            {
                var patient = await _context.Patients.FindAsync(patientId);
                if (patient == null)
                {
                    return NotFound();
                }

                var movements = await _context.Movements
                    .Where(m => m.PatientID == patientId)
                    .OrderByDescending(m => m.TimeStamp)
                    .ToListAsync();

                ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
                return View(movements);
            }

        public async Task<IActionResult> EditMovement(int id)
        {


            var movement = await _context.Movements
                                         .Include(m => m.Patient)
                                         .FirstOrDefaultAsync(m => m.MovementID == id);

            if (movement == null)
            {
                return NotFound();
            }
            var admission = await _context.Admissions
                                     .Include(a => a.Ward)
                                     .Include(a => a.Bed)
                                     .FirstOrDefaultAsync(a => a.PatientID == movement.PatientID && a.DischargeDate == null);

            ViewBag.WardLocation = admission != null
                ? $"Ward: {admission.Ward.Name} - Bed: {admission.Bed.BedNo}"
                : "Ward";

            return View(movement);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovement(Movement updated)
        {
            if (!ModelState.IsValid)
            {
                return View(updated);
            }

            var movement = await _context.Movements.FindAsync(updated.MovementID);
            if (movement == null)
            {
                return NotFound();
            }
            movement.ToLocation = updated.ToLocation;
            movement.TimeStamp = updated.TimeStamp;

            _context.Update(updated);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Movement updated successfully!";
            return RedirectToAction("MovementHistory", new { patientId = updated.PatientID });
        }

        // Get patient medical history
        public async Task<IActionResult> GetMedicalHistory(int patientId)
            {
                var history = await _context.MedicalHistories
                    .Where(m => m.PatientId == patientId)
                    .OrderByDescending(m => m.RecorderDate)
                    .ToListAsync();

                return PartialView("_MedicalHistoryPartial", history);
            }

            // Form to add medical history
            public IActionResult AddMedicalHistory(int patientId)
            {
                ViewBag.PatientId = patientId;
                return PartialView("_AddMedicalHistoryPartial");
            }

            // Save medical history
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddMedicalHistory(MedicalHistory model)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.RecorderDate = DateTime.Now;
                        _context.MedicalHistories.Add(model);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = "Medical history added successfully" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = $"Error adding medical history: {ex.Message}" });
                    }
                }

                ViewBag.PatientId = model.PatientId;
                return PartialView("_AddMedicalHistoryPartial", model);
            }

        public async Task<IActionResult> EditMedicalHistory(int id)
        {
            var history = await _context.MedicalHistories.FindAsync(id);
            if (history == null)
                return NotFound();

            return PartialView("_EditMedicalHistoryPartial", history);
        }

        // POST: Edit Medical History
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedicalHistory(MedicalHistory model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.MedicalHistories.Update(model);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Medical history updated successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error updating medical history: {ex.Message}" });
                }
            }
            return PartialView("_EditMedicalHistoryPartial", model);
        }

        // Get patient allergies
        public async Task<IActionResult> GetPatientAllergies(int patientId)
            {
                var allergies = await _context.PatientAllergies
                    .Include(pa => pa.Allergy)
                    .Where(pa => pa.PatientId == patientId)
                    .ToListAsync();

                return PartialView("_AllergyPartial", allergies);
            }

            // Form to add allergy
            public IActionResult AddAllergy(int patientId)
            {
                ViewBag.PatientId = patientId;
                ViewBag.Allergies = _context.Allergies.ToList();
                return PartialView("_AddAllergyPartial");
            }

            // Save allergy after adding
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AddAllergy(PatientAllergy model)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Prevent duplicate allergy entry
                        bool allergyExists = await _context.PatientAllergies
                            .AnyAsync(pa => pa.PatientId == model.PatientId && pa.AllergyId == model.AllergyId);

                        if (!allergyExists)
                        {
                            _context.PatientAllergies.Add(model);
                            await _context.SaveChangesAsync();
                            return Json(new { success = true, message = "Allergy added successfully" });
                        }

                        return Json(new { success = false, message = "This allergy is already recorded for the patient" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = $"Error adding allergy: {ex.Message}" });
                    }
                }

                ViewBag.PatientId = model.PatientId;
                ViewBag.Allergies = _context.Allergies.ToList();
                return PartialView("_AddAllergyPartial", model);
            }

        public async Task<IActionResult> EditAllergy(int id)
        {
            var model = await _context.PatientAllergies.FindAsync(id);
            if (model == null) return NotFound();

            ViewBag.Allergies = _context.Allergies.ToList();
            return PartialView("_EditAllergyPartial", model);
        }

        // POST: Edit Allergy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllergy(PatientAllergy model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.PatientAllergies.Update(model);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Allergy updated successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error updating allergy: {ex.Message}" });
                }
            }

            ViewBag.Allergies = _context.Allergies.ToList();
            return PartialView("_EditAllergyPartial", model);
        }

        // Get active admissions
        public async Task<IActionResult> Active()
        {
            var admissions = await _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Bed)
                .ThenInclude(b => b.Ward)
                .Where(a => a.DischargeDate == null)
                .ToListAsync();

            return View(admissions);
        }

        //get bed by ward
        public async Task<IActionResult> bedIndex(int? wardId)
        {
            var beds = _context.Beds.Include(b => b.Ward).Where(b=>!b.IsOccupied).AsQueryable();

            if (wardId.HasValue)
            {
                beds = beds.Where(b => b.WardID == wardId);
            }
            // Get all active wards for dropdown
            ViewBag.Wards = await _context.Wards
                .Where(w => w.IsActive)
                .OrderBy(w => w.Name)
                .ToListAsync();

            ViewBag.WardsSelectList = new SelectList(
                await _context.Wards.Where(w => w.IsActive).OrderBy(w => w.Name).ToListAsync(),
                "WardID",
                "Name",
                wardId 
            );

            // Summary count
            ViewBag.AvailableCount = await beds.CountAsync();

            return View(await beds.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentAdmission(int patientId)
        {
            var admission = await _context.Admissions
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .FirstOrDefaultAsync(a => a.PatientID == patientId && a.DischargeDate == null);

            if (admission == null)
            {
                return Json(new { success = false, message = "No active admission found." });
            }

            var fromLocation = $"{admission.Ward.Name} - Bed {admission.Bed.BedNo}";
            return Json(new { success = true, fromLocation });
        }

        // GET: Step 1 - Basic Admission Info
        public IActionResult AdmitPatientStep1(int? patientId)
        {
            var model = new AdmissionStep1ViewModel
            {
                Patients = _context.Patients
                    .Where(p => !p.IsDeleted)
                    .Select(p => new SelectListItem
                    {
                        Value = p.PatientID.ToString(),
                        Text = p.FirstName + " " + p.LastName
                    }).ToList(),

                Beds = _context.Beds
                    .Where(b => !b.IsOccupied)
                    .Select(b => new SelectListItem
                    {
                        Value = b.BedId.ToString(),
                        Text = b.BedNo
                    }).ToList(),

                Wards = _context.Wards
                    .Where(w => w.IsActive)
                    .Select(w => new SelectListItem
                    {
                        Value = w.WardID.ToString(),
                        Text = w.Name
                    }).ToList(),

                Doctors = _context.Users
                    .Where(u => u.RoleType == "Doctor")
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.FullName
                    }).ToList(),

                AdmissionDate = DateOnly.FromDateTime(DateTime.Now)
            };

            if (patientId.HasValue)
                model.PatientID = patientId.Value;

            return View(model);
        }

        // Save Basic Admission Info
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdmitPatientStep1(AdmissionStep1ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // repopulate dropdowns so Step 1 can show again if invalid
                model.Patients = _context.Patients
                    .Where(p => !p.IsDeleted)
                    .Select(p => new SelectListItem
                    {
                        Value = p.PatientID.ToString(),
                        Text = p.FirstName + " " + p.LastName
                    }).ToList();

                model.Beds = _context.Beds
                    .Where(b => !b.IsOccupied)
                    .Select(b => new SelectListItem
                    {
                        Value = b.BedId.ToString(),
                        Text = b.BedNo
                    }).ToList();

                model.Wards = _context.Wards
                    .Where(w => w.IsActive)
                    .Select(w => new SelectListItem
                    {
                        Value = w.WardID.ToString(),
                        Text = w.Name
                    }).ToList();

                model.Doctors = _context.Users
                    .Where(u => u.RoleType == "Doctor")
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.FullName
                    }).ToList();
                return View(model);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Assign bed
                var bed = await _context.Beds.FindAsync(model.BedID);
                if (bed == null || bed.IsOccupied)
                {
                    ModelState.AddModelError("", "Selected bed is invalid or occupied.");
                    return View(model);
                }

                // Validate doctor
                var doctor = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.DoctorID && u.RoleType == "Doctor");
                if (doctor == null)
                {
                    ModelState.AddModelError("", "Invalid doctor selected.");
                    return View(model);
                }

                // Create admission
                var admission = new Admission
                {
                    PatientID = model.PatientID,
                    BedID = bed.BedId,
                    WardID = bed.WardID,
                    DoctorId = model.DoctorID,
                    AdmissionDate = DateOnly.FromDateTime(DateTime.Now),
                    ReasonForAdmission = model.ReasonForAdmission,
                    Notes = model.Notes
                };

                // Assign doctor
                var doctorAssignment = new DoctorAssignment
                {
                    DoctorID = model.DoctorID,
                    PatientID = model.PatientID,
                    AssignmentDate = DateTime.Now,
                    IsActive = true
                };

                bed.IsOccupied = true;

                _context.Admissions.Add(admission);
                _context.DoctorAssignments.Add(doctorAssignment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction("AdmitPatientStep2", new { admissionId = admission.AdmisionID });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View(model);
            }
        }

        // GET: Step 2 - Record Allergies
        public IActionResult AdmitPatientStep2(int admissionId)
        {
            ViewBag.AdmissionId = admissionId;
            return View(new AllergiesStepViewModel { AdmissionID = admissionId });
        }

        // POST: Save Allergies
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdmitPatientStep2(AllergiesStepViewModel model)
        {
            if (model.PatientAllergy != null && model.PatientAllergy.Any())
            {
                foreach (var allergyName in model.PatientAllergy)
                {
                    var allergy = new PatientAllergy
                    {
                        PatientId = model.PatientID,
                        AdmissionId = model.AdmissionID,
                        Name = allergyName,
                        Severity = model.Severity 
                    };
                    _context.PatientAllergies.Add(allergy);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AdmitPatientStep3", new { admissionId = model.AdmissionID });
        }

        // GET: Step 3 - Medical History
        public IActionResult AdmitPatientStep3(int admissionId)
        {
            ViewBag.AdmissionId = admissionId;
            return View(new MedicalHistoryStepViewModel { AdmissionID = admissionId });
        }

        // POST: Save Medical History
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdmitPatientStep3(MedicalHistoryStepViewModel model)
        {
            if (model.MedicalHistories != null && model.MedicalHistories.Any())
            {
                var admission = await _context.Admissions
                    .FirstOrDefaultAsync(a => a.AdmisionID == model.AdmissionID);

                if (admission == null)
                {
                    ModelState.AddModelError("", "Invalid admission ID.");
                    return View(model);
                }

                foreach (var mh in model.MedicalHistories)
                {
                    var history = new MedicalHistory
                    {
                        PatientId = admission.PatientID,
                        AdmissionId = model.AdmissionID,
                        ChronicCondition = mh.ChronicCondition,
                        MedicationHistory = mh.MedicationHistory,
                        PastSurgicalHistory = mh.PastSurgicalHistory,
                        RecorderDate = mh.RecorderDate,
                    };
                    _context.MedicalHistories.Add(history);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AdmissionSummary", new { admissionId = model.AdmissionID });
        }

        public IActionResult AdmissionSummary(int admissionId)
        {
            var admission = _context.Admissions
                .Include(a => a.Patient)
                .Include(a => a.Bed)
                .ThenInclude(b => b.Ward)
                .FirstOrDefault(a => a.AdmisionID == admissionId);

            var allergies = _context.PatientAllergies.Where(a => a.AdmissionId == admissionId).ToList();
            var history = _context.MedicalHistories.Where(h => h.AdmissionId == admissionId).ToList();

            var model = new AdmissionSummaryViewModel
            {
                Admission = admission,
                PatientAllergies = allergies,
                MedicalHistory = history
            };

            return View(model);
        }
    }
}
