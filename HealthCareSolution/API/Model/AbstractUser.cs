using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string PhoneNumber { get; set; } = "";
        


        public string ChangePassword(String newPassword)
        {
            Console.WriteLine("Type your old password: ");
            string? oldPasswordInput = Console.ReadLine();

            string oldPassword = oldPasswordInput ?? string.Empty;

            if (string.IsNullOrEmpty(oldPassword))
            {
                return "Old password cannot be empty. Password change failed.";
            }
            if (oldPassword == Password)
            {
                Password = newPassword;
                return "Password changed successfully.";
            }
            else
            {
                return "Old password is incorrect. Password change failed.";
            }
        }


    }
}
