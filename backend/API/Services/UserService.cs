using API.Data;
using API.dto;
using API.Model;
using API.PasswordHelper;
using Azure;
using backend.API.dto;
using backend.API.FieldValidator;
using backend.API.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace backend.API.Services
{
    public class UserService<T, TDto> : IUserService<T, TDto> where T: AbstractUser, IConvertToDto<TDto>
    {
        private readonly AppDbContext _context;
        private readonly PasswordLogic _passwordLogic;
        private readonly PasswordValidator _passwordValidator;

        public UserService(AppDbContext context, PasswordLogic passwordLogic, PasswordValidator passwordValidator)
        {
            _context = context;
            _passwordLogic = passwordLogic;
            _passwordValidator = passwordValidator;

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

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            var hashednSalted = HashAndsaltPassword(password);
            user.PasswordHash = hashednSalted.passwordHash;
            user.PassWordSalt = hashednSalted.passWordSalt;

            await _context.SaveChangesAsync();

            return response; 
        }

        public (string passwordHash, string passWordSalt) HashAndsaltPassword(string password)
        {
           byte[] salt;
           var hashedPass = _passwordLogic.HashPass(password, out salt);

           var passwordHash = hashedPass;
           var passWordSalt = Convert.ToBase64String(salt);

           return (passwordHash, passWordSalt);

        }
      

        //metoden først validerer nåværende passord og deretter hasher og salter ny passord og returnerer ny hash og ny salt.
        public (bool success, string message, string? newHash, string? newSalt) ChangePassword(
            string newPassword, string oldPassword, string storedHash, string storedSalt)
        {

            var storedSaltBytes = Convert.FromBase64String(storedSalt);
            var isCorrect = _passwordLogic.VerifyHashPass(oldPassword, storedHash, storedSaltBytes);

            if (!isCorrect)
            {
                return (false, "Current password is incorrect", null, null);
            }
            if (oldPassword == newPassword)
            {
                return (false, "Current password cannot be the same as the new password, please choose a new password", null, null); 

            }

            var (newHash, newSalt) = HashAndsaltPassword(newPassword);

            return (true, "Password changed successfully", newHash, newSalt);
        }


        public async Task<ApiResponse> ChangePasswordByEmail(string email, string newPassword, string oldPassword)
        {

            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var user = await _context.Set<T>().FirstOrDefaultAsync(u => u.Email == email); 

            if (user is null)
            {
                response.ErrorMessages.Add($"Could not find a user with email: {email}");
                return response;
            }

            if (!_passwordValidator.PasswordValid(newPassword))
            {
                response.ErrorMessages.Add("new password is not valid. Choose a valid password");
                return response;
            }

            var passChange = ChangePassword(newPassword, oldPassword, user.PasswordHash, user.PassWordSalt);

            if (!passChange.success)
            {
                response.ErrorMessages.Add(passChange.message);
                return response;
            }

            user.PasswordHash = passChange.newHash;
            user.PassWordSalt = passChange.newSalt;

            await _context.SaveChangesAsync();

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;

            return response;

        }

        public async Task<ApiResponse> LogInUserByEmail(LoginDto loginDto)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            var user = await _context.Set<T>().FirstOrDefaultAsync(u => u.Email == loginDto.Email); 

            if (user is null)
            {
                response.ErrorMessages.Add("User does not exist");
                return response;
            }

            bool passwordMatch = _passwordLogic.VerifyHashPass(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PassWordSalt)); 

            if (!passwordMatch)
            {
                response.ErrorMessages.Add("Wrong password, please try again");
                return response;
            }

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Result = new { Id = user.Id, Name = user.FirstName + " " + user.LastName, Email = user.Email };

            return response; 

        }


        public async Task<ApiResponse> GetAllUsers()
        {

            ApiResponse response = new();

            var users = await _context.Set<T>().ToListAsync();

            response.Result = users.Select(u => u.ToDto()).ToList(); 

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return response; 
        }


        public async Task<ApiResponse> CreateUser(ICreateUserDto<T> dto)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest, ErrorMessages = new List<string>() };

            if (!_passwordValidator.PasswordValid(dto.Password))
            {
                response.ErrorMessages.Add("Password is not valid. Please choose a valid password");
                return response;
            }


            var entity = dto.ToEntity();

            if (entity is null)
            {
                response.ErrorMessages.Add("Valid input. Please register a valid patient user");
                return response;
            }

            var (passwordHash, PasswordSalt) = HashAndsaltPassword(dto.Password);

            entity.PasswordHash = passwordHash;
            entity.PassWordSalt = PasswordSalt;

            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            return response;
        }

        public async Task<ApiResponse> GetUserByID(int id)
        {
            ApiResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound, ErrorMessages = new List<string>() };

            var user = await _context.Set<T>().FindAsync(id); 

            if (user is null)
            {
                response.ErrorMessages.Add("User not found");
                return response; 
            }

            response.Result = user.ToDto()!;
            response.StatusCode = HttpStatusCode.Found; 
            response.IsSuccess = true;

            return response; 

        }

        public Task<ApiResponse> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

       
    }
}
