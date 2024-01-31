namespace LogCleaner.Services;

interface IFilterService
{
    bool IsMatch(string line);
}