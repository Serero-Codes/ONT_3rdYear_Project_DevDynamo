using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONT_3rdyear_Project.Models
{
	public class Order
	{
		[Key]
		public int OrderID { get; set; }
		[Required]
		[ForeignKey("Supplier")]
		public int SupplierID { get; set; }
		[Required]
		public DateTime OrderDate { get; set; }
		[Required]
		public int OrderQuantity { get; set; }

		public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ConsumableOrder> ConsumableOrders { get; set; }
    }
}
