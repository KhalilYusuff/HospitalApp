using API.Model;

namespace backend.API.dto
{
    public interface ICreateUserDto<T> where T : AbstractUser
    {
        string Password { get; }
        int Id { get; }

        T ToEntity();
    }
}
