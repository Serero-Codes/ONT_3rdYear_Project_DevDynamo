using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.Models
{
	public class ConsumableOrder
	{
		[Key]
		public int ConsumableOrderId { get; set; }

		[ForeignKey("Consumable")]
		public int ConsumableId { get; set; }
		public virtual Consumable Consumable { get; set; }

		[ForeignKey("Order")]
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }
	}
}
