using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;

namespace API.Model
{
    public class Appointment

    {
        public int Id { get; set; }
        public int PatientId { get; set;  }
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor {get; set;}

        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "";



        public AppointmentDto toDto()
        {
            return new AppointmentDto
            {
                PatientID = this.Patient.Id,
                DoctorID = this.Doctor.Id,
                Date = this.AppointmentDate,
                Status = this.Status
            };
        }


    }


}
