using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONT_3rdyear_Project.Models
{
	public class Bed
	{
		[Key]
		public int BedId { get; set; }

		[Required]
		[ForeignKey("Ward")]
		public int WardID { get; set; }

		[Required]
		public string BedNo { get; set; }

		[Required]
		public bool IsOccupied { get; set; }

		public virtual Ward Ward { get; set; }
		public virtual Admission Admissions { get; set; }
	}
}
