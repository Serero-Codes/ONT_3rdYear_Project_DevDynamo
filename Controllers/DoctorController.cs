using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using ONT_3rdyear_Project.Data;
using ONT_3rdyear_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ONT_3rdyear_Project.Controllers
{
	[Authorize(Roles = "Doctor")]
	public class DoctorController : Controller
	{
		private readonly ApplicationDbContext _context;

		public DoctorController(ApplicationDbContext context)
		{
			_context = context;
		}

		//// 🗂 View Patient Folder
		//public async Task<IActionResult> PatientFolder(int id)
		//{
		//	var patient = await _context.patient
		//		.Include(p => p.vitals)
		//		.Include(p => p.treatments)
		//		.Include(p => p.medical_History)
		//		.Include(p => p.patient_Allergie)
		//			.ThenInclude(pa => pa.Allergy)
		//		.FirstOrDefaultAsync(p => p.PatientID == id);

		//	if (patient == null)
		//		return NotFound();

		//	ViewBag.Visits = await _context.treat_Visit
		//		.Where(v => v.PatientID == id)
		//		.Include(v => v.employee)
		//		.Include(v => v.instruction)
		//		.OrderByDescending(v => v.VisitDate)
		//		.ToListAsync();

		//	//ViewBag.Prescriptions = await _context.prescribe_Madication
		//	//	.Where(p => p.PatientID == id)
		//	//	.Include(p => p.Medication)
		//	//	.OrderByDescending(p => p.CreatedAt)
		//	//	.ToListAsync();
		//	//ViewBag.Prescriptions = await _context.prescribe_Madication

		//	var prescribedMeds = await _context.prescribe_Madication
		//		.Include(pm => pm.Medication)
		//		.Include(pm => pm.Prescription)
		//		.ThenInclude(p => p.Patient)
		//		.Where(pm => pm.Prescription.PatientId == id)
		//		.ToListAsync();
		//	ViewBag.prescriptions = prescribedMeds;
		//	return View(patient);
		//}

		//// 💉 Visit Patient
		//public IActionResult VisitPatient(int id)
		//{
		//	ViewBag.PatientId = id;
		//	return View();
		//}

		//[HttpPost]
		//public async Task<IActionResult> VisitPatient(int id, string procedureNote)
		//{
		//	var visit = new Treat_Visit
		//	{
		//		PatientID = id,
		//		EmployeeID = employee.EmployeeID,
		//		VisitDate = DateTime.Now,
		//		Notes = procedureNote,
		//		IsCompleted = false
		//	};

		//	_context.treat_Visit.Add(visit);
		//	await _context.SaveChangesAsync();

		//	return RedirectToAction("PatientFolder", new { id });
		//}

		//// 📝 Write Instruction for Visit
		//public IActionResult WriteInstruction(int visitId)
		//{
		//	ViewBag.VisitId = visitId;
		//	return View();
		//}

		//[HttpPost]
		//public async Task<IActionResult> WriteInstruction(int visitId, string instruction)
		//{
		//	var visit = await _context.treat_Visit.FindAsync(visitId);
		//	if (visit == null)
		//		return NotFound();

		//	var note = new VisitInstruction
		//	{
		//		TreatVisitID = visitId,
		//		Instruction = instruction,
		//		CreatedAt = DateTime.Now,
		//		CreatedBy = User.Identity!.Name
		//	};
		//	//var Instruction = await _context.visitInstruction.FindAsync(visitId);
		//	_context.visitInstruction.Add(note);
		//	await _context.SaveChangesAsync();

		//	return RedirectToAction("PatientFolder", new { id = visit.PatientID });
		//}

		//// 💊 Prescribe Medication
		//public IActionResult PrescribeMedication(int id)
		//{
		//	ViewBag.PatientId = id;
		//	ViewBag.Medications = _context.medication.ToList();
		//	return View();
		//}

		//[HttpPost]
		//public async Task<IActionResult> PrescribeMedication(int patientId, int medicationId, string dosage, int durationDays)
		//{
		//	var prescription = new Prescribe_Madication
		//	{
		//		PatientID = patientId,
		//		MedicationID = medicationId,
		//		Dosage = dosage,
		//		DurationInDays = durationDays,
		//		PrescribedBy = User.Identity!.Name,
		//		DateIssued = DateTime.Now
		//	};

		//	_context.prescribe_Madication.Add(prescription);
		//	await _context.SaveChangesAsync();

		//	return RedirectToAction("PatientFolder", new { id = patientId });
		//}

		//// 📤 Discharge Patient
		//[HttpPost]
		//public async Task<IActionResult> DischargePatient(int id)
		//{
		//	var patient = await _context.patient.FindAsync(id);
		//	if (patient == null)
		//		return NotFound();

		//	var discharge = await (from d in _context.discharge
		//						   join p in _context.patient on d.PatientID equals p.PatientID
		//						   where p.PatientID == id && d.IsDischarged == false
		//						   select d).FirstOrDefaultAsync();

		//	if (discharge == null)
		//		return BadRequest("Patient already discharged or discharge record not found.");


		//	discharge.IsDischarged = true;
		//	discharge.DischargeDate = DateTime.Now;
			

		//	await _context.SaveChangesAsync();
		//	return RedirectToAction("Index", "Dashboard");
		//}

		//// 🗓️ Schedule Visit (Optional for future visits)
		//public IActionResult CreateVisitSchedule(int patientId)
		//{
		//	ViewBag.PatientId = patientId;
		//	return View();
		//}

		//[HttpPost]
		//public async Task<IActionResult> CreateVisitSchedule(VisitSchedule visit)
		//{
			
		//	visit.EmployeeID = employee.EmployeeID;
		//	visit.VisitDate = DateTime.Now; // Use input if scheduling ahead

		//	_context.visitSchedule.Add(visit);
		//	await _context.SaveChangesAsync();

		//	return RedirectToAction("PatientFolder", new { id = visit.PatientID });
		//}
	}
}
