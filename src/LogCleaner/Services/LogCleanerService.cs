namespace LogCleaner.Services;

class LogCleanerService(
    IFilterService deleteTablesFilter,
    IFilterService outputMatchesFilter
) : ILogCleanerService
{
    public async Task CleanLogs(string sourcePath, string outputPath)
    {
        ValidateFile(sourcePath);
        
        using var writer = await GetOutputWriterAsync(outputPath);
        long lineCount = 0;
        var defaultColor = Console.ForegroundColor;

        await foreach (var line in File.ReadLinesAsync(sourcePath))
        {
            ++lineCount;
            if (deleteTablesFilter.IsMatch(line))
            {
                continue;
            }
            if (outputMatchesFilter.IsMatch(line))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Line {lineCount}: {line}");
                Console.ForegroundColor = defaultColor;
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
