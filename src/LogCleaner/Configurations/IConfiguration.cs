namespace LogCleaner.Configurations;

interface IConfiguration
{
    IEnumerable<string> DeletingTables { get; set; }

    IEnumerable<string> OutputMatches { get; set; }

    void LoadFromJson();
    void LoadFromJson(string configPath);
}
