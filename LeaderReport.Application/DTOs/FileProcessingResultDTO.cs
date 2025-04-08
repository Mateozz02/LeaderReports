using System;

namespace LeaderReport.Application.DTOs;

public class FileProcessingResultDto
{
    public string FileName { get; set; } = string.Empty;
    public int ProcessedCount { get; set; }
    public int ErrorsCount { get; set; }
}

