namespace backend.API.Model
{
    public interface IConvertToDto<TDto>
    {
        TDto ToDto(); 
    }
}
