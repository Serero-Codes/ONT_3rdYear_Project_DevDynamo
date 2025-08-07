using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.ViewModels
{
    public class AdministerMedicationViewModel
    {
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        [Required]
        public int MedicationId { get; set; }

        public SelectList MedicationList { get; set; }

        [Required]
        public string Dosage { get; set; }

        [Required]
        public DateTime AdministeredDate { get; set; }

        [Required]
        public int ApplicationUserID { get; set; }

        public SelectList UserList { get; set; }

        public string? PrescriptionNote { get; set; } // Optional for high schedule meds
    }

}
