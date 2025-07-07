using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    using API.dto;
    public class Doctor : AbstractUser
    {
        public string Specialization { get; set; } = "";


        public List<Appointment> Appointments { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }

        public ResponseDoctorDto toDto()
        {
            return new ResponseDoctorDto
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                Birthdate = this.Birthdate,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Specialization = this.Specialization
            };
        }
    } 
}