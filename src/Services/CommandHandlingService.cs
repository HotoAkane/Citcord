using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using ICommandResult = Discord.Commands.IResult;
using IInteractionResult = Discord.Interactions.IResult;

namespace Citcord.Services;

public sealed class CommandHandlingService
{
    private readonly DiscordSocketClient _client;

    private readonly CommandService _command;

    private readonly InteractionService _interaction;

    private readonly IServiceProvider _provider;

    public CommandHandlingService(DiscordSocketClient client, CommandService command, InteractionService interaction, IServiceProvider provider)
    {
        _client = client;

        _command = command;

        _interaction = interaction;
            
        _provider = provider;

        _client.MessageReceived += OnMessageAsync;

        _client.SlashCommandExecuted += OnInteractionAsync;

        _client.UserCommandExecuted += OnInteractionAsync;

        _client.MessageCommandExecuted += OnInteractionAsync;

        _client.ButtonExecuted += OnButtonAsync;

        _client.SelectMenuExecuted += OnInteractionAsync;
        
        _command.CommandExecuted += AfterCommandExecutedAsync;

        _interaction.SlashCommandExecuted += AfterSlashCommandExecutedAsync;
    }

    public async Task InitializeAsync()
    {
        await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

        await _interaction.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
    }
        
    private async Task OnMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage is SocketUserMessage message)
        {
            if (message.Author.IsBot || message.Author.Id == _client.CurrentUser.Id) return;
                
            var context = new SocketCommandContext(_client, message);
            var argPos = 0;

            if (message.HasStringPrefix(CitcordConfig.Prefix, ref argPos))
            {
                if (message.Channel is IPrivateChannel or IGroupChannel)
                {
                    // var resx = await _resource.GetStringAsync(message.Author.Id, "PrivateCommandMessage");
                    var resx = "DMでコマンドは実行できません。";
                    
                    var dm = await message.Channel.SendMessageAsync(resx);
                    
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3));

                        await dm.DeleteAsync();
                    });

                    return;
                }
                
                await _command.ExecuteAsync(context, argPos, _provider);
            }
        }
    }

    private async Task AfterCommandExecutedAsync(Optional<CommandInfo> info, ICommandContext context, ICommandResult result)
    {
        if (result.IsSuccess) return;

        Embed embed = new EmbedBuilder
        {
            Title = $"Command {(result.Error.HasValue ? result.Error.Value : "Error")}",
            Description = $"{result.ErrorReason}"
        }.Build();

        await context.Message.ReplyAsync(embed: embed, allowedMentions: AllowedMentions.None);
    }
        
    private async Task OnButtonAsync(SocketMessageComponent component)
    {
        await OnInteractionAsync(component);
    }

    private async Task OnInteractionAsync(SocketInteraction interaction)
    {
        var ctx = new SocketInteractionContext(_client, interaction);
            
        await _interaction.ExecuteCommandAsync(ctx, _provider);
    }
    
    private async Task AfterSlashCommandExecutedAsync(SlashCommandInfo info, IInteractionContext context, IInteractionResult result)
    {
        if (result.IsSuccess) return;

        Embed embed = new EmbedBuilder
        {
            Title = $"Slash Command {(result.Error.HasValue ? result.Error.Value : "Error")}",
            Description = $"{result.ErrorReason}"
        }.Build();

        await context.Interaction.RespondAsync(embed: embed, ephemeral: true);
    }
}