namespace API.dto
{
    using API.Model;

    public class CreateDoctorDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public char Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Specialization { get; set; } = "";
        public string Password { get; set; } = "";

        public Doctor toDoctor()
        {
            return new Doctor
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Gender = this.Gender,
                Birthdate = this.Birthdate,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                Password = this.Password,
                Specialization = this.Specialization
            };

        }
    }
}