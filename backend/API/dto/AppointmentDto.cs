
namespace API.dto
{
    using System.ComponentModel.DataAnnotations;
    using API.Model;
	
    public class AppointmentDto 
		
	{
		public int PatientID { get; set;  }
		public int DoctorID { get; set; }
		public DateTime StarteDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "";

	}
}