using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Citcord.Services;

public sealed class LoggingService
{
    public LoggingService(IServiceProvider provider)
    {
        var client = provider.GetRequiredService<DiscordSocketClient>();

        var command = provider.GetRequiredService<CommandService>();

        var interaction = provider.GetRequiredService<InteractionService>();
            
        client.Log += OnLogging;

        command.Log += OnLogging;

        interaction.Log += OnLogging;
    }

    private Task OnLogging(LogMessage log)
    {
        switch (log.Severity)
        {
            case LogSeverity.Info:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Message}";

                Console.WriteLine(console);
                    
                break;
            }
                
            case LogSeverity.Error when !log.Exception.Message.Contains("Missing Permissions"):
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Exception}";

                Console.WriteLine(console);
                    
                break;
            }

            case LogSeverity.Critical:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Exception}";

                Console.WriteLine(console);
                    
                break;
            }
                
            case LogSeverity.Warning when !log.Exception.Message.Contains("The default TypeReader"):
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.ToString()}";

                Console.WriteLine(console);
                    
                break;
            }

            /*
            case LogSeverity.Debug:
            {
                var console = $"{DateTime.Now:HH:mm:ss} [{log.Severity}] {log.Source}: {log.Message}";

                Console.WriteLine(console);
                
                break;
            }
            */
                
            // default: break;
        }
            
        return Task.CompletedTask;
    }
}