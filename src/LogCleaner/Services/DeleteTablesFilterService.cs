using LogCleaner.Configurations;

namespace LogCleaner.Services;

class DeleteTablesFilterService(IConfiguration configuration) : IFilterService
{
    public bool IsMatch(string line)
    {
        return configuration.DeletingTables.Any(table => line.StartsWith($"INSERT INTO `{table}`"));
    }
}