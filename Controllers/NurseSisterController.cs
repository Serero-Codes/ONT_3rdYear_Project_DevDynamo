using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var today = DateTime.Today;

            var totalPatients = await _context.Patients.CountAsync();

            var treatmentsToday = await _context.Treatments
                .Where(t => t.IsActive)
                .CountAsync();

            var medicationsGivenToday = await _context.PatientMedicationScripts
                .Where(m => m.isActive)
                .CountAsync();
            var totalVitals = await _context.Vitals.CountAsync();

            var model = new DashboardViewModel
            {
                Stats = new DashboardStatsViewModel
                {
                    TotalPatients = totalPatients,
                    TreatmentsToday = treatmentsToday,
                    MedicationsGivenToday = medicationsGivenToday,
                    //HoursOnDuty = 24 
                    TotalVitals = totalVitals
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

        public async Task<IActionResult> PatientsList()
        {

            

            var beds = await _context.Beds.Where(b => !b.IsDeleted).ToListAsync();
            ViewBag.Beds = new SelectList(beds, "BedId", "BedNo");

            var wards = await _context.Wards.ToListAsync();
            ViewBag.Wards = new SelectList(wards, "WardID", "Name");


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


       





        //CRUD for administering
        public async Task<IActionResult> ListAdministered()
        {
            var scheduledScripts = await _context.PatientMedicationScripts
                .Include(p => p.Prescription)
                    .Include(pm => pm.Medication).Include(d=>d.Prescription)
                
                .Include(p => p.AdministeredBy)
                .Include(p => p.VisitSchedule)
                .Include(p => p.Patient)
                
                .Where(s=>s.isActive) // Schedule 2 and above are scheduled meds
                .ToListAsync();

            var viewModel = scheduledScripts.Select(m => new AdministerMedicationViewModel
            {
                Id = m.Id,
                PatientName = m.Patient.FirstName + " " + m.Patient.LastName,
                MedicationName = m.Medication.Name,
                Dosage = m.Dosage,
                ApplicationUserName = m.AdministeredBy.FullName,
                //HasPrescription = m.PrescriptionId != null,
                PrescriptionId = m.PrescriptionId,
                AdministeredDate = m.AdministeredDate
            }).ToList();

            return View(viewModel);

            
        }

        
        // GET: show form and optionally fetch prescription when medicationId provided
       [HttpGet]
        public async Task<IActionResult> CreateAdminister(int patientId, int? medicationId = null)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null) return NotFound();

            var meds = await _context.Medications.Select(m => new
            {
                m.MedicationId,DisplayName = m.Name + " (Schedule " + m.Schedule + ")"
            }).ToListAsync();

            bool requiresPrescription = false;
            // If medication is selected, filter prescriptions for that patient + medication
            List<Prescription> prescriptions = new List<Prescription>();

            if (medicationId.HasValue)
            {
                var med = await _context.Medications.FindAsync(medicationId.Value);
                if (med != null && med.Schedule > 4)
                {
                    requiresPrescription = true;
                    prescriptions = await _context.Prescriptions
                        .Include(p => p.Prescribed_Medication)
                        .Where(p => p.PatientId == patientId &&
                            p.Prescribed_Medication.Any(pm => pm.MedicationId == medicationId.Value))
                        .ToListAsync();
                }
            }

            var nurses = _context.Users.Where(u => u.RoleType == "NursingSister").Select(u => new { u.Id, u.FullName }).ToList();

            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _userManager.GetUserAsync(User);
            var model = new AdministerMedicationViewModel
            {
                PatientId = patient.PatientID,
                PatientName = $"{patient.FirstName} {patient.LastName}",
                ApplicationUserID = user.Id,
                ApplicationUserName = user.FullName,
                AdministeredDate = DateTime.Today,
                MedicationList = new SelectList(meds, "MedicationId", "DisplayName", medicationId),
                UserList = new SelectList(nurses, "Id", "FullName"),
                PrescriptionList = new SelectList(prescriptions, "Id", "PrescriptionNote"), // or some meaningful display
                RequiresPrescription = requiresPrescription
            };

            return View(model);
        }


        // POST: save administration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminister(AdministerMedicationViewModel vm)
        {
            var user = await _userManager.GetUserAsync(User);
            vm.ApplicationUserID = user.Id;

            var med = await _context.Medications.FindAsync(vm.MedicationId);
            vm.RequiresPrescription = med?.Schedule > 4;
            //if (med == null) ModelState.AddModelError("", "Medication not found.");

            // If medication schedule is high and no prescription found, add error
            if (vm.RequiresPrescription && vm.PrescriptionId == null)
            {
                ModelState.AddModelError("PrescriptionId", "A prescription is required for Schedule 5 and above medications.");
            }

            if (!ModelState.IsValid)
            {
                var patient = await _context.Patients.FindAsync(vm.PatientId);
                if (patient != null)
                    vm.PatientName = $"{patient.FirstName} {patient.LastName}";
                var meds = await _context.Medications
                    .Select(m => new
                    {
                        m.MedicationId,
                        DisplayName = m.Name + " (Schedule " + m.Schedule + ")"
                    })
                    .ToListAsync();

                var nurses = _context.Users.Where(u => u.RoleType == "NursingSister").Select(u => new { u.Id, u.FullName }).ToList();


                var prescriptions = vm.RequiresPrescription
                ? await _context.Prescriptions.Where(p => p.PatientId == vm.PatientId).ToListAsync()
                : new List<Prescription>();


                vm.MedicationList = new SelectList(meds, "MedicationId", "DisplayName", vm.MedicationId);
                vm.UserList = new SelectList(nurses, "Id", "FullName", vm.ApplicationUserID);
                vm.PrescriptionList = new SelectList(prescriptions, "Id", "PrescriptionNote", vm.PrescriptionId);

                return View(vm);
            }

            // Find prescription again (or use cached variable)
            var script = new PatientMedicationScript
            {
                PatientId = vm.PatientId,
                MedicationId = vm.MedicationId,
                Dosage = vm.Dosage,
                AdministeredDate = vm.AdministeredDate,
                ApplicationUserID = vm.ApplicationUserID,
                PrescriptionId = vm.RequiresPrescription ? vm.PrescriptionId : null,
                isActive = true
            };

            _context.PatientMedicationScripts.Add(script);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Medication successfully administered.";
            return RedirectToAction("ListAdministered");
        }


        public async Task<JsonResult> GetPrescriptions(int patientId, int medicationId)
        {
            // medicationId is now non-nullable
            var prescriptions = await _context.Prescriptions
                .Include(p => p.Prescribed_Medication)
                .Where(p => p.PatientId == patientId &&
                            p.Prescribed_Medication.Any(pm => pm.MedicationId == medicationId))
                .Select(p => new
                {
                    p.PrescriptionId/*,
                    p.PrescriptionInstruction*/
                })
                .ToListAsync();

            return Json(prescriptions);
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





        public async Task<IActionResult> EditAdministered(int id)
        {
            var script = await _context.PatientMedicationScripts.Include(p => p.Patient).Include(m => m.Medication).Include(a => a.AdministeredBy).FirstOrDefaultAsync(m => m.Id == id);

            if (script == null)
                return NotFound();

            var meds = _context.Medications/*.Where(m => m.Schedule >= 5)*/.Select(m => new
            {
                m.MedicationId,
                DisplayName = m.Name + " (Schedule " + m.Schedule + ")"
            }).ToList();

            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var loggedInUser = await _context.Users.FindAsync(loggedInUserId);

            // Load prescriptions for this patient and selected medication (if any)
            List<Prescription> prescriptions = new List<Prescription>();
            if (script.MedicationId != 0)
            {
                prescriptions = await _context.Prescriptions.Where(p => p.PatientId == script.PatientId).ToListAsync();
            }

            var vm = new AdministerMedicationViewModel
            {
                PrescriptionId = script.PrescriptionId,
                PatientId = script.PatientId,
                PatientName = $"{script.Patient.FirstName} {script.Patient.LastName}",
                MedicationId = script.MedicationId,
                Dosage = script.Dosage,
                AdministeredDate = script.AdministeredDate,
                ApplicationUserID = loggedInUser.Id,       // ← logged-in nurse
                ApplicationUserName = loggedInUser.FullName,
                MedicationList = new SelectList(meds, "MedicationId", "DisplayName", script.MedicationId),
                PrescriptionList = new SelectList(prescriptions.Select(p => new { p.PrescriptionId, Display = "Prescription #" + p.PrescriptionId }),"PrescriptionId","Display", script.PrescriptionId)

            };

            return View(vm);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministered(AdministerMedicationViewModel vm)
        {
            var med = await _context.Medications.FindAsync(vm.MedicationId);
            if (med == null) ModelState.AddModelError("", "Medication not found.");

            // Require prescription for schedule 5 and above
            if (med?.Schedule > 4)
            {
                var prescription = await _context.Prescriptions.Include(p => p.Prescribed_Medication).Where(p => p.PatientId == vm.PatientId && p.Prescribed_Medication.Any(pm => pm.MedicationId == vm.MedicationId))
                    .FirstOrDefaultAsync();

                if (prescription == null)
                {
                    ModelState.AddModelError("", "Prescription is required for Schedule 5 and above medication.");
                }
                else
                {
                    vm.PrescriptionId = prescription.PrescriptionId; // assign found prescription
                }
            }

            if (!ModelState.IsValid)
            {
                // Reload dropdowns
                var meds = _context.Medications
                    .Select(m => new
                    {
                        m.MedicationId,
                        DisplayName = m.Name + " (Schedule " + m.Schedule + ")"
                    })
                    .ToList();

                var nurses = _context.Users.Where(u => u.RoleType == "NursingSister").Select(u => new { u.Id, u.FullName }).ToList();
                vm.MedicationList = new SelectList(meds, "MedicationId", "DisplayName", vm.MedicationId);
                vm.UserList = new SelectList(nurses, "Id", "FullName", vm.ApplicationUserID);

                // Reload prescriptions for the patient if needed
                var prescriptions = await _context.Prescriptions.Where(p => p.PatientId == vm.PatientId).ToListAsync();

                vm.PrescriptionList = new SelectList(prescriptions.Select((p, index) => new
                {
                    p.PrescriptionId,
                    DisplayName = $"Prescription #{index + 1}"
                }), "PrescriptionId", "DisplayName", vm.PrescriptionId);



                return View(vm);
            }

            // Fetch existing record
            var existing = await _context.PatientMedicationScripts.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (existing == null) return NotFound();

            // Update entity properties from ViewModel
            existing.MedicationId = vm.MedicationId;
            existing.Dosage = vm.Dosage;
            existing.AdministeredDate = vm.AdministeredDate;
            existing.PrescriptionId = vm.PrescriptionId;
            // existing.ApplicationUserID = vm.ApplicationUserID;
            // existing.ApplicationUserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            existing.ApplicationUserID = loggedInUserId;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Medication updated successfully.";
            return RedirectToAction("ListAdministered");
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
                TempData["SuccessMessage"] = "Medication successfully Deleted!";
            }
            return RedirectToAction(nameof(ListAdministered));
        }


        public async Task<IActionResult> LiveSearch(string query, string ward, string bed)
        {
            var wards = await _context.Wards.ToListAsync();

            // Pass to the view
            ViewBag.Wards = new SelectList(wards, "WardID", "Name");
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
                patientsQuery = patientsQuery.Where(p => p.Admissions != null && p.Admissions.Ward.Name == ward);
            }

            if (!string.IsNullOrWhiteSpace(bed))
            {
                patientsQuery = patientsQuery.Where(p =>
                    p.Admissions != null &&
                    p.Admissions.Bed != null &&
                    p.Admissions.Bed.BedNo.StartsWith(bed));
            }

            var patients = await patientsQuery.ToListAsync();

            var viewModel = patients.Select(p => new PatientDashboardViewModel
            {
                PatientID = p.PatientID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                WardName = p.Admissions?.Ward?.Name ?? "N/A",
                BedNo = p.Admissions?.Bed?.BedNo ?? "N/A",
                Status = p.Admissions != null ? "Admitted" : "Not Admitted"
            }).ToList();

            
            return PartialView("_PatientSearchResults", viewModel);
        }

        public async Task<IActionResult> GetDosage(int prescriptionId, int medicationId)
        {
            var prescribedMed = await _context.PrescribeMedications
                .FirstOrDefaultAsync(pm => pm.PrescriptionId == prescriptionId && pm.MedicationId == medicationId);

            if (prescribedMed == null)
                return NotFound();

            return Json(new { dosage = prescribedMed.Dosage });
        }


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


        /* public async Task<IActionResult> DownloadPrescription(int id)
         {
             var prescription = await _context.Prescriptions
                 .Include(p => p.Patient)
                 .Include(p => p.User)
                 .Include(p => p.Prescribed_Medication)
                     .ThenInclude(pm => pm.Medication)
                 .FirstOrDefaultAsync(p => p.PrescriptionId == id);

             if (prescription == null) return NotFound();

             using (var ms = new MemoryStream())
             {
                 var doc = new Document(PageSize.A4, 50, 50, 50, 50);
                 PdfWriter.GetInstance(doc, ms);
                 doc.Open();

                 // Title
                 var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                 doc.Add(new Paragraph("Prescription", titleFont));
                 doc.Add(new Paragraph("\n"));

                 // Patient & Doctor Info
                 doc.Add(new Paragraph($"Prescription ID: {prescription.PrescriptionId}"));
                 doc.Add(new Paragraph($"Patient: {prescription.Patient.FirstName} {prescription.Patient.LastName}"));
                 doc.Add(new Paragraph($"Issued By: {prescription.User.FullName}"));
                 doc.Add(new Paragraph($"Date Issued: {prescription.DateIssued.ToString("yyyy-MM-dd")}"));
                 doc.Add(new Paragraph("\n"));

                 // Prescription instruction
                 doc.Add(new Paragraph("Instruction:"));
                 //doc.Add(new Paragraph(prescription.PrescriptionInstruction ?? "N/A"));
                 doc.Add(new Paragraph("\n"));

                 // Table of Medications
                 if (prescription.Prescribed_Medication.Any())
                 {
                     var table = new PdfPTable(3); // Columns: Medication, Dosage, Schedule
                     table.WidthPercentage = 100;

                     // Header
                     var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                     table.AddCell(new Phrase("Medication", headerFont));
                     table.AddCell(new Phrase("Dosage", headerFont));
                     table.AddCell(new Phrase("Schedule", headerFont));

                     foreach (var pm in prescription.Prescribed_Medication)
                     {
                         table.AddCell(pm.Medication?.Name ?? "N/A");
                         table.AddCell(pm.Dosage ?? "N/A");
                         table.AddCell(pm.Medication?.Schedule.ToString() ?? "N/A");
                     }

                     doc.Add(table);
                 }
                 else
                 {
                     doc.Add(new Paragraph("No medications listed for this prescription."));
                 }

                 doc.Close();

                 return File(ms.ToArray(), "application/pdf", $"Prescription_{prescription.Patient.FirstName}_{prescription.Patient.LastName}.pdf");
             }
         }*/

        public async Task<IActionResult> DownloadPrescription(int id)
        {
            var prescription = await _context.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.User)
                .Include(p => p.Prescribed_Medication)
                    .ThenInclude(pm => pm.Medication)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescription == null) return NotFound();

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // ======================
                // HEADER (Logo + Company)
                // ======================
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;

                // Logo (from wwwroot/images/logo.png)
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
                if (System.IO.File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleAbsolute(60f, 60f);
                    PdfPCell logoCell = new PdfPCell(logo);
                    logoCell.Border = Rectangle.NO_BORDER;
                    logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    headerTable.AddCell(logoCell);
                }
                else
                {
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
                }

                // Company name
                var companyFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.Black);
                PdfPCell companyCell = new PdfPCell(new Phrase("CyberMed-Care / DevDynamo", companyFont));
                companyCell.Border = Rectangle.NO_BORDER;
                companyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                companyCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(companyCell);

                doc.Add(headerTable);
                doc.Add(new Paragraph("\n"));

                // ======================
                // Title
                // ======================
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                doc.Add(new Paragraph("Prescription", titleFont));
                doc.Add(new Paragraph("\n"));

                // ======================
                // Patient & Doctor Info
                // ======================
                doc.Add(new Paragraph($"Prescription ID: {prescription.PrescriptionId}"));
                doc.Add(new Paragraph($"Patient: {prescription.Patient.FirstName} {prescription.Patient.LastName}"));
                doc.Add(new Paragraph($"Issued By: {prescription.User.FullName}"));
                doc.Add(new Paragraph($"Date Issued: {prescription.DateIssued.ToString("yyyy-MM-dd")}"));
                doc.Add(new Paragraph("\n"));

                
                // ======================
                // Table of Medications
                // ======================
                if (prescription.Prescribed_Medication.Any())
                {
                    var table = new PdfPTable(3); // Columns: Medication, Dosage, Schedule
                    table.WidthPercentage = 100;

                    // Header
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                    table.AddCell(new Phrase("Medication", headerFont));
                    table.AddCell(new Phrase("Dosage", headerFont));
                    table.AddCell(new Phrase("Schedule", headerFont));

                    foreach (var pm in prescription.Prescribed_Medication)
                    {
                        table.AddCell(pm.Medication?.Name ?? "N/A");
                        table.AddCell(pm.Dosage ?? "N/A");
                        table.AddCell(pm.Medication?.Schedule.ToString() ?? "N/A");
                    }

                    doc.Add(table);
                }
                else
                {
                    doc.Add(new Paragraph("No medications listed for this prescription."));
                }

                doc.Close();

                return File(ms.ToArray(), "application/pdf",
                    $"Prescription_{prescription.Patient.FirstName}_{prescription.Patient.LastName}.pdf");
            }
        }





    }
}
