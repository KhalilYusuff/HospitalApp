
namespace API.dto
{
    using API.Model;
    using backend.API.dto;
    using System.ComponentModel.DataAnnotations;

    public class CreatePatientDto : ICreateUserDto<Patient>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public char Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Password { get; set; } = "";

        public Patient ToEntity()
        {
            return new Patient
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