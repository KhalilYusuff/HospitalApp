using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;
using backend.API.Model;

namespace API.Model
{
    public class Appointment : IConvertToDto<AppointmentDto>

    {
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set;  }

        public Patient Patient { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public Doctor Doctor {get; set;}
        [Required]
        public DateTime StartDate { get; set; } 
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Status { get; set; } = "";



        public AppointmentDto ToDto()
        {
            return new AppointmentDto
            {
                PatientID = PatientId,
                DoctorID = DoctorId,
                StarteDate = StartDate,
                EndDate = EndDate,
                Status = Status
            };
        }


    }


}
