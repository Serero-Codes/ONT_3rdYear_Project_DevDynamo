using System.ComponentModel.DataAnnotations;

namespace ONT_3rdyear_Project.ViewModels
{
    public class MedicalHistoryStepViewModel
    {
        [Required]
        public int AdmissionID { get; set; }

        [Required]
        public int PatientID { get; set; }

        public List<MedicalHistoryItemViewModel> MedicalHistories { get; set; } = new List<MedicalHistoryItemViewModel>();
    }
}
