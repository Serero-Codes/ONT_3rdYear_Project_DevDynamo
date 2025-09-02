using ONT_3rdyear_Project.Models;

namespace ONT_3rdyear_Project.ViewModels
{
    public class AdmissionSummaryViewModel
    {
        public Admission Admission { get; set; }

        public List<PatientAllergy> PatientAllergies { get; set; } = new List<PatientAllergy>();

        public List<MedicalHistory> MedicalHistory { get; set; } = new List<MedicalHistory>();
    }
}
