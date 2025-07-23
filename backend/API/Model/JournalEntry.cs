using backend.API.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    public class JournalEntry
    {

        public int Id { get; set; }
        public string Notes { get; set; } = "";
        public DateTime DateNTime { get; set; }

        public Patient Patient { get; set; }
        public int PatientID { get; set; }

        public Doctor Doctor { get; set; }
        public int DoctorID { get; set; }

        public CreateJournalEntryDto toDto()
        {

            return new CreateJournalEntryDto
            {
                Notes = this.Notes,
                DateNTime = this.DateNTime,
                PatientID = this.PatientID,
                DoctorID = this.DoctorID    
            };

        }

    } 


   

}
