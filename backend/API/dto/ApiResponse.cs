using System.Net;

namespace backend.API.dto
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            List<string> ErrorMessages = [];
        }

        public Boolean IsSuccess { get; set; }
        public Object Result { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public List<string> ErrorMessages { get; set; }

    }
}
