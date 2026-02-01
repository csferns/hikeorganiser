namespace HikeOrganiser.Core.Model;

public sealed record PagedModel<TResult>
{
    public required int FullCount { get; init; }
    public required List<TResult> Items { get; init; }
}