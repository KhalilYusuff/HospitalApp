namespace API.dto
{
    using API.Model;

    public class ResponsePatientDto

    {
        
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public char Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
      
    }
}
