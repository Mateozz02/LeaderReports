using LeaderReport.Application.DTOs;
using LeaderReport.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LeaderReport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileProcessService _fileProcessService;

        public FileController(IFileProcessService fileProcessService)
        {
            _fileProcessService = fileProcessService;
        }

[HttpPost("upload")]
[Consumes("multipart/form-data")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO fileUploadDto)
{
    // Validar que la propiedad File no sea nula y tenga contenido
    if (fileUploadDto?.File == null || fileUploadDto.File.Length == 0)
        return BadRequest("Debe proporcionar un archivo válido.");

    using var memoryStream = new MemoryStream();
    await fileUploadDto.File.CopyToAsync(memoryStream);
    byte[] fileData = memoryStream.ToArray();

    // Utilizar la propiedad File para obtener el nombre del archivo
    FileProcessingResultDto result = await _fileProcessService.GuardarYProcesarArchivoAsync(fileData, fileUploadDto.File.FileName);

    return Ok(result);
}

    }
}
