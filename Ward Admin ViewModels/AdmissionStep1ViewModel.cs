using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.ViewModels
{
    public class AdmissionStep1ViewModel
    {
        // Existing patient selection
        [Required(ErrorMessage = "Please select a patient.")]
        public int PatientID { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }

        // Admission details
        [Required]
        public int BedID { get; set; }
        public IEnumerable<SelectListItem> Beds { get; set; }

        [Required(ErrorMessage = "Please select a ward.")]
        public int WardID { get; set; }
        public IEnumerable<SelectListItem> Wards { get; set; } = new List<SelectListItem>();

        [Required]
        public int DoctorID { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; } = new List<SelectListItem>();
        [Required]
        public string ReasonForAdmission { get; set; }

        public string Notes { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly AdmissionDate { get; set; }
    }   
}

