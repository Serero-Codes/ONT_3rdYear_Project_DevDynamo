using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
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

            // Search for patients
            public async Task<IActionResult> Search(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return View(new List<Patient>());
                }

                var patients = await _context.Patients
                    .Where(p => p.FirstName.Contains(query) ||
                               p.LastName.Contains(query))
                    .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                    .ToListAsync();

                return View(patients);
            }

            // Get All patients
            public async Task<IActionResult> Index()
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
                ViewBag.Beds = _context.Beds.Where(b => !b.IsOccupied).Include(b => b.Ward).ToList();
                ViewBag.Doctors = _context.Users.Where(u => u.RoleType == "Doctor").ToList();
                return View();
            }

            // Admit Patient
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> AdmitPatient(Patient patient, int bedId, int doctorId)
            {
                if (ModelState.IsValid)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        //// Check if patient already exists (by ID number)
                        //var existingPatient = await _context.Patients
                        //    .FirstOrDefaultAsync(p => p.IDNumber == patient.IDNumber);

                        //if (existingPatient != null)
                        //{
                        //    ModelState.AddModelError("IDNumber", "Patient with this ID number already exists");
                        //    ViewBag.Beds = _context.Beds.Where(b => !b.IsOccupied).Include(b => b.Ward).ToList();
                        //    ViewBag.Doctors = _context.Users.Where(u => u.RoleType == "Doctor").ToList();
                        //    return View(patient);
                        //}

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
                            Notes = $"Admitted by {User.Identity.Name} on {DateTime.Now:yyyy-MM-dd HH:mm}"
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

            // Discharge patient
            [HttpPost]
            [Authorize(Roles = "WardAdmin")]
            public async Task<IActionResult> Discharge(int patientId, string instructions)
            {
                if (string.IsNullOrWhiteSpace(instructions))
                {
                    TempData["Error"] = "Discharge instructions are required";
                    return RedirectToAction("Index");
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
                        return RedirectToAction("Index");
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    TempData["Error"] = $"Error discharging patient: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }

            // Record patient movement
            [HttpPost]
            public async Task<IActionResult> LogMovement(int patientId, string from, string to)
            {
                if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                {
                    return Json(new { success = false, message = "From and To locations are required" });
                }

                try
                {
                    var movement = new Movement
                    {
                        PatientID = patientId,
                        FromLocation = from,
                        ToLocation = to,
                        TimeStamp = DateTime.Now,
                    };

                    _context.Add(movement);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Movement logged successfully" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Error logging movement: {ex.Message}" });
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
        
    }
}
