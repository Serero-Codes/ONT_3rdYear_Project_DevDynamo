using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.Models
{
	public class Patient
	{
		[Key]
		public int PatientID { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Name must not be greater than be 50 charecters")]
		public string FirstName { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Last Name must not be greater than be 50 charecters")]
		public string LastName { get; set; }
		[Required]
		public DateOnly DateOfBirth {  get; set; }
		[Required]
		public string Gender { get; set; }
		[Required]
		public string ChronicMed { get; set; }
		[Required]
		public bool Admitted { get; set; } = false;

		public virtual ICollection<PatientMedicationScript> PatientMedicationScripts { get; set; }
		public virtual ICollection<Instruction> Instructions { get; set; }
		public virtual ICollection<Treatment> Treatments { get; set; }
		public virtual ICollection<Vital> Vitals { get; set; }
		public virtual ICollection<Discharge> Discharges { get; set; }
		public virtual Admission Admissions { get; set; }
		public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
		public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
		public virtual ICollection<Prescription> Prescriptions { get; set; }
		public virtual ICollection<Movement> Movements { get; set; }
		public virtual ICollection<VisitSchedule> VisitSchedules { get; set; }
	}
}
