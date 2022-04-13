using System.Text.RegularExpressions;
using Discord;
using Discord.WebSocket;
using DLsiteSearch;
using DLsiteSearch.Entities;

namespace Citcord.Services;

public sealed class DlsiteViewer
{
    private const string ProductRegex = "https://www.dlsite.com/.+/work/=/product_id/[A-Z]{2}[0-9]{6}.html";
    
    private DLsiteParser _parser;
    
    public DlsiteViewer(DiscordSocketClient client, DLsiteParser parser)
    {
        _parser = parser;
        
        client.MessageReceived += OnMessageAsync;
    }
    
    private async Task OnMessageAsync(SocketMessage socketMessage)
    {
        if (socketMessage is SocketUserMessage message)
        {
            if (socketMessage.Content.Contains("https://www.dlsite.com/"))
            {
                Match match = Regex.Match(socketMessage.Content, ProductRegex);

                if (match.Success)
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Run(async () =>
                        {
                            DLsiteProduct product = await _parser.GetProductFromUriAsync(match.Value);

                            Embed embed = SafelyNsfwProvider.CreateById(product, message);

                            await message.ReplyAsync(embed: embed, allowedMentions: AllowedMentions.None);
                        });
                    });
                }
            }
        }
    }
}