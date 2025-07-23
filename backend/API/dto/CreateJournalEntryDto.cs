namespace backend.API.dto
{
    public class CreateJournalEntryDto
    {
        public string Notes { get; set; }
        public DateTime DateNTime { get; set; }
        public int PatientID { get; set;  }
        public int DoctorID { get; set; }

    }
}
