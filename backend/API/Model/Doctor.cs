using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Model
{
    using API.dto;
    using backend.API.Model;

    public class Doctor : AbstractUser, IConvertToDto<CreateDoctorDto>
    {
        public string Specialization { get; set; } = "";
        public List<Appointment> Appointments { get; set; }
        public List<JournalEntry> JournalEntries { get; set; }

        public List<Perscription> Perscriptions { get; set; }

        public CreateDoctorDto ToDto()
        {
            return new CreateDoctorDto
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