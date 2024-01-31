namespace LogCleaner.Services;

interface ILogCleanerService
{
    Task CleanLogs(string sourcePath, string outputPath);
}