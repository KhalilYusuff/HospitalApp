using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;

namespace API.model
{
    public class Patient : AbstractUser
    {

       public List<Appointment> Appointments { get; set; }
       public List<JournalEntry> JournalEntries { get; set; }


       public PatientDto toDto()
        {

            PatientDto dto = new PatientDto(
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                Birthdate = this.Birthdate,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber
                );
            return dto;
        }

    }

   

}
