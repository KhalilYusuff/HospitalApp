using API.dto;
using API.Model;
using backend.API.dto;
using backend.API.Model;

namespace backend.API.Services
{
    public interface IUserService<T, TDto> where T: AbstractUser, IConvertToDto<TDto>
    {
        Task<ApiResponse> ChangePasswordByEmail(string email, string newPassword, string oldPassword);

        Task<ApiResponse> CreateUser(ICreateUserDto<T> dto);

        Task<ApiResponse> DeleteUser(int id);

        Task<ApiResponse> GetAllUsers();

        Task<ApiResponse> GetUserByID(int id); 




    }
}
