namespace LogCleaner.Services;

class LogCleanerService : ILogCleanerService
{

    public async Task CleanLogs(IEnumerable<string> tables, string sourcePath, string outputPath)
    {
        ValidateFile(sourcePath);
        
        var writer = await GetOutputWriterAsync(outputPath);

        await foreach (var line in File.ReadLinesAsync(sourcePath))
        {
            if (tables.Any(table => line.StartsWith($"INSERT INTO `{table}`")))
            {
                continue;
            }
            await writer.WriteLineAsync(line);
        }
    }

    private async Task<StreamWriter> GetOutputWriterAsync(string outputPath)
    {
        await File.WriteAllTextAsync(outputPath, "");
        return new StreamWriter(outputPath, true);
    }

    private void ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }
    }
}
