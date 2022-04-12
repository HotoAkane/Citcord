using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace Citcord.Commands;

public sealed class UpdateSystemModule : ModuleBase<SocketCommandContext>
{
    private readonly InteractionService _interaction;

    public UpdateSystemModule(InteractionService interaction)
    {
        _interaction = interaction;
    }
    
    [Command("update-commands")]
    public async Task UpdateInteractionModule()
    {
        if (Context.User.Id != CitcordConfig.OwnerId) return;

        await _interaction.RegisterCommandsGloballyAsync();

        // await _interaction.RegisterCommandsToGuildAsync(Context.Guild.Id);

        await Context.Message.ReplyAsync("update commands");
    }
}