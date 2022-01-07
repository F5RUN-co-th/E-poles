using Microsoft.AspNetCore.Identity;

namespace E_poles.Dal
{
    public class BaseResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Resource { get; set; }
        public bool Succeeded { get; protected set; }
    }

}
