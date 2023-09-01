namespace LogCleaner.Services;

interface ILogCleanerService
{
    Task CleanLogs(IEnumerable<string> tables, string sourcePath, string outputPath);
}