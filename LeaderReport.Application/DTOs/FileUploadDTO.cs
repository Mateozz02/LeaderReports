using Microsoft.AspNetCore.Http;

namespace LeaderReport.Application.DTOs
{
    public class FileUploadDTO
    {
        public IFormFile File { get; set; }
    }

}
