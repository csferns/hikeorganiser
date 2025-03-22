using Discord;
using Discord.WebSocket;
using HikeOrganiser.Core.Options;
using Microsoft.Extensions.Options;

namespace HikeOrganiser.DiscordBot.Services;

public sealed class DiscordInteractionService
{
    private readonly DiscordSocketClient _client;

    public DiscordInteractionService(IOptions<AppKeys> keys)
    {
        DiscordSocketClient client = new(new()
        {
            AlwaysDownloadUsers = true,
            MessageCacheSize = 100,
            GatewayIntents = GatewayIntents.AllUnprivileged,
            LogLevel = LogSeverity.Info
        });
            
        //await client.LoginAsync(TokenType.Bot, keys.Value.BotToken);

        _client = client;
    }
}