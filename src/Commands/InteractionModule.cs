using Discord;
using Discord.Interactions;

namespace Citcord.Commands;

[Group("cit-service", "cit interaction class")]
public sealed class InteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    [Group("cafeteria", "食堂のメニューを表示します")]
    public sealed class Cafeteria : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("td", "津田沼食堂のメニューを表示します")]
        public async Task Index0Async()
        {
            var embed = new EmbedBuilder
            {
                Title = "今週のメニュー",
                ImageUrl = DinnerConverter.GetUrl("td")
            }.Build();

            await RespondAsync(embed: embed, ephemeral: false);
        }
            
        [SlashCommand("sd1", "新習志野食堂のメニューを表示します")]
        public async Task Index1Async()
        {
            var embed = new EmbedBuilder
            {
                Title = "今週のメニュー",
                ImageUrl = DinnerConverter.GetUrl("sd1")
            }.Build();

            await RespondAsync(embed: embed, ephemeral: false);
        }
            
        [SlashCommand("sd2", "新習志野食堂2Fのメニューを表示します")]
        public async Task Index2Async()
        {
            var embed = new EmbedBuilder
            {
                Title = "今週のメニュー",
                ImageUrl = DinnerConverter.GetUrl("sd2")
            }.Build();

            await RespondAsync(embed: embed, ephemeral: false);
        }
    }
}