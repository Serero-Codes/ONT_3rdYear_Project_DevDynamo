using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONT_3rdyear_Project.Models
{
	public class Ward
	{
		[Key]
		public int WardID { get; set; }
		
		[Required]
		public string Name { get; set; }
		[Required]
		public int Capacity { get; set; }

<<<<<<< HEAD
        public bool IsActive { get; set; } = true;		
=======
        public bool IsActive { get; set; } = true;

		
>>>>>>> fd2377ef523ad4c8b0fb4ff7d58611fb54cd65c9
        public virtual ICollection<Bed> Beds { get; set; }		
		public virtual ICollection<Admission> Admissions { get; set; }
		public virtual ICollection<WardConsumable> WardConsumables { get; set; }
		public virtual ICollection<Movement> Movements { get; set; }
    }
}
