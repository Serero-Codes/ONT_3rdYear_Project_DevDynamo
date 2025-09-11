using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using ONT_3rdyear_Project.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

            var totalVitals = await _context.Vitals.Where(v=>v.IsActive).CountAsync();
            var model = new DashboardViewModel
            {
                Stats = new DashboardStatsViewModel
                {
                    TotalPatients = totalPatients,
                    TreatmentsToday = treatmentsToday,
                    MedicationsGivenToday = medicationsGivenToday,
                    //HoursOnDuty = 24 // Replace with actual logic if needed
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
                join d in _context.Discharges on p.PatientID equals d.PatientID into dischargeGroup
                from d in dischargeGroup.OrderByDescending(x => x.DischargeDate).Take(1).DefaultIfEmpty()
                select new PatientDashboardViewModel
                {
                    PatientID = p.PatientID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    WardName = w != null ? w.Name : "N/A",
                    BedNo = b != null ? b.BedNo : "N/A",
                    /*Status = a != null ? "Admitted" : "Not Admitted"*/
                    Status = d != null && d.IsDischarged ? "Discharged" :
                     a != null ? "Admitted" : "Not Admitted"

                }
            ).ToListAsync();

            return View(data);
        }



















        //CRUD for Vitals
        /* public async Task<IActionResult> Vitals()
         {
             var vitals = await _context.Vitals.Include(v => v.Patient).Include(v=>v.VisitSchedule).Include(u=>u.User).Where(v=> v.IsActive).OrderByDescending(v => v.Date).ToListAsync();
             return View(vitals);
         }*/

        public async Task<IActionResult> Vitals(string searchPatient, DateTime? fromDate, DateTime? toDate)
        {
            var vitalsQuery = _context.Vitals
                .Include(v => v.Patient)
                .Include(v => v.VisitSchedule)
                .Include(v => v.User)
                .Where(v => v.IsActive);

            if (!string.IsNullOrEmpty(searchPatient))
            {
                vitalsQuery = vitalsQuery.Where(v =>
                    v.Patient.FirstName.Contains(searchPatient) ||
                    v.Patient.LastName.Contains(searchPatient));
            }

            if (fromDate.HasValue)
            {
                vitalsQuery = vitalsQuery.Where(v => v.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                vitalsQuery = vitalsQuery.Where(v => v.Date <= toDate.Value);
            }

            var vitals = await vitalsQuery.OrderByDescending(v => v.Date).ToListAsync();
            return View(vitals);
        }

        public async Task<IActionResult> VitalsPdf()
        {
            var vitals = await _context.Vitals
                .Include(v => v.Patient)
                .Include(v => v.User)
                .Where(v => v.IsActive)
                .OrderByDescending(v => v.Date)
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                var doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 60, 40);
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Define fonts and colors
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.DarkGray);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.White);
                var subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.DarkGray);
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.Black);
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.Gray);

                // Title section with logo space
                var titleTable = new PdfPTable(2) { WidthPercentage = 100 };
                titleTable.SetWidths(new float[] { 3, 1 });

                var titleCell = new PdfPCell(new Phrase("VITALS REPORT", titleFont))
                {
                    Border = Rectangle.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 10
                };

                var logoCell = new PdfPCell(new Phrase("", bodyFont)) // Space for logo if needed
                {
                    Border = Rectangle.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };

                titleTable.AddCell(titleCell);
                titleTable.AddCell(logoCell);
                doc.Add(titleTable);

                // Report info section
                var infoTable = new PdfPTable(2) { WidthPercentage = 100 };
                infoTable.SetWidths(new float[] { 1, 1 });

                var generateDateCell = new PdfPCell(new Phrase($"Generated: {DateTime.Now:MMM dd, yyyy HH:mm}", subHeaderFont))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                };

                var totalRecordsCell = new PdfPCell(new Phrase($"Total Records: {vitals.Count}", subHeaderFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingBottom = 5
                };

                infoTable.AddCell(generateDateCell);
                infoTable.AddCell(totalRecordsCell);
                doc.Add(infoTable);

                // Add separator line
                var line = new LineSeparator(1f, 100f, BaseColor.LightGray, Element.ALIGN_CENTER, -2);
                doc.Add(new Chunk(line));
                doc.Add(new Paragraph(" "));

                if (vitals.Any())
                {
                    // Create main data table
                    var table = new PdfPTable(7) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 2.5f, 2f, 1.2f, 1.2f, 1.2f, 1.2f, 2f });

                    // Header row
                    var headerBg = new BaseColor(70, 130, 180); // Steel blue
                    string[] headers = { "Date & Time", "Patient", "BP", "Sugar", "Temp", "Pulse", "Recorded By" };

                    foreach (string header in headers)
                    {
                        var headerCell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = headerBg,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 8,
                            Border = Rectangle.BOX,
                            BorderColor = BaseColor.White,
                            BorderWidth = 1
                        };
                        table.AddCell(headerCell);
                    }

                    // Data rows with alternating colors
                    var lightBlue = new BaseColor(240, 248, 255); // Alice blue
                    bool isAlternate = false;

                    foreach (var vital in vitals)
                    {
                        var rowColor = isAlternate ? lightBlue : BaseColor.White;

                        // Date cell
                        var dateCell = new PdfPCell(new Phrase(vital.Date.ToString("MMM dd, yyyy\nHH:mm"), bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(dateCell);

                        // Patient cell
                        var patientName = $"{vital.Patient?.FirstName ?? "N/A"} {vital.Patient?.LastName ?? ""}".Trim();
                        var patientCell = new PdfPCell(new Phrase(patientName, bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(patientCell);

                        // Vital signs cells
                        var bpCell = new PdfPCell(new Phrase(vital.BP?.ToString() ?? "-", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(bpCell);


                        var sugarCell = new PdfPCell(new Phrase(vital.SugarLevel.ToString() ?? "-", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(sugarCell);

                        var tempCell = new PdfPCell(new Phrase(vital.Temperature.ToString("F1") ?? "-", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(tempCell);

                        var pulseCell = new PdfPCell(new Phrase(vital.PulseRate.ToString() ?? "-", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(pulseCell);

                        // Recorded by cell
                        var recordedByCell = new PdfPCell(new Phrase(vital.User?.FullName ?? "Unknown", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        };
                        table.AddCell(recordedByCell);

                        isAlternate = !isAlternate;
                    }

                    doc.Add(table);
                }
                else
                {
                    // No data message
                    var noDataParagraph = new Paragraph("No active vital records found.", subHeaderFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 20,
                        SpacingAfter = 20
                    };
                    doc.Add(noDataParagraph);
                }

                // Footer
                doc.Add(new Paragraph(" "));
                doc.Add(new Chunk(line));

                var footerTable = new PdfPTable(3) { WidthPercentage = 100 };
                footerTable.SetWidths(new float[] { 1, 1, 1 });

                var leftFooter = new PdfPCell(new Phrase("Healthcare Management System", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingTop = 10
                };

                var centerFooter = new PdfPCell(new Phrase($"Page 1", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 10
                };

                var rightFooter = new PdfPCell(new Phrase("Confidential", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingTop = 10
                };

                footerTable.AddCell(leftFooter);
                footerTable.AddCell(centerFooter);
                footerTable.AddCell(rightFooter);
                doc.Add(footerTable);

                doc.Close();
                return File(ms.ToArray(), "application/pdf", $"VitalsReport_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
            }
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
            if (vital == null)
                return NotFound();

            vital.IsActive = false; // soft delete
            _context.Vitals.Update(vital);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
































        //TREATMENT CRUD
       /* public async Task<IActionResult> Treatments()
        {
            var treatments = await _context.Treatments.Include(p=>p.Patient).Include(u=>u.User).Include(v=>v.VisitSchedule).Where(t=>t.IsActive).ToListAsync();
            return View(treatments);
        }*/
       public async Task<IActionResult>Treatments(string searchPatient, DateTime? fromDate, DateTime? toDate)
        {
            var treatmentQuery = _context.Treatments.Include(p => p.Patient).Include(p => p.User).Include(p => p.VisitSchedule).Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(searchPatient))
            {
                treatmentQuery = treatmentQuery.Where(p => p.Patient.FirstName.Contains(searchPatient) || p.Patient.LastName.Contains(searchPatient));
            }

            if (fromDate.HasValue)
            {
                treatmentQuery = treatmentQuery.Where(p => p.TreatmentDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                treatmentQuery = treatmentQuery.Where(p => p.TreatmentDate <= toDate.Value);
            }
            var treatments = await treatmentQuery.OrderByDescending(p => p.TreatmentDate).ToListAsync();
            return View(treatments);
        }
        public async Task<IActionResult> TreatmentsPdf()
        {
            var treatments = await _context.Treatments
                .Include(v => v.Patient)
                .Include(v => v.User)
                .Where(v => v.IsActive)
                .OrderByDescending(v => v.TreatmentDate)
                .ToListAsync();

            using (var ms = new MemoryStream())
            {
                var doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 60, 40);
                var writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();

                // Define fonts and colors
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.DarkGray);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.White);
                var subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.DarkGray);
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.Black);
                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.Gray);

                // Title section with logo space
                var titleTable = new PdfPTable(2) { WidthPercentage = 100 };
                titleTable.SetWidths(new float[] { 3, 1 });

                var titleCell = new PdfPCell(new Phrase("TREATMENTS REPORT", titleFont))
                {
                    Border = Rectangle.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 10
                };

                var logoCell = new PdfPCell(new Phrase("", bodyFont)) // Space for logo if needed
                {
                    Border = Rectangle.NO_BORDER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };

                titleTable.AddCell(titleCell);
                titleTable.AddCell(logoCell);
                doc.Add(titleTable);

                // Report info section
                var infoTable = new PdfPTable(2) { WidthPercentage = 100 };
                infoTable.SetWidths(new float[] { 1, 1 });

                var generateDateCell = new PdfPCell(new Phrase($"Generated: {DateTime.Now:MMM dd, yyyy HH:mm}", subHeaderFont))
                {
                    Border = Rectangle.NO_BORDER,
                    PaddingBottom = 5
                };

                var totalRecordsCell = new PdfPCell(new Phrase($"Total Records: {treatments.Count}", subHeaderFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingBottom = 5
                };

                infoTable.AddCell(generateDateCell);
                infoTable.AddCell(totalRecordsCell);
                doc.Add(infoTable);

                // Add separator line
                var line = new LineSeparator(1f, 100f, BaseColor.LightGray, Element.ALIGN_CENTER, -2);
                doc.Add(new Chunk(line));
                doc.Add(new Paragraph(" "));

                if (treatments.Any())
                {
                    // Create main data table
                    var table = new PdfPTable(4) { WidthPercentage = 100 };
                    table.SetWidths(new float[] { 2.5f, 2f, 2f, 2f });

                    // Header row
                    var headerBg = new BaseColor(70, 130, 180); // Steel blue
                    string[] headers = { "Date & Time", "Patient", "Treatment Type", "Treated By"};

                    foreach (string header in headers)
                    {
                        var headerCell = new PdfPCell(new Phrase(header, headerFont))
                        {
                            BackgroundColor = headerBg,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Padding = 8,
                            Border = Rectangle.BOX,
                            BorderColor = BaseColor.White,
                            BorderWidth = 1
                        };
                        table.AddCell(headerCell);
                    }

                    // Data rows with alternating colors
                    var lightBlue = new BaseColor(240, 248, 255); // Alice blue
                    bool isAlternate = false;

                    foreach (var treatment in treatments)
                    {
                        var rowColor = isAlternate ? lightBlue : BaseColor.White;

                        // Date cell
                        table.AddCell(new PdfPCell(new Phrase(
                            treatment.TreatmentDate.ToString("MMM dd, yyyy\nHH:mm"), bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        // Patient cell
                        table.AddCell(new PdfPCell(new Phrase(
                            $"{treatment.Patient?.FirstName} {treatment.Patient?.LastName}", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        // Treatment type
                        table.AddCell(new PdfPCell(new Phrase(
                            treatment.TreatmentType ?? "N/A", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        // Treated by (User)
                        table.AddCell(new PdfPCell(new Phrase(
                            treatment.User?.FullName ?? "Unknown", bodyFont))
                        {
                            BackgroundColor = rowColor,
                            Padding = 6,
                            VerticalAlignment = Element.ALIGN_MIDDLE
                        });

                        isAlternate = !isAlternate;
                    }

                    doc.Add(table);
                }
                else
                {
                    // No data message
                    var noDataParagraph = new Paragraph("No active treatment records found.", subHeaderFont)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingBefore = 20,
                        SpacingAfter = 20
                    };
                    doc.Add(noDataParagraph);
                }

                // Footer
                doc.Add(new Paragraph(" "));
                doc.Add(new Chunk(line));

                var footerTable = new PdfPTable(3) { WidthPercentage = 100 };
                footerTable.SetWidths(new float[] { 1, 1, 1 });

                var leftFooter = new PdfPCell(new Phrase("Healthcare Management System", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingTop = 10
                };

                var centerFooter = new PdfPCell(new Phrase($"Page 1", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop = 10
                };

                var rightFooter = new PdfPCell(new Phrase("Confidential", footerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    PaddingTop = 10
                };

                footerTable.AddCell(leftFooter);
                footerTable.AddCell(centerFooter);
                footerTable.AddCell(rightFooter);
                doc.Add(footerTable);

                doc.Close();
                return File(ms.ToArray(), "application/pdf", $"TreatmentsReport_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
            }
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

            var treatment = await _context.Treatments.Include(p => p.Patient).Include(t=>t.TreatVisit).Include(t=>t.VisitSchedule).Include(a => a.User).FirstOrDefaultAsync(t => t.TreatmentID == id );

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

            //return RedirectToAction(nameof(Treatments));\
            return Ok();

        }











































        //administer medication
        /*public async Task<IActionResult> Administered()
        {
            var medication = await _context.PatientMedicationScripts.Include(p=>p.Patient).Include(v=>v.VisitSchedule).Include(p => p.Prescription).Include(a=>a.AdministeredBy).Include(m=>m.Medication).Where(ad=>ad.isActive).ToListAsync();
            return View(medication);
        }*/
        public async Task<IActionResult> Administered(string searchPatient, DateTime? fromDate, DateTime? toDate)
        {
            var medsQuery = _context.PatientMedicationScripts
                .Include(p => p.Patient)
                .Include(v => v.VisitSchedule)
                .Include(p => p.Prescription)
                .Include(a => a.AdministeredBy)
                .Include(m => m.Medication)
                .Where(m => m.isActive); 

            if (!string.IsNullOrEmpty(searchPatient))
            {
                medsQuery = medsQuery.Where(m =>
                    m.Patient.FirstName.Contains(searchPatient) ||
                    m.Patient.LastName.Contains(searchPatient));
            }

            if (fromDate.HasValue)
            {
                medsQuery = medsQuery.Where(m => m.AdministeredDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var endOfDay = toDate.Value.Date.AddDays(1).AddTicks(-1);
                medsQuery = medsQuery.Where(m => m.AdministeredDate <= endOfDay);
            }

            var medications = await medsQuery
                .OrderByDescending(m => m.AdministeredDate)
                .ToListAsync();

            return View(medications);
        }

        public async Task<IActionResult> AdministeredPdf()
        {
            var medications = await _context.PatientMedicationScripts
                .Include(p => p.Patient)
                .Include(m => m.Medication)
                .Include(a => a.AdministeredBy)
                .Include(p => p.Prescription)
                .Where(m => m.isActive)
                .OrderByDescending(m => m.AdministeredDate)
                .ToListAsync();

            var stream = new MemoryStream();
                var doc = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter.GetInstance(doc, stream).CloseStream = false;
                doc.Open();

                // Title
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                doc.Add(new Paragraph("Administered Medications Report", titleFont));
                doc.Add(new Paragraph($"Generated on: {DateTime.Now}", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                doc.Add(new Paragraph("\n"));

                // Table
                PdfPTable table = new PdfPTable(6); // 5 columns
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 2, 1, 2, 2, 2 });

                // Header row
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                string[] headers = { "Patient", "Medication", "Dosage", "Administered By", "Prescription", "Date" };
                foreach (var h in headers)
                {
                    table.AddCell(new PdfPCell(new Phrase(h, headerFont))
                    {
                        BackgroundColor = BaseColor.LightGray,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });
                }

                // Data rows
                foreach (var med in medications)
                {
                    table.AddCell($"{med.Patient?.FirstName} {med.Patient?.LastName}");
                    table.AddCell(med.Medication?.Name ?? "N/A");
                    table.AddCell(med.Dosage ?? "N/A");
                    table.AddCell(med.AdministeredBy?.FullName ?? "N/A");
                    table.AddCell(med.Prescription != null ? "Yes" : "No");
                    table.AddCell(med.AdministeredDate.ToString("yyyy-MM-dd HH:mm"));
                }

                doc.Add(table);
                doc.Close();

                stream.Position = 0;
                return File(stream, "application/pdf", "AdministeredMedications.pdf");
          
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdminister(int patientId)
        {
            
            var patient = await _context.Patients.Include(p => p.PatientAllergies).ThenInclude(pa => pa.Allergy).FirstOrDefaultAsync(p => p.PatientID == patientId);
            if (patient == null)
            {
                return NotFound();
            }

          

            ViewBag.PatientId = patient.PatientID;
            ViewBag.PatientName = $"{patient.FirstName} {patient.LastName}";
            ViewBag.PatientAllergies = patient.PatientAllergies.Select(pa => pa.Allergy.Name).ToList();
            
            ViewBag.MedicationList = _context.Medications.ToList();
            ViewBag.UserList = new SelectList(_context.Users.ToList(), "ApplicationUserID", "FullName");
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminister(PatientMedicationScript model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                model.ApplicationUserID = user.Id; 
            }
            // Load the selected medication to check its schedule
            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == model.MedicationId);
            var patient = await _context.Patients.Include(p => p.PatientAllergies).ThenInclude(pa => pa.Allergy).FirstOrDefaultAsync(p => p.PatientID == model.PatientId);
            var patientAllergies = patient?.PatientAllergies.Select(pa => pa.Allergy.Name).ToList() ?? new List<string>();


            if (medication == null)
            {
                ModelState.AddModelError("", "Invalid medication selected.");
            }
            else if (medication.Schedule > 4)
            {
                ModelState.AddModelError("", "You cannot administer medication with a schedule higher than 4.");
            }
            else if (patientAllergies.Any(a => medication.Name.Contains(a, StringComparison.OrdinalIgnoreCase)))
                ModelState.AddModelError("", "⚠ This patient is allergic to the selected medication!");


            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns when returning view
                
                ViewBag.PatientId = patient?.PatientID;
                ViewBag.PatientName = $"{patient?.FirstName} {patient?.LastName}";
                ViewBag.MedicationList = _context.Medications.ToList();
                ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", model.ApplicationUserID);
                ViewBag.PatientAllergies = patientAllergies;

                return View(model);
            }

            try
            {
                if (medication.Quantity > 0)
                {
                    medication.Quantity -= 1; // Assuming one unit per administration
                }
                else
                {
                    ModelState.AddModelError("", "Medication is out of stock!");
                    ViewBag.PatientId = patient?.PatientID;
                    ViewBag.PatientName = $"{patient?.FirstName} {patient?.LastName}";
                    ViewBag.MedicationList = _context.Medications.ToList();
                    ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", model.ApplicationUserID);
                    ViewBag.PatientAllergies = patientAllergies;
                    return View(model);
                }

                _context.PatientMedicationScripts.Add(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medication administered successfully!";
                return RedirectToAction(nameof(Administered));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "An error has occurred.");
            }

            // In case of error, reload dropdowns
            var patientReload = await _context.Patients.FindAsync(model.PatientId);
            
            ViewBag.PatientId = patient?.PatientID;
            ViewBag.PatientName = $"{patient?.FirstName} {patient?.LastName}";
            ViewBag.MedicationList = _context.Medications.ToList();
            ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", model.ApplicationUserID);
            ViewBag.PatientAllergies = patientAllergies;
            return View(model);
        }


        



        public async Task<IActionResult> EditAdministered(int? id)
        {
            if (id == null)
                return NotFound();

            var medication = await _context.PatientMedicationScripts.Include(p => p.Patient).Include(m => m.Medication).Include(a => a.AdministeredBy).Include(v => v.VisitSchedule).FirstOrDefaultAsync(m => m.Id == id);

            
            if (medication == null)
                return NotFound();

            
            ViewBag.MedicationList = _context.Medications.ToList();
            ViewBag.UserList = new SelectList(_context.Users, "ApplicationUserID", "FullName", medication.ApplicationUserID);
            return View(medication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdministered(int id, PatientMedicationScript model)
        {
            if (id != model.Id)
                return NotFound();

            var existing = await _context.PatientMedicationScripts.FindAsync(id);
            if (existing == null)
                return NotFound();

            var medication = await _context.Medications.FirstOrDefaultAsync(m => m.MedicationId == model.MedicationId);

            if (medication == null)
            {
                ModelState.AddModelError("", "Invalid medication selected.");
            }
            else if (medication.Schedule > 4)
            {
                ModelState.AddModelError("", "You cannot administer medication with a schedule higher than 4.");
            }

            if (!ModelState.IsValid)
            {
                // Re-populate dropdowns
                ViewBag.MedicationList = new SelectList(_context.Medications.ToList(), "MedicationId", "Name", model.MedicationId);
                return View(model);
            }

            // Update only editable fields
            existing.MedicationId = model.MedicationId;
            
            existing.ApplicationUserID = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            existing.AdministeredDate = model.AdministeredDate;
            existing.Dosage = model.Dosage;
            
            

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

                
                // HEADER (Logo + Company)
                
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

                return File(ms.ToArray(), "application/pdf",
                    $"Prescription_{prescription.Patient.FirstName}_{prescription.Patient.LastName}.pdf");
            }


        }












        public async Task<IActionResult> InstructionList()
        {
            var instructions = await _context.Instructions.Include(i => i.Patient).ToListAsync();

            return View(instructions);
        }


        public async Task<IActionResult> ViewAdvice(int patientId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                return NotFound(); // patient itself doesn't exist

            var instruction = await _context.Instructions.Include(i => i.Patient).FirstOrDefaultAsync(i => i.PatientID == patientId);

            if (instruction == null)
            {
                // Create a temporary instruction object with fallback text
                instruction = new Instruction
                {
                    Patient = patient,
                    Instructions = "No advice from doctor"
                };
            }

            return View(instruction);
        }

        public async Task<IActionResult> DoctorResponse(int id)
        {
            var instruction = await _context.Instructions.Include(i => i.Patient).FirstOrDefaultAsync(i => i.InstructionID == id);

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

        public async Task<IActionResult> EditRequest(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var request = await _context.Instructions.Include(i => i.Patient).FirstOrDefaultAsync(i => i.InstructionID == id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequest(int? id, Instruction request)
        {
            if(id != request.InstructionID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingRequest = await _context.Instructions.FindAsync(id);
                    if(existingRequest == null)
                    {
                        return NotFound();
                    }
                    existingRequest.NurseRequest = request.NurseRequest;
                    _context.Update(existingRequest);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Request successfully updated!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Instructions.Any(e => e.InstructionID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(InstructionList)); 
            }
            return View(request);
        }

        public async Task<IActionResult> DeleteRequest(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var request = await _context.Instructions.Include(p => p.Patient).FirstOrDefaultAsync(p => p.InstructionID == id);
            if(request == null)
            {
                return NotFound();
            }
            return View(request);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteRequest(int id)
        {
            var request = await _context.Instructions.FindAsync(id);
            if(request == null)
            {
                return NotFound();
            }
            _context.Instructions.Remove(request);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Request Successfully Deleted!";
            return RedirectToAction(nameof(InstructionList));
        }



        public async Task<IActionResult> LiveSearch(string query, string ward, string bed)
        {
            var wards = await _context.Wards.ToListAsync();

            // Pass to the view
            ViewBag.Wards = new SelectList(wards, "WardID", "Name");
            var patientsQuery = _context.Patients.Include(p => p.Admissions).ThenInclude(a => a.Ward).Include(p => p.Admissions).ThenInclude(a => a.Bed).AsQueryable();

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

        public async Task<IActionResult> LiveSearchAll()
        {
            var patients = await _context.Patients.Include(p => p.Admissions).ThenInclude(a => a.Ward).Include(p => p.Admissions).ThenInclude(a => a.Bed).ToListAsync();

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
            var results = _context.Vitals.Include(v => v.Patient).Include(v => v.User).Where(v => v.Patient.FirstName.Contains(query) || v.Patient.LastName.Contains(query)).ToList();

            return PartialView("_VitalSearchResultsPartial", results);
        }

        public IActionResult LiveSearchAllVitals()
        {
            var allVitals = _context.Vitals.Include(v => v.Patient).Include(v => v.User).ToList();

            return PartialView("_VitalSearchResultsPartial", allVitals);
        }

        public IActionResult LiveSearchTreatments(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return LiveSearchAllTreatments();
            }

            var results = _context.Treatments.Include(v => v.Patient).Include(v => v.User).Where(v => v.Patient != null && (EF.Functions.Like(v.Patient.FirstName, $"%{query}%") || EF.Functions.Like(v.Patient.LastName, $"%{query}%")))
                .ToList();

            return PartialView("_TreatmentSearchResultsPartial", results);
        }



        public IActionResult LiveSearchAllTreatments()
        {
            var allTreatments = _context.Treatments.Include(v => v.Patient).Include(v => v.User).ToList();

            return PartialView("_TreatmentSearchResultsPartial", allTreatments);
        }






    }
}
