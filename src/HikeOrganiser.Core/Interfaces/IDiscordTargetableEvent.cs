namespace HikeOrganiser.Core.Interfaces;

public interface IDiscordTargetableEvent
{
    ulong? DiscordGuildId { get; init; }
    ulong? DiscordChannelId { get; init; }    
    public ulong? EventCreatedByDiscordId { get; init; }
}