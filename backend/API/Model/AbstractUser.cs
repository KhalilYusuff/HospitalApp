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

namespace API.Model
{
    public class AbstractUser 
    {

        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public char Gender { get; set; }
        public DateTime Birthdate { get; set;  }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string PassWordSalt { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        
    }
}
