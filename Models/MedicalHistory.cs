using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONT_3rdyear_Project.Models
{
	public class MedicalHistory
	{
		[Key]
		public int MedHistoryID {  get; set; }

		[ForeignKey("Patient")]
		public int PatientId { get; set; }
		public virtual Patient Patient { get; set; }

		public string ChronicCondition { get; set; }

		public string MedicationHistory { get; set; }

		public string PastSurgicalHistory { get; set; }

		public DateTime RecorderDate { get; set; }

		public string ConditonSeverity { get; set; }
	}
}
