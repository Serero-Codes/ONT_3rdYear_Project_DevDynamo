using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace namespace ONT_3rdyear_Project.Models
{
    public class Request
    {
        [Key]
        public int RequestID { get; set; }      

        [ForeignKey(nameof(RequestedByUser))]
        public string RequestedByUserId { get; set; }
        public User RequestedByUser { get; set; }

        public int WardID { get; set; }
        public Ward Ward { get; set; }

        public DateTime RequestDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; }

        public string Remarks { get; set; }

        public ICollection<RequestItem> RequestItems { get; set; }
    }
}
