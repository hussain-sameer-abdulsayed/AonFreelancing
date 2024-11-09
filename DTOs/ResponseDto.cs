using AonFreelancing.JwtClasses;

namespace AonFreelancing.DTOs
{
    public class ResponseDto
    {
        public class ApiResponse<T>
        {
            public bool IsSuccess { get; set; }
            public T Results { get; set; }
            public IList<Error> Errors { get; set; }
            public string? AccessToken { get; set; }

        }

        public class Error
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }
    }
}
