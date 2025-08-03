using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ONT_3rdyear_Project.Models
{
	public class Vital
	{
		[Key]
		public int VitalID { get; set; }
		
		[ForeignKey("VisitSchedule")]
		public int? VisitID {  get; set; }


		[Required]
		[ForeignKey("User")]
		public int ApplicationUserID { get; set; }


		[Required]
		[ForeignKey("Patient")]
		public int PatientID { get; set; }


		[Required]
		public string BP {  get; set; }


		[Required]
        public double Temperature { get; set; }


		[Required]
		public string SugarLevel { get; set; }


        [Required]
        public string PulseRate { get; set; }

        [Required]
		public DateTime Date { get; set; }

		public bool IsActive { get; set; } = true;


        public virtual VisitSchedule VisitSchedule { get; set; }
		public virtual Patient Patient { get; set; }
		public virtual ApplicationUser User { get; set; }
		
	}
}
