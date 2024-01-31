namespace LogCleaner.Configurations;

using Newtonsoft.Json;

class Configuration : IConfiguration
{
    public IEnumerable<string> DeletingTables { get; set; } = new List<string>
    {
        "mdl_analytics_indicator_calc",
        "mdl_analytics_models_log",
        "mdl_backup_logs",
        "mdl_config_log",
        "mdl_grade_grades_history",
        "mdl_logstore_standard_log",
        "mdl_portfolio_log",
        "mdl_sessions",
        "mdl_task_log",
        "mdl_upgrade_log",
    };

    public IEnumerable<string> OutputMatches { get; set; } = new List<string> {};

    public void LoadFromJson()
    { 
        LoadFromJson(GetConfigPath());
    }

    public void LoadFromJson(string configPath)
    {
        var config = ReadConfigFromJson(configPath);
        DeletingTables = config.DeletingTables;
        OutputMatches = config.OutputMatches;
    }

    private JsonConfiguration ReadConfigFromJson(string configPath)
    {
        var json = File.ReadAllText(configPath);
        Console.WriteLine($"Try to read config file from: {configPath}");
        return JsonConvert.DeserializeObject<JsonConfiguration>(json) ?? throw new NullReferenceException();
    }

    private string GetConfigPath()
    {
        var executablePath = Path.GetDirectoryName(AppContext.BaseDirectory ?? "");
        var configPath = Path.GetFullPath($"{executablePath}{Path.DirectorySeparatorChar}config.json");
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Config file not found in {configPath}", configPath);
        }
        return configPath;
    }
}
