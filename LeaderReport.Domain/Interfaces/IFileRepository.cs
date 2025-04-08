namespace LeaderReport.Domain.Interfaces;

public interface IFileRepository
{
    Task<string> GuardarArchivoAsync(byte[] fileData, string fileName);
}
