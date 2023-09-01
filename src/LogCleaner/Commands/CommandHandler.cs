using LogCleaner.Configurations;
using LogCleaner.Services;

namespace LogCleaner.Commands;

class CommandHandler
{
    public ICommand Command { get; }
    public IConfiguration Configuration { get; }
    public ILogCleanerService Service { get; }
    public string ExecutableFilename => Path.GetFileName(Environment.ProcessPath) ?? "LogCleaner";

    public CommandHandler(
        ICommand command,
        IConfiguration config,
        ILogCleanerService service
        )
    {
        Command = command;
        Configuration = config;
        Service = service;
    }

    public async Task Execute()
    {
        if (IsHelp())
        {
            return;
        }
        
        if (IsDryRun())
        {
            return;
        }

        var configPath = GetConfigPath();
        if (configPath != default)
        {
            Configuration.LoadFromJson(configPath);
        }

        var fullpath = GetFilePath();
        var newFilename = GetNewFilePath(fullpath);

        Console.WriteLine("Removing log entries");

        await Service.CleanLogs(Configuration.DeletingTables, fullpath, newFilename);

        Console.WriteLine($"Removed log entries, see more at {newFilename}");
        Console.WriteLine("Done");

    }

    private string? GetConfigPath()
    {
        return Command.GetArgument("--config", "-c");
    }

    private string GetFilePath()
    {
        var filepath = Command.GetArgument("--file", "-f");
        filepath ??= Command.GetArgument("--source", "-s");
        if (filepath == default)
        {
            throw new ArgumentException("Please provide a file path");
        }

        var fullPath = Path.GetFullPath(filepath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found", filepath);
        }

        if (!fullPath.EndsWith(".sql"))
        {
            throw new ArgumentException("File must be a .sql file");
        }
        return filepath;
    }

    private string GetNewFilePath(string sourcePath)
    {
        var output = Command.GetArgument("--output", "-o");
        output ??= Command.GetArgument("--dest");
        if (output == default)
        {
            return GetDefaultNewFilePath(sourcePath);
        }
        return Path.GetFullPath(output);
    }
    
    private string GetDefaultNewFilePath(string sourcePath)
    {
        var dir = Path.GetDirectoryName(sourcePath) ?? throw new NullReferenceException("Invalid source path");
        var filename = Path.GetFileName(sourcePath);
        var newName = filename[..^4];
        return Path.Combine(dir, $"{newName}.nolog.sql");
    }

    private bool IsDryRun()
    {
        if (!Command.HasArgument("--dry-run", "-d"))
        {
            return false;
        }

        var fullpath = GetDefaultSourcePath();
        var configPath = GetConfigPath();
        var newFilename = fullpath == default ? default : GetNewFilePath(fullpath);

        if (!string.IsNullOrEmpty(configPath))
        {
            Configuration.LoadFromJson(configPath);
        }
        
        Console.WriteLine("Rapport of what would be deleted");
        Console.WriteLine();
        
        foreach (var table in Configuration.DeletingTables)
        {
            Console.WriteLine($"Would delete from {table}");
        }

        Console.WriteLine();

        DisplayFilePath(configPath, "Config file: {0}", "No config file provided");
        DisplayFilePath(fullpath, "Source file: {0}", "No source file provided");
        DisplayFilePath(newFilename, "New file: {0}", "No new file provided");

        return true;
    }

    private string? GetDefaultSourcePath()
    {
        try
        {
            return GetFilePath();
        }
        catch (Exception)
        {}
        return default;
    }

    private void DisplayFilePath(string? filepath, string successMessage, string errorMessage)
    {
        if (string.IsNullOrEmpty(filepath))
        {
            Console.WriteLine(errorMessage);
            return;
        }
        Console.WriteLine(successMessage, filepath);
    }

    private bool IsHelp()
    {
        if (!Command.HasArgument("--help", "-h"))
        {
            return false;
        }
        Console.WriteLine("Usage:");
        Console.WriteLine($"{ExecutableFilename} --file <file>, -f <file>");
        Console.WriteLine($"{ExecutableFilename} --source <file>, -s <file>");
        Console.WriteLine($"{ExecutableFilename} --dry-run, -d");
        Console.WriteLine($"{ExecutableFilename} --output <file>, --dest <file>, -o <file>");
        Console.WriteLine($"{ExecutableFilename} --config <json>, -o <json>");
        Console.WriteLine($"{ExecutableFilename} --help, -h");

        return true;
    }
}
