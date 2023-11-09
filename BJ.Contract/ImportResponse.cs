using Microsoft.AspNetCore.Http;

namespace BJ.Contract
{
    public class ImportResponse
    {
        public IFormFile File { get; set; }
        public string Msg { get; set; }
    }
}
