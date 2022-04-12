using Discord;
using Newtonsoft.Json;

namespace Citcord;

internal static class CitcordConfig
{
    private static ConfigJson _configJson;

    internal static async Task InitAsync()
    {
        _configJson = JsonConvert.DeserializeObject<ConfigJson>(await File.ReadAllTextAsync("./config/env.json"));
    }
    
    internal static string Token => _configJson.Token;
    
    internal static string Prefix => _configJson.Prefix;

    internal static ulong OwnerId => _configJson.OwnerId;

    internal static Color MainColor => new Color(2336576);

    private struct ConfigJson
    {
        [JsonProperty("botToken")]
        internal string Token { get; set; }
        
        [JsonProperty("botPrefix")]
        internal string Prefix { get; set; }
        
        [JsonProperty("ownerId")]
        internal ulong OwnerId { get; set; }
    }
}