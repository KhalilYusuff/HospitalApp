using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;

namespace API.Model
{
    public class Patient : AbstractUser
    {

       public List<Appointment> Appointments { get; set; }
       public List<JournalEntry> JournalEntries { get; set; }


       public PatientDto toDto()
        {

           return new PatientDto
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                Birthdate = this.Birthdate,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                
            };
        }

        

    }

   

}
