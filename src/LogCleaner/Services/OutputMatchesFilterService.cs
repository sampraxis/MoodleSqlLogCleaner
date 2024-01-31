namespace LogCleaner.Services;


using LogCleaner.Configurations;


class OutputMatchesFilterService(IConfiguration configuration) : IFilterService
{
    public bool IsMatch(string line)
    {
        return configuration.OutputMatches.Any(line.Contains);
    }
}