using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONT_3rdyear_Project.Models
{
    public class WardConsumable
    {
        [Key]
        public int StockID { get; set; }
        [ForeignKey("Ward")]
        public int WardID { get; set; }
        public virtual Ward Ward { get; set; }
        [ForeignKey("Consumable")]
        public int ConsumableID { get; set; }
        public virtual Consumable Consumable { get; set; }
        public int Quantity { get; set; }

    }
}
