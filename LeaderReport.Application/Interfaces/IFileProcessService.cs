using LeaderReport.Application.DTOs;

namespace LeaderReport.Application.Interfaces
{
    public interface IFileProcessService
    {
        Task<FileProcessingResultDto> GuardarYProcesarArchivoAsync(byte[] fileData, string fileName);
    }
}
