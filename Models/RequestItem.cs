using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace namespace ONT_3rdyear_Project.Models
{
    public class RequestItem
    {
        [Key]
        public int ItemID { get; set; }
        [ForeignKey("Request")]
        public int RequestID { get; set; }
        public Request Request { get; set; }
        [ForeignKey("Consumable")]
        public int ConsumableID { get; set; }
        public Consumable Consumable { get; set; }

        public int QuantityRequested { get; set; }

        public int? QuantityApproved { get; set; }

        public string Reason { get; set; }
    }
}