using AutoMapper.QueryableExtensions;
using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.Events.Get;

public sealed class Handler : IRequestHandler<Request, Model>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly IMapper _mapper;

    public Handler(HikeOrganiserContext hikeOrganiserContext, IMapper mapper)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _mapper = mapper;
    }

    public async Task<Model> Handle(Request request, CancellationToken cancellationToken)
    {
        EventModel e = await _hikeOrganiserContext.Events
            .TagWithCallSite()
            .AsNoTracking()
            .ProjectTo<EventModel>(_mapper.ConfigurationProvider)
            .FirstAsync(e => e.Id == request.Id, cancellationToken);
        
        return new()
        {
            Event = e
        };
    }
}