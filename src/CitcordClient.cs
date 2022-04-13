using System.Reflection;
using Citcord.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using DLsiteSearch;
using Microsoft.Extensions.DependencyInjection;

namespace Citcord;

public sealed class CitcordClient
{
    private readonly DiscordSocketClient _client;

    private const GatewayIntents Intents = GatewayIntents.Guilds |
                                           GatewayIntents.DirectMessages |
                                           GatewayIntents.GuildMembers |
                                           GatewayIntents.GuildMessages |
                                           GatewayIntents.GuildMessageReactions;

    public CitcordClient()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = Intents,
            LogLevel = LogSeverity.Debug,
            MessageCacheSize = 1000
        });
    }

    public async Task LoginAsync()
    {
        await CitcordConfig.InitAsync();
            
        var provider = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton<CommandService>()
            .AddSingleton<InteractionService>()
            .AddSingleton<CommandHandlingService>()
            .AddSingleton<LoggingService>()
            .AddSingleton<HttpClient>()
            .AddSingleton<DLsiteParser>()
            .AddSingleton<DlsiteViewer>()
            .BuildServiceProvider();
            
        provider.GetRequiredService<LoggingService>();
        
        provider.GetRequiredService<DlsiteViewer>();
            
        await provider.GetRequiredService<CommandHandlingService>().InitializeAsync();

        await provider.GetRequiredService<DLsiteParser>().UpdateGenreAsync();
            
        await _client.LoginAsync(TokenType.Bot, CitcordConfig.Token);
        await _client.StartAsync();

        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            
        string versionCode = version is null ? "undefined version" : $"version {version.Major}.{version.Minor}.{version.Build}";

        await _client.SetActivityAsync(new Game($"Citcord {versionCode}"));
    }
}