using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ONT_3rdyear_Project.Models
{
	public class Patient
	{
		[Key]
		public int PatientID { get; set; }
		[ForeignKey("ApplicationUser")]
		public int DoctorID { get; set; } // Foreign key to ApplicationUser (Doctor)
        [Required]
		[StringLength(50, ErrorMessage = "Name of the patient is required")]
		public string FirstName { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Last Name of the patient is required")]
		public string LastName { get; set; }
		[Required]
		public DateOnly DateOfBirth {  get; set; }
		[Required]
		public string Gender { get; set; }
		[Required]
		public string ChronicIllness { get; set; }
		[Required]
		public bool Admitted { get; set; } = false;

        //[NotMapped]
        //public bool IsAdmitted => Admissions != null && Admissions.DischargeDate == null;

        public bool IsDeleted { get; set; } = false;

		public virtual ICollection<PatientMedicationScript> PatientMedicationScripts { get; set; }
		public virtual ICollection<Instruction> Instructions { get; set; }
		public virtual ICollection<Treatment> Treatments { get; set; }
		public virtual ICollection<Vital> Vitals { get; set; }
		public virtual ICollection<Discharge> Discharges { get; set; }
        //public virtual ICollection<Admission> Admissions { get; set; }
        public virtual Admission Admissions { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
		public virtual ICollection<MedicalHistory> MedicalHistories { get; set; }
		public virtual ICollection<Prescription> Prescriptions { get; set; }
		public virtual ICollection<Movement> Movements { get; set; }
		public virtual ICollection<VisitSchedule> VisitSchedules { get; set; }
        public ICollection<DoctorAssignment> DoctorAssignments { get; set; }
		public ApplicationUser ApplicationUsers { get; set; } // Navigation property to ApplicationUser (Doctor)
	
    }
}
