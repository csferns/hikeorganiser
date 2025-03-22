using HikeOrganiser.Core.Interfaces;

namespace HikeOrganiser.Core.Model.Events;

public sealed class EventCreatedArgs : EventArgs, IDiscordTargetableEvent
{
    public int EventId { get; set; }
    public string Title { get; init; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string? Location { get; init; }
    
    public ulong? DiscordGuildId { get; init; }
    public ulong? DiscordChannelId { get; init; }    
    public ulong? EventCreatedByDiscordId { get; init; }
}