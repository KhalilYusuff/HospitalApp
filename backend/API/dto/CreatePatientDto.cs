
namespace API.dto
{
    using API.Model;
    using backend.API.dto;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;

    public class CreatePatientDto : ICreateUserDto<Patient>
    {
        [Required, MinLength(2), MaxLength(30)]
        public string FirstName { get; set; } = "";
        [Required, MinLength(2), MaxLength(30)]
        public string LastName { get; set; } = "";
        [Required]
        public char Gender { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        [Required, Phone]
        public string PhoneNumber { get; set; } = "";
        [Required]
        public string Password { get; set; } = "";
        [Required]
        public string Address { get; set; } = "";
        [Required]
        public string City { get; set; } = "";
        [Required]
        public char Country { get; set; }
        [Required]
        public string PostalCode { get; set; } = "";

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