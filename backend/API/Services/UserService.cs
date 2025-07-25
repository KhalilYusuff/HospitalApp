using API.Data;
using API.Model;
using API.PasswordHelper;
using backend.API.dto;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace backend.API.Services
{
    public class UserService<T> where T : AbstractUser
    {
        private readonly AppDbContext _context;
        private readonly PasswordLogic _passwordLogic;

        public UserService(AppDbContext context, PasswordLogic passwordLogic)
        {
            _context = context;
            _passwordLogic = passwordLogic;

        }

      public async Task<ApiResponse> SavePasswordToDB(int iD, string password)
        {

            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };

            var user = await _context.Set<T>().FindAsync(iD);

            if (user is null)
            {
                response.ErrorMessages.Add("User not found");
                return response; 
            }

            byte[] salt;
            var hashedPass = _passwordLogic.HashPass(password, out salt);

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK; 

            user.PasswordHash = hashedPass; 
            user.PassWordSalt = Convert.ToBase64String(salt);

            await _context.SaveChangesAsync();

            return response; 
        }
      

        public string ChangePassword(String newPassword)
        {
            Console.WriteLine("Type your old password: ");
            string? oldPasswordInput = Console.ReadLine();

            string oldPassword = oldPasswordInput ?? string.Empty;

            if (string.IsNullOrEmpty(oldPassword))
            {
                return "Old password cannot be empty. Password change failed.";
            }
           /* if (oldPassword == Password)
            {
                Password = newPassword;
                return "Password changed successfully.";
            }*/
            else
            {
                return "Old password is incorrect. Password change failed.";
            }
        }

    }
}
