using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;


namespace ONT_3rdyear_Project.Controllers
{
    [Authorize(Roles = "Nurse, Sister")]
    public class NurseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public NurseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var today = DateTime.Today;

            var totalPatients = await _context.Patients.CountAsync();

            var treatmentsToday = await _context.Treatments
                .Where(t => t.IsActive)
                .CountAsync();

            var medicationsGivenToday = await _context.PatientMedicationScripts
                .Where(m =>  m.isActive)
                .CountAsync();

            var model = new DashboardViewModel
            {
                Stats = new DashboardStatsViewModel
                {
                    TotalPatients = totalPatients,
                    TreatmentsToday = treatmentsToday,
                    MedicationsGivenToday = medicationsGivenToday,
                    HoursOnDuty = 24 // Replace with actual logic if needed
                },
                Patients = await (
                    from p in _context.Patients
                    join a in _context.Admissions on p.PatientID equals a.PatientID into admissionGroup
                    from a in admissionGroup.DefaultIfEmpty()
                    join w in _context.Wards on a.WardID equals w.WardID into wardGroup
                    from w in wardGroup.DefaultIfEmpty()
                    join b in _context.Beds on a.BedID equals b.BedId into bedGroup
                    from b in bedGroup.DefaultIfEmpty()
                    select new PatientDashboardViewModel
                    {
                        PatientID = p.PatientID,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        WardName = w != null ? w.Name : "N/A",
                        BedNo = b != null ? b.BedNo : "N/A"
                    }
                ).ToListAsync()
            };

            return View(model);
        }


        // NurseController.cs
        /*public async Task<IActionResult> PatientsList()
        {
            var data = await (
                from p in _context.Patients
                join a in _context.Admissions on p.PatientID equals a.PatientID into admissionGroup
                from a in admissionGroup.DefaultIfEmpty()
                join w in _context.Wards on a.WardID equals w.WardID into wardGroup
                from w in wardGroup.DefaultIfEmpty()
                join b in _context.Beds on a.BedID equals b.BedId into bedGroup
                from b in bedGroup.DefaultIfEmpty()
                select new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = w != null ? w.Name : "N/A",
                    BedNo = b != null ? b.BedNo : "N/A"
                }
            ).ToListAsync();

            return View(data);
        }*/

        public async Task<IActionResult> PatientsList()
        {
            var data = await (
                from p in _context.Patients
                join a in _context.Admissions on p.PatientID equals a.PatientID into admissionGroup
                from a in admissionGroup.DefaultIfEmpty()
                join w in _context.Wards on a.WardID equals w.WardID into wardGroup
                from w in wardGroup.DefaultIfEmpty()
                join b in _context.Beds on a.BedID equals b.BedId into bedGroup
                from b in bedGroup.DefaultIfEmpty()
                select new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = w != null ? w.Name : "N/A",
                    BedNo = b != null ? b.BedNo : "N/A",
                    Status = a != null ? "Admitted" : "Not Admitted"
                }
            ).ToListAsync();

            return View(data);
        }




        /*public async Task<IActionResult> SearchPatients(string query)
        {
            var patients = await _context.Patients.Include(p => p.Admissions).ThenInclude(a => a.Ward).Include(p => p.Admissions).ThenInclude(a => a.Bed)
                                                                                                .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query)).ToListAsync();

            // Map to ViewModel
            var patientViewModels = patients.Select(p =>
            {
                var admission = p.Admissions;
                return new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = admission?.Ward?.Name ?? "N/A",
                    BedNo = admission?.Bed?.BedNo ?? "N/A"
                };
            }).ToList();

            return View("PatientsList", patientViewModels);
        }*/
        public async Task<IActionResult> SearchPatients(string query)
        {
            var data = await (
                from p in _context.Patients
                join a in _context.Admissions on p.PatientID equals a.PatientID into admissionGroup
                from a in admissionGroup.DefaultIfEmpty()
                join w in _context.Wards on a.WardID equals w.WardID into wardGroup
                from w in wardGroup.DefaultIfEmpty()
                join b in _context.Beds on a.BedID equals b.BedId into bedGroup
                from b in bedGroup.DefaultIfEmpty()
                where p.FirstName.Contains(query) || p.LastName.Contains(query)
                select new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = w != null ? w.Name : "N/A",
                    BedNo = b != null ? b.BedNo : "N/A",
                    Status = a != null ? "Admitted" : "Not Admitted"
                }
            ).ToListAsync();

            return View("PatientsList", data);
        }










        /*public async Task<IActionResult> Dashboard()
        {
            var data = await (
                from p in _context.Patients
                join a in _context.Admissions on p.PatientID equals a.PatientID
                join w in _context.Wards on a.WardID equals w.WardID
                join b in _context.Beds on a.BedID equals b.BedId

                // LEFT JOIN Discharges, so you can check if discharged
                join d in _context.Discharges on p.PatientID equals d.PatientID into dischargesGroup
                from discharge in dischargesGroup.DefaultIfEmpty()

                where discharge == null || discharge.IsDischarged == false

                select new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = w != null ? w.Name : "N/A",
                    BedNo = b != null ? b.BedNo : "N/A"
                }
            ).ToListAsync();

            return View(data);
        }*/





        //CRUD for Vitals
        public async Task<IActionResult> Vitals()
        {
            var vitals = await _context.Vitals.Include(v => v.Patient).Include(v=>v.VisitSchedule).Include(u=>u.User).Where(v=> v.IsActive).OrderByDescending(v => v.Date).ToListAsync();
            return View(vitals);
        }

        
        public async Task<IActionResult> CreateVital(int PatientID)
        {
            var patient = await _context.Patients.FindAsync(PatientID);
            if (patient == null)
            {
                return NotFound(); // patient ID doesn't exist
            }

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientId = PatientID;

            // Check if there's a record for today
            /* var today = DateTime.Today;
             var existingVitals = await _context.Vitals
                 .Where(v => v.PatientID == PatientID && v.Date.Date == today && v.IsActive)
                 .FirstOrDefaultAsync();

             if (existingVitals != null)
             {
                 // Redirect to details of existing record for today
                 return RedirectToAction("VitalsExists", new { PatientID });
             }*/

            /* var nurses = await _context.Users
                 .Where(u => u.RoleType == "Nurse," || u.RoleType == "Sister")
                 .ToListAsync();

             ViewBag.UserList = new SelectList(nurses, "Id", "FullName");*/
            var user = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserName = user.FullName;

            return View();
        }


        //vitals details
        public async Task<IActionResult> VitalsDetails(int? id)
        {
            if(id == null || _context.Vitals == null)
            {
                return NotFound();
            }
            var vitals = await _context.Vitals.Include(v => v.Patient).Include(v => v.User).FirstOrDefaultAsync(v => v.VitalID == id);
            return View(vitals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVital(Vital vital)
        {
            var user = await _userManager.GetUserAsync(User);
            vital.ApplicationUserID = user.Id;
            if (ModelState.IsValid)
            {
                _context.Add(vital);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vitals successfully taken!";
                return RedirectToAction(nameof(Vitals));
            }

            ViewBag.PatientList = new SelectList(_context.Patients.ToList(), "PatientID", "FirstName", vital.PatientID);
            var nurses = await _context.Users.Where(u => u.RoleType == "Nurse").ToListAsync();
            ViewBag.UserList = new SelectList(nurses, "Id", "FullName", vital.ApplicationUserID);
            ViewBag.VisitList = new SelectList(_context.VisitSchedules.ToList(), "VisitID", "VisitDate", vital.VisitID);


            var userId = int.Parse(_userManager.GetUserId(User));
            vital.ApplicationUserID = userId;
            return View(vital);
        }

        public async Task<IActionResult> VitalsExists(int patientId)
        {
            var vital = await _context.Vitals.Include(v => v.User).FirstOrDefaultAsync(v => v.PatientID == patientId);

            if (vital == null)
                return RedirectToAction("CreateVital", new { patientId = patientId });//TODO: change addvitals to createvitals

            return View(vital);
        }

        public async Task<IActionResult> EditVital(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var vital = await _context.Vitals.FindAsync(id);
            if(vital == null)
            {
                return NotFound();
            }
            var nurseId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var nurse = await _context.Users.FindAsync(nurseId);
            ViewBag.NurseName = nurse?.FullName;
            ViewBag.PatientList = new SelectList(_context.Patients, "PatientID", "FirstName", vital.PatientID);
            ViewBag.UserList = new SelectList(_context.Users, "Id", "FullName", vital.ApplicationUserID);
            ViewBag.VisitList = new SelectList(_context.VisitSchedules, "VisitID", "VisitDate", vital.VisitID);

            return View(vital);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVital(int id, Vital vital)
        {
            if (id != vital.VitalID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    vital.ApplicationUserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                    _context.Update(vital);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vitals successfully updated!";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vitals.Any(e => e.VitalID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Vitals));
            }

           
            ViewBag.PatientList = new SelectList(_context.Patients, "PatientID", "FirstName", vital.PatientID);
            ViewBag.UserList = new SelectList(_context.Users, "Id", "FullName", vital.ApplicationUserID);
            ViewBag.VisitList = new SelectList(_context.VisitSchedules, "VisitID", "VisitDate", vital.VisitID);

            return View(vital);
        }

        public async Task<IActionResult> DeleteVital(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vital = await _context.Vitals.Include(p => p.Patient).Include(u => u.User).Include(v => v.VisitSchedule).FirstOrDefaultAsync(v => v.VitalID == id);
            if(vital == null)
            {
                return NotFound();
            }
            return View(vital);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVital(int id)
        {
            var vital = await _context.Vitals.FindAsync(id);
            if(vital != null)
            {
                vital.IsActive = false;
                _context.Update(vital);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Vitals successfully deleted!";
            }
            return RedirectToAction(nameof(Vitals));
        }
































        //TREATMENT CRUD
        public async Task<IActionResult> Treatments()
        {
            var treatments = await _context.Treatments.Include(p=>p.Patient).Include(u=>u.User).Include(v=>v.VisitSchedule).Where(t=>t.IsActive).ToListAsync();
            return View(treatments);
        }

        public async Task<IActionResult> TreatmentExists(int patientId)
        {
            var treatment = await _context.Treatments.Include(p=>p.Patient).Include(v => v.User).FirstOrDefaultAsync(v => v.PatientID == patientId);

            if (treatment == null)
                return RedirectToAction("CreateTreatment", new { patientId = patientId });

            return View(treatment);
        }

        
        public async Task<IActionResult> CreateTreatment(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                return NotFound();

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientId = patientId;
            // ✅ Correct way to check if there's a treatment today
            /*var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var existingTreatment = await _context.Treatments
                .Where(v => v.PatientID == patientId
                         && v.TreatmentDate >= today
                         && v.TreatmentDate < tomorrow
                         && v.IsActive)
                .FirstOrDefaultAsync();

            if (existingTreatment != null)
            {
                return RedirectToAction("TreatmentExists", new { patientId });
            }*/

            // Continue as normal
            

            var visits = await _context.VisitSchedules.Where(v => v.PatientID == patientId).ToListAsync();
            ViewBag.VisitList = new SelectList(visits, "VisitID", "VisitDate");

            ViewBag.TreatmentTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Wound Dressing", Value = "Wound Dressing" },
                new SelectListItem { Text = "IV Drip", Value = "IV Drip" },
                new SelectListItem { Text = "Catheter Change", Value = "Catheter Change" },
                new SelectListItem { Text = "Physiotherapy", Value = "Physiotherapy" },
                new SelectListItem { Text = "Other", Value = "Other" }
            };

            return View(new Treatment { PatientID = patientId });
        }


        // POST: CreateTreatment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTreatment(Treatment treatment)
        {
            // Handle custom treatment type
            if (treatment.TreatmentType == "Other")
            {
                var otherTreatment = Request.Form["TreatmentTypeOther"].ToString();
                if (!string.IsNullOrWhiteSpace(otherTreatment))
                    treatment.TreatmentType = otherTreatment;
                else
                    ModelState.AddModelError("TreatmentTypeOther", "Please specify the treatment type.");
            }

            if (!ModelState.IsValid)
            {
                // Reload necessary data to redisplay the form
                var patient = await _context.Patients.FindAsync(treatment.PatientID);
                if (patient == null) return NotFound();

                ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
                ViewBag.PatientId = treatment.PatientID;

                var visits = await _context.VisitSchedules.Where(v => v.PatientID == treatment.PatientID).ToListAsync();
                ViewBag.VisitList = new SelectList(visits, "VisitID", "VisitDate", treatment.VisitID);

                ViewBag.TreatmentTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Wound Dressing", Value = "Wound Dressing" },
                    new SelectListItem { Text = "IV Drip", Value = "IV Drip" },
                    new SelectListItem { Text = "Catheter Change", Value = "Catheter Change" },
                    new SelectListItem { Text = "Physiotherapy", Value = "Physiotherapy" },
                    new SelectListItem { Text = "Other", Value = "Other" }
                };

                return View(treatment);
            }

            // Validate patient exists
            var existingPatient = await _context.Patients.FindAsync(treatment.PatientID);
            if (existingPatient == null)
                return NotFound("Patient not found.");

            // Get current logged-in user ID
            var userIdStr = _userManager.GetUserId(User);
            if (!int.TryParse(userIdStr, out int userId))
                return Unauthorized();

            treatment.ApplicationUserID = userId;
            treatment.TreatmentDate = DateTime.Now;
            treatment.IsActive = true;

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Treatment successfully performed!";
            return RedirectToAction("Treatments", new { PatientId = treatment.PatientID });
        }


        //editing treatment
        public async Task<IActionResult> EditTreatment(int id)
        {
            var treatment = await _context.Treatments.Include(t => t.Patient).FirstOrDefaultAsync(t => t.TreatmentID == id);

            if (treatment == null)
            {
                return NotFound();
            }

            // Load visits for the patient to populate dropdown
            var visits = await _context.VisitSchedules.Where(v => v.PatientID == treatment.PatientID).ToListAsync();
            ViewBag.VisitList = new SelectList(visits, "VisitID", "VisitDate", treatment.VisitID);

            ViewBag.TreatmentTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Wound Dressing", Value = "Wound Dressing" },
                new SelectListItem { Text = "IV Drip", Value = "IV Drip" },
                new SelectListItem { Text = "Catheter Change", Value = "Catheter Change" },
                new SelectListItem { Text = "Physiotherapy", Value = "Physiotherapy" },
                new SelectListItem { Text = "Other", Value = "Other" }
            }.Select(x => {
                x.Selected = x.Value == treatment.TreatmentType;
                return x;
            }).ToList();

            var nurseId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var nurse = await _context.Users.FindAsync(nurseId);
            ViewBag.NurseName = nurse?.FullName;
            return View(treatment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTreatment(int id, Treatment model)
        {
            if (id != model.TreatmentID)
                return NotFound();

            // Handle custom "Other" treatment type
            if (model.TreatmentType == "Other")
            {
                var otherTreatment = Request.Form["TreatmentTypeOther"].ToString();
                if (!string.IsNullOrWhiteSpace(otherTreatment))
                {
                    model.TreatmentType = otherTreatment;
                }
                else
                {
                    ModelState.AddModelError("TreatmentTypeOther", "Please specify the treatment type.");
                }
            }

            if (!ModelState.IsValid)
            {
                // Reload dropdowns if invalid
                var visits = await _context.VisitSchedules.Where(v => v.PatientID == model.PatientID).ToListAsync();
                ViewBag.VisitList = new SelectList(visits, "VisitID", "VisitDate", model.VisitID);

                ViewBag.TreatmentTypes = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Wound Dressing", Value = "Wound Dressing" },
                    new SelectListItem { Text = "IV Drip", Value = "IV Drip" },
                    new SelectListItem { Text = "Catheter Change", Value = "Catheter Change" },
                    new SelectListItem { Text = "Physiotherapy", Value = "Physiotherapy" },
                    new SelectListItem { Text = "Other", Value = "Other" }
                };

                return View(model);
            }

            try
            {
                // Attach existing treatment and update fields
                var existingTreatment = await _context.Treatments.FindAsync(id);
                if (existingTreatment == null)
                    return NotFound();

                existingTreatment.VisitID = model.VisitID;
                existingTreatment.TreatmentType = model.TreatmentType;
                existingTreatment.TreatmentDate = model.TreatmentDate;

                // Keep other fields like PatientID, ApplicationUserID unchanged or adjust as needed
                model.ApplicationUserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                existingTreatment.ApplicationUserID = int.Parse(
    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
);

                _context.Update(existingTreatment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Treatment successfully updated!";
                return RedirectToAction("Treatments");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Treatments.Any(e => e.TreatmentID == id))
                    return NotFound();

                throw;
            }
        }

        public async Task<IActionResult> DeleteTreatment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatment = await _context.Treatments.Include(t => t.Patient).Include(t => t.VisitSchedule).FirstOrDefaultAsync(t => t.TreatmentID == id && t.IsActive);

            if (treatment == null)
            {
                return NotFound();
            }

            return View(treatment); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);

            if (treatment != null)
            {
                treatment.IsActive = false; 
                _context.Treatments.Update(treatment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Treatment successfully deleted!";
            }

            return RedirectToAction(nameof(Treatments)); 
        }











































        //administer medication
        public async Task<IActionResult> Administered()
        {
            var medication = await _context.PatientMedicationScripts.Include(p=>p.Patient).Include(v=>v.VisitSchedule).Include(a=>a.AdministeredBy).Include(m=>m.Medication).Where(ad=>ad.isActive).ToListAsync();
            return View(medication);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdminister(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }

           /* var existingAdministered = await _context.PatientMedicationScripts
                .Where(t => t.PatientId == patientId && t.isActive)
                .FirstOrDefaultAsync();

            if (existingAdministered != null)
            {
                // Redirect to TreatmentExists page 
                return RedirectToAction("ManageAdministered", new { id = existingAdministered.Id });
            }*/

            ViewBag.PatientId = patient.PatientID;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";

            ViewBag.MedicationList = new SelectList(_context.Medications.Where(m => m.Schedule == 1 || m.Schedule == 2 || m.Schedule == 3 || m.Schedule == 4).ToList(), "MedicationId", "Name");
            ViewBag.UserList = new SelectList(_context.Users.ToList(), "ApplicationUserID", "FullName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminister(PatientMedicationScript medication)
        {
            

            try
            {
                _context.PatientMedicationScripts.Add(medication);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medication administered successfully!";
                return RedirectToAction(nameof(Administered));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error has occurred.");
            }

            var patient = await _context.Patients.FindAsync(medication.PatientId);
            ViewBag.PatientId = patient?.PatientID;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";

            ViewBag.MedicationList = new SelectList(_context.Medications, "MedicationId", "Name", medication.MedicationId);
            ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", medication.ApplicationUserID);
            return View(medication);
        }

        public async Task<IActionResult> ManageAdministered(int id)
        {
            var medication = await _context.PatientMedicationScripts
                .Include(p => p.Patient)
                .Include(p => p.Medication)
                .Include(v => v.VisitSchedule)
                .Include(a => a.AdministeredBy)
                .FirstOrDefaultAsync(m => m.Id == id && m.isActive);

            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }



        public async Task<IActionResult> EditAdministered(int? id)
        {
            if (id == null)
                return NotFound();

            var medication = await _context.PatientMedicationScripts
            .Include(p => p.Patient)           // Include patient info
            .Include(m => m.Medication)
            .Include(a => a.AdministeredBy)
            .Include(v => v.VisitSchedule)
            .FirstOrDefaultAsync(m => m.Id == id);

            
            if (medication == null)
                return NotFound();

            ViewBag.MedicationList = new SelectList(_context.Medications/*.Where(m => m.Schedule == 1 || m.Schedule == 2 || m.Schedule == 3 || m.Schedule == 4)*/.ToList(), "MedicationId", "Name", medication.MedicationId);
            ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", medication.ApplicationUserID);
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministered(int id, PatientMedicationScript medication)
        {
            if (id != medication.Id)
                return NotFound();

            var existing = await _context.PatientMedicationScripts.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Update only editable fields
            existing.MedicationId = medication.MedicationId;
            /*            existing.ApplicationUserID = medication.ApplicationUserID;
            */
            existing.ApplicationUserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            existing.AdministeredDate = medication.AdministeredDate;
            existing.Dosage = medication.Dosage;
            
            // DO NOT override isActive or navigation properties like Patient, VisitSchedule

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Medication successfully updated!";
            return RedirectToAction(nameof(Administered));
        }


        public async Task<IActionResult> DeleteAdministered(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var medication = await _context.PatientMedicationScripts.Include(p => p.Patient).Include(v => v.VisitSchedule).Include(a => a.AdministeredBy).Include(m => m.Medication).FirstOrDefaultAsync(a=>a.Id == id);
            if (medication == null)
            {
                return NotFound();
            }
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdminister(int id)
        {
            var medication = await _context.PatientMedicationScripts.FindAsync(id);
            if (medication != null)
            {
                medication.isActive = false;
                _context.PatientMedicationScripts.Update(medication);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medication successfully Deleted!";
            }
            return RedirectToAction(nameof(Administered));
        }











        //doctor advice

        /*public async Task<IActionResult> RequestedAdvices()
        {
            var instructions = await _context.Instructions
                .Include(i => i.Patient)
                .OrderByDescending(i => i.DateRecorded)
                .ToListAsync();

            return View(instructions);
        }*/

        public async Task<IActionResult> InstructionList()
        {
            var instructions = await _context.Instructions
                .Include(i => i.Patient)
                //.Where(i => i.ApplicationUserID == currentNurseId) // Filter by logged-in nurse
                .ToListAsync();

            return View(instructions);
        }


        public async Task<IActionResult> ViewAdvice(int id)
        {
            var instruction = await _context.Instructions
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(i => i.InstructionID == id);

            if (instruction == null || string.IsNullOrEmpty(instruction.Instructions))
            {
                return NotFound();
            }

            return View(instruction);
        }




        [HttpGet]
        public async Task<IActionResult> CreateRequest(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.PatientID = patient.PatientID;
            ViewBag.PatientName = patient.FirstName + " " + patient.LastName;
            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRequest(int patientId, string message)
        {
            var userId = _userManager.GetUserId(User);
            var request = new Instruction
            {
                PatientID = patientId,
                NurseRequest = message, 
                DateRecorded = DateTime.Now,
                ApplicationUserID = int.Parse(userId)
            };

            _context.Instructions.Add(request);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Advice successfully requested!";
            return RedirectToAction("InstructionList");
        }

        

        [HttpGet]
        public async Task<IActionResult> LiveSearch(string query, string ward, string bed)
        {
            var patientsQuery = _context.Patients
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Ward)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                patientsQuery = patientsQuery.Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query));
            }

            if (!string.IsNullOrWhiteSpace(ward))
            {
                patientsQuery = patientsQuery.Where(p => p.Admissions.Ward.Name == ward);
            }

            if (!string.IsNullOrWhiteSpace(bed))
            {
                patientsQuery = patientsQuery.Where(p => p.Admissions.Bed.BedNo == bed);
            }

            var patients = await patientsQuery.ToListAsync();

            var viewModel = patients.Select(p =>
            {
                var admission = p.Admissions;

                return new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = admission?.Ward?.Name ?? "N/A",
                    BedNo = admission?.Bed?.BedNo ?? "N/A"
                };
            }).ToList();

            return PartialView("_PatientSearchResults", viewModel);
        }
        /*[HttpGet]
        public async Task<IActionResult> LiveSearchAll()
        {
            var patients = await _context.Patients
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Ward)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                .ToListAsync();

            var viewModel = patients.Select(p =>
            {
                var admission = p.Admissions;

                return new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = admission?.Ward?.Name ?? "N/A",
                    BedNo = admission?.Bed?.BedNo ?? "N/A"
                };
            }).ToList();

            return PartialView("_PatientSearchResults", viewModel);
        }*/

        public async Task<IActionResult> LiveSearchAll()
        {
            var patients = await _context.Patients
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Ward)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                .ToListAsync();

            var viewModel = patients.Select(p => new PatientDashboardViewModel
            {
                PatientID = p.PatientID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                WardName = p.Admissions != null ? p.Admissions.Ward.Name : "N/A",
                BedNo = p.Admissions != null ? p.Admissions.Bed.BedNo : "N/A",
                Status = p.Admissions != null ? "Admitted" : "Not Admitted"
            }).ToList();

            return PartialView("_PatientSearchResults", viewModel);
        }

        public async Task<IActionResult> LiveSearch(string query)
        {
            var patients = await _context.Patients
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Ward)
                .Include(p => p.Admissions)
                    .ThenInclude(a => a.Bed)
                .Where(p => p.FirstName.Contains(query) || p.LastName.Contains(query))
                .ToListAsync();

            var viewModel = patients.Select(p => new PatientDashboardViewModel
            {
                PatientID = p.PatientID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                WardName = p.Admissions != null ? p.Admissions.Ward.Name : "N/A",
                BedNo = p.Admissions != null ? p.Admissions.Bed.BedNo : "N/A",
                Status = p.Admissions != null ? "Admitted" : "Not Admitted"
            }).ToList();

            return PartialView("_PatientSearchResults", viewModel);
        }



        public IActionResult LiveSearchVitals(string query)
        {
            var results = _context.Vitals
                .Include(v => v.Patient)
                .Include(v => v.User)
                .Where(v => v.Patient.FirstName.Contains(query) || v.Patient.LastName.Contains(query))
                .ToList();

            return PartialView("_VitalSearchResultsPartial", results);
        }

        public IActionResult LiveSearchAllVitals()
        {
            var allVitals = _context.Vitals
                .Include(v => v.Patient)
                .Include(v => v.User)
                .ToList();

            return PartialView("_VitalSearchResultsPartial", allVitals);
        }

        public IActionResult LiveSearchTreatments(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return LiveSearchAllTreatments();
            }

            var results = _context.Treatments
                .Include(v => v.Patient)
                .Include(v => v.User)
                .Where(v => v.Patient != null &&
                    (EF.Functions.Like(v.Patient.FirstName, $"%{query}%") ||
                     EF.Functions.Like(v.Patient.LastName, $"%{query}%")))
                .ToList();

            return PartialView("_TreatmentSearchResultsPartial", results);
        }



        public IActionResult LiveSearchAllTreatments()
        {
            var allTreatments = _context.Treatments
                .Include(v => v.Patient)
                .Include(v => v.User)
                .ToList();

            return PartialView("_TreatmentSearchResultsPartial", allTreatments);
        }






    }
}
