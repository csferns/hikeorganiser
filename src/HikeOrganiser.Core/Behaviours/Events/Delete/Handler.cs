using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.Events.Delete;

public sealed class Handler : IRequestHandler<Request, Model>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;

    public Handler(HikeOrganiserContext hikeOrganiserContext)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
    }

    public async Task<Model> Handle(Request request, CancellationToken cancellationToken)
    {
        int s = await _hikeOrganiserContext.Events
            .TagWithCallSite()
            .Where(e => e.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);
        
        return new()
        {
            Success = s > 0
        };
    }
}