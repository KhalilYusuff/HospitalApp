using API.Model;

namespace backend.API.dto
{
    public class CreateJournalEntryDto
    {
        public string Notes { get; set; } = ""; 
        public DateTime DateNTime { get; set; }
        public int PatientID { get; set;  }
        public int DoctorID { get; set; }



        public JournalEntry ToJournalEntry()
        {
            return new JournalEntry
            {
                Notes = this.Notes,
                DateNTime = this.DateNTime,
                PatientID = this.PatientID,
                DoctorID = this.DoctorID
            }; 

        }

    }

}
