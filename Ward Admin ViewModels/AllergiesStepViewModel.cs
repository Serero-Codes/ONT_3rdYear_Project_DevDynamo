using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.ViewModels
{
    public class AllergiesStepViewModel
    {
        [Required]
        public int AdmissionID { get; set; }

        [Required]
        public int PatientID { get; set; }
        [Required(ErrorMessage = "Allergy Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Allergy Severity is required")]
        public string Severity { get; set; }

        // List of allergy names
        public List<string> PatientAllergy { get; set; } = new List<string>();
    }
}
