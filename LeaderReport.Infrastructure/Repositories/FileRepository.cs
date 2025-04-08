using LeaderReport.Domain.Interfaces;

namespace LeaderReport.Infrastructure.Repositories;

public class FileRepository : IFileRepository

{
    private readonly string _storagePath = "D:/LaudinoTech/ArchivosSubidos/";

    public async Task<string> GuardarArchivoAsync(byte[] fileData, string fileName)
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }

        string fullPath = Path.Combine(_storagePath, fileName);
        await File.WriteAllBytesAsync(fullPath, fileData);
        return fullPath;
    }
}
