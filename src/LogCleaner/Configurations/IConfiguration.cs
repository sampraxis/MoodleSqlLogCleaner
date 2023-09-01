namespace LogCleaner.Configurations;

interface IConfiguration
{
    IEnumerable<string> DeletingTables { get; set; }

    void LoadFromJson();
    void LoadFromJson(string configPath);
}
