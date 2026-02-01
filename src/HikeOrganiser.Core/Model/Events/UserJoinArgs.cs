namespace HikeOrganiser.Core.Model.Events;

public sealed class UserJoinArgs : IDiscordTargetableEvent
{
    public int EventId { get; set; }
    
    public ulong? DiscordUserId { get; set; }
    public ulong? DiscordChannelId { get; init; }
    public ulong? DiscordGuildId { get; init; }
    public ulong? EventCreatedByDiscordId { get; init; }
}