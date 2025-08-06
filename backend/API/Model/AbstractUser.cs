using API.Data;
using API.PasswordHelper;
using backend.API.dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.dto;
using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class AbstractUser 
    {

        public int Id { get; set; }
        [Required, MinLength(2), MaxLength(30)]
        public string FirstName { get; set; } = "";
        [Required, MinLength(2), MaxLength(30)]
        public string LastName { get; set; } = "";
        [Required]
        public char Gender { get; set; }
        [DataType(DataType.DateTime), Required]
        public DateTime Birthdate { get; set;  }
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PassWordSalt { get; set; } = "";

        [Required]
        public string PhoneNumber { get; set; } = "";
        [Required]
        public string Address { get; set; } = "";
        [Required]
        public string City { get; set; } = "";
        [Required]
        public char Country { get; set; }
        [Required]
        public string PostalCode { get; set; } = "";


    }
}
