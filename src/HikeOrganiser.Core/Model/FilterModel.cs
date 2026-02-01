namespace HikeOrganiser.Core.Model;

public sealed record FilterModel
{
    public int? PageSize { get; init; }
    public int? Page { get; init; }
    
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}