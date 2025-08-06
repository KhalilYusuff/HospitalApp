using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;
using backend.API.Model;

namespace API.Model
{
    public class Patient : AbstractUser, IConvertToDto<CreatePatientDto>
    {

       public List<Appointment> Appointments { get; set; }
       public List<JournalEntry> JournalEntries { get; set; }
       public List<Perscription> Perscriptions { get; set; }

        public CreatePatientDto ToDto()
        {
            return new CreatePatientDto
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
