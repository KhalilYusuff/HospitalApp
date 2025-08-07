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

            var response = new ApiResponse();

            var user = await _context.Set<T>().FindAsync(iD) ?? throw new Exception("User not found"); ;

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            var hashednSalted = _passwordLogic.HashAndsaltPassword(password);
            user.PasswordHash = hashednSalted.passwordHash;
            user.PassWordSalt = hashednSalted.passWordSalt;

            await _context.SaveChangesAsync();

            return response; 
        }

        public async Task<ApiResponse> ChangePasswordByEmail(string email, string newPassword, string oldPassword)
        {

                var response = new ApiResponse(); 
                
                var user = await _context.Set<T>().FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim()) ?? throw new Exception($"Could not find a user with email: {email}"); ;

             

                var passChange = _passwordLogic.ChangePassword(newPassword.ToLower().Trim(), oldPassword.ToLower().Trim(), user.PasswordHash, user.PassWordSalt);

                _passwordValidator.PasswordValid(newPassword);

                if (!passChange.success)
                {
                throw new Exception("An error has occured! Could not change the password"); 
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

            var response = new ApiResponse();

            var user = await _context.Set<T>().FirstOrDefaultAsync(u => u.Email == loginDto.Email) ?? throw new Exception($"Could not find a user with email: {loginDto.Email}"); 

            bool passwordMatch = _passwordLogic.VerifyHashPass(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PassWordSalt)); 

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

            var response = new ApiResponse();

            _passwordValidator.PasswordValid(dto.Password);

            var entity = dto.ToEntity() ?? throw new Exception("Invalid input. Please register a valid patient user");

            var (passwordHash, PasswordSalt) = _passwordLogic.HashAndsaltPassword(dto.Password);

            entity.PasswordHash = passwordHash;
            entity.PassWordSalt = PasswordSalt;

            response.Result = await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();

            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;

            return response;

        }

        public async Task<ApiResponse> GetUserByID(int id)
        {
            var response = new ApiResponse();

            var user = await _context.Set<T>().FindAsync(id) ?? throw new Exception("User not found");

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
