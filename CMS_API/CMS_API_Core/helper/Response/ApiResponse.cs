using Microsoft.AspNetCore.Http;

namespace CMS_API_Core.helper.Response
{
    public class ApiResponse<T>(IEnumerable<T>? data, int status, IEnumerable<string> message)
    {
        public int Status { get; set; } = status;
        public IEnumerable<string> Message { get; set; } = message;
        public IEnumerable<T>? Data { get; set; } = data;

    }

}
