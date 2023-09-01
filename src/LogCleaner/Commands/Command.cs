namespace LogCleaner.Commands;

class Command : ICommand
{
    private readonly List<string> _arguments;

    public Command(IEnumerable<string> arguments)
    {
        _arguments = arguments.ToList();
    }

    public bool HasArgument(string name)
    {
        return _arguments.Contains(name);
    }

    public bool HasArgument(string name, string shortName)
    {
        return _arguments.Contains(name) || _arguments.Contains(shortName);
    }

    public string? GetArgument(string name)
    {
        TryGetArgument(name, out var value);
        return value;
    }

    public string? GetArgument(string name, string shortName)
    {
        if (TryGetArgument(name, out var value))
        {
            return value;    
        }

        if (TryGetArgument(shortName, out value))
        {
            return value;
        }
        return default;
    }

    private bool TryGetArgument(string name, out string? value)
    {
        value = default;
        var index = _arguments.IndexOf(name);

        if (index == -1)
        {
            return false;
        }

        value = _arguments[index + 1];
        return true;
    }
}
