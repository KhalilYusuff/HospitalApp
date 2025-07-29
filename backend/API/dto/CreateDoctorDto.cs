namespace API.dto
{
    using API.Model;
    using backend.API.dto;

    public class CreateDoctorDto : ICreateUserDto<Doctor>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public char Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Password { get; set; } = "";
        public string Specialization { get; set; } = "";
        
        public Doctor ToEntity()
        {
            return new Doctor
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


        public CreateDoctorDto ToResponseDto()
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