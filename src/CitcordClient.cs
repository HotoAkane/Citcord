using System.Reflection;
using Citcord.Services;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
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
            .BuildServiceProvider();
            
        provider.GetRequiredService<LoggingService>();
            
        await provider.GetRequiredService<CommandHandlingService>().InitializeAsync();
            
        await _client.LoginAsync(TokenType.Bot, CitcordConfig.Token);
        await _client.StartAsync();

        Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            
        string versionCode = version is null ? "undefined version" : $"version {version.Major}.{version.Minor}.{version.Build}";

        await _client.SetActivityAsync(new Game($"Citcord {versionCode}"));
    }
}