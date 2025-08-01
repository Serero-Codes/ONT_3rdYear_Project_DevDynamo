using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;

namespace ONT_3rdyear_Project.Controllers
{
    [Authorize(Roles ="Sister")]
    public class NurseSisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public NurseSisterController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
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
        }

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
                    BedNo = b != null ? b.BedNo : "N/A"
                }
            ).ToListAsync();

            return View(data);
        }














        //CRUD for Vitals
        public async Task<IActionResult> Vitals()
        {
            var vitals = await _context.Vitals.Include(v => v.Patient).Include(v => v.VisitSchedule).Include(u => u.User).Where(v => v.IsActive).OrderByDescending(v => v.Date).ToListAsync();
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

            // Check if the patient already has vitals 
            var existingVitals = await _context.Vitals.Where(v => v.PatientID == PatientID && v.IsActive).FirstOrDefaultAsync();

            if (existingVitals != null)
            {
                return RedirectToAction("VitalsExists", new { PatientID });
            }

            var nurses = await _context.Users.Where(u => u.RoleType == "Nurse").ToListAsync();

            ViewBag.UserList = new SelectList(nurses, "Id", "FullName");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVital(Vital vital)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Vitals));
            }

            ViewBag.PatientList = new SelectList(_context.Patients.ToList(), "PatientID", "FirstName", vital.PatientID);
            var nurses = await _context.Users.Where(u => u.RoleType == "Nurse").ToListAsync();
            ViewBag.UserList = new SelectList(nurses, "Id", "FullName", vital.ApplicationUserID);
            ViewBag.VisitList = new SelectList(_context.VisitSchedules.ToList(), "VisitID", "VisitDate", vital.VisitID);

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
            if (id == null)
            {
                return NotFound();
            }
            var vital = await _context.Vitals.FindAsync(id);
            if (vital == null)
            {
                return NotFound();
            }
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
                    _context.Update(vital);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Dashboard));
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
            if (vital == null)
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
            if (vital != null)
            {
                vital.IsActive = false;
                _context.Update(vital);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Vitals));
        }








        //CRUD for administering
        public async Task<IActionResult> ListAdministered()
        {
            var scheduledScripts = await _context.PatientMedicationScripts
                .Include(p => p.Prescription)
                    .Include(pm => pm.Medication)
                
                .Include(p => p.AdministeredBy)
                .Include(p => p.VisitSchedule)
                .Include(p => p.Patient)
                /*.Where(p => p.Medication.Schedule >= 5) */// Schedule 2 and above are scheduled meds
                .ToListAsync();

            return View(scheduledScripts);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdminister(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var existingAdministered = await _context.PatientMedicationScripts
                .Where(t => t.PatientId == patientId && t.isActive)
                .FirstOrDefaultAsync();

            if (existingAdministered != null)
            {
                // Redirect to manage existing administration
                return RedirectToAction("ManageAdministered", new { id = existingAdministered.Id });
            }

            ViewBag.PatientId = patient.PatientID;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";

            // Nursing Sister only administers Schedule 5 or 6 meds
            ViewBag.MedicationList = new SelectList(_context.Medications./*.Where(m => m.Schedule >= 5).*/ToList(),
                "MedicationId", "Name"
            );

            // Only show nursing sisters in user list
            ViewBag.UserList = new SelectList(_context.Users.Where(u => u.RoleType == "NursingSister").ToList(),"Id", "Name");

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
                TempData["SuccessMessage"] = "Medication successfully administered.";
                return RedirectToAction(nameof(ListAdministered));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error has occurred while saving.");
            }

            var patient = await _context.Patients.FindAsync(medication.PatientId);
            ViewBag.PatientId = patient?.PatientID;
            ViewBag.PatientName = $"{patient?.FirstName} {patient?.LastName}";

            ViewBag.MedicationList = new SelectList(_context.Medications.Where(m => m.Schedule == 5 || m.Schedule == 6).ToList(),"MedicationId", "Name", medication.MedicationId);

            ViewBag.UserList = new SelectList(_context.Users.Where(u => u.RoleType == "NursingSister").ToList(),"Id", "Name", medication.ApplicationUserID);

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
            ViewBag.CurrentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);



            return View(medication);
        }



        /*public async Task<IActionResult> EditAdministered(int? id)
        {
            if (id == null)
                return NotFound();

            var medication = await _context.PatientMedicationScripts
        .Include(p => p.Patient)           // Include patient info
        .Include(m => m.Medication)
        .Include(a => a.AdministeredBy)
        .Include(v => v.VisitSchedule)
        .FirstOrDefaultAsync(m => m.Id == id);

            //var medication = await _context.PatientMedicationScripts.FindAsync(id);
            if (medication == null)
                return NotFound();

            *//*if (medication.Medication.Schedule < 5)
            {
                return View("NotAllowedToEdit", medication);  // You’ll create this view below
            }*//*

            ViewBag.MedicationList = new SelectList(_context.Medications.Where(m => m.Schedule == 5 || m.Schedule == 6),"MedicationId", "Name", medication.MedicationId);

            ViewBag.UserList = new SelectList(_context.Users.Where(u => u.RoleType == "NursingSister"),"Id", "Name", medication.ApplicationUserID);
*//*            ViewBag.CurrentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
*//*

            return View(medication);
        }*/


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

            //var medication = await _context.PatientMedicationScripts.FindAsync(id);
            if (medication == null)
                return NotFound();

            ViewBag.MedicationList = new SelectList(_context.Medications/*.Where(m => m.Schedule == 5 || m.Schedule == 6)*/.ToList(), "MedicationId", "Name", medication.MedicationId);
            ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", medication.ApplicationUserID);
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministered(int id, PatientMedicationScript medication)
        {
            if (id != medication.Id)
                return NotFound();

            var existing = await _context.PatientMedicationScripts.Include(m => m.Medication).FirstOrDefaultAsync(m => m.Id == id);

            if (existing == null)
                return NotFound();

            // Load the selected medication (new one being chosen)
            var selectedMedication = await _context.Medications.FindAsync(medication.MedicationId);
            if (selectedMedication == null)
            {
                ModelState.AddModelError("MedicationId", "Selected medication not found.");
            }

            //  Enforce schedule access rules
            if ((selectedMedication.Schedule == 5 || selectedMedication.Schedule == 6) && !User.IsInRole("Sister"))
            {
                ModelState.AddModelError("", "You are not allowed to administer or update Schedule 5 or 6 medication.");
            }

            if (!ModelState.IsValid)
            {
                // Refill dropdowns
                ViewBag.MedicationList = new SelectList(_context.Medications.Where(m => m.Schedule == 5 || m.Schedule == 6).ToList(),"MedicationId", "Name", medication.MedicationId);

                ViewBag.UserList = new SelectList(_context.Users.Where(u => u.RoleType == "NursingSister").ToList(),"Id", "Name", medication.ApplicationUserID);

                return View(medication);
            }

            existing.ApplicationUserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            // Update only editable fields
            existing.MedicationId = medication.MedicationId;
/*            existing.ApplicationUserID = medication.ApplicationUserID;
*/            existing.AdministeredDate = medication.AdministeredDate;
            existing.Dosage = medication.Dosage;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Medication successfully administered.";
            return RedirectToAction(nameof(ListAdministered));
        }



        public async Task<IActionResult> DeleteAdministered(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var medication = await _context.PatientMedicationScripts.Include(p => p.Patient).Include(v => v.VisitSchedule).Include(a => a.AdministeredBy).Include(m => m.Medication).FirstOrDefaultAsync(a => a.Id == id);
            if (medication == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);


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
            }
            return RedirectToAction(nameof(ListAdministered));
        }


        //Instruction
        public async Task<IActionResult> InstructionList()
        {
            var instructions = await _context.Instructions.Include(i => i.Patient).ToListAsync();
            //.Where(i => i.ApplicationUserID == currentNurseId) // Filter by logged-in nurse


            return View(instructions);
        }


        public async Task<IActionResult> ViewAdvice(int patientId)
        {
            var instruction = await _context.Instructions.Include(i => i.Patient).FirstOrDefaultAsync(i => i.PatientID == patientId);

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

            return RedirectToAction("InstructionList");
        }







        //TREATMENT CRUD
        public async Task<IActionResult> Treatments()
        {
            var treatments = await _context.Treatments.Include(p => p.Patient).Include(u => u.User).Include(v => v.VisitSchedule).Where(t => t.IsActive).ToListAsync();
            return View(treatments);
        }

        public async Task<IActionResult> TreatmentExists(int patientId)
        {
            var treatment = await _context.Treatments.Include(p => p.Patient).Include(v => v.User).FirstOrDefaultAsync(v => v.PatientID == patientId);

            if (treatment == null)
                return RedirectToAction("CreateTreatment", new { patientId = patientId });

            return View(treatment);
        }

        // GET: CreateTreatment for a specific patient (and optionally visit)
        public async Task<IActionResult> CreateTreatment(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                return NotFound();

            // Check if an active treatment exists for this patient
            var existingTreatment = await _context.Treatments.Where(t => t.PatientID == patientId && t.IsActive).FirstOrDefaultAsync();

            if (existingTreatment != null)
            {
                // Redirect to TreatmentExists page 
                return RedirectToAction("TreatmentExists", new { patientId });
            }

            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientId = patientId;


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

                _context.Update(existingTreatment);
                await _context.SaveChangesAsync();

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

            var treatment = await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.VisitSchedule)
                .FirstOrDefaultAsync(t => t.TreatmentID == id && t.IsActive);

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
            }

            return RedirectToAction(nameof(Treatments));
        }


    }
}
