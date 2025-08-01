using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.Models
{
	public class Medication
	{
		[Key]
		public int MedicationId { get; set; }

		[Required(ErrorMessage = "Medication Name is required")]
		public string Name { get; set; }
		
        [Required]
        [Range(1, 6, ErrorMessage = "Schedule must be between 1 and 6")]
        public int Schedule { get; set; }
        public DateOnly ExpiryDate { get; set; }

		public virtual ICollection<PharmacyMedication> PharmacyMedications { get; set; }
		public virtual ICollection<PrescribeMedication> PrescribeMadications { get; set; }
		public virtual ICollection<PatientMedicationScript> PatientMedicationScripts { get; set; }

	}
}
