using Newtonsoft.Json;

namespace LogCleaner.Configurations;

class JsonConfiguration
{
    [JsonProperty("deletingTables")]
    public IEnumerable<string> DeletingTables { get; set; } = null!;

    [JsonProperty("outputMatches")]
    public IEnumerable<string> OutputMatches { get; set; } = null!;
}
