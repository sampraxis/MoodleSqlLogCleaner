namespace LogCleaner.Commands;

interface ICommand
{
    bool HasArgument(string name);
    bool HasArgument(string name, string shortName);
    string? GetArgument(string name);
    string? GetArgument(string name, string shortName);
}
