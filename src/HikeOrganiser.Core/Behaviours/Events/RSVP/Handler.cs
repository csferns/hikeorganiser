using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.Events.RSVP;

public sealed class Handler : IRequestHandler<Request>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly ICurrentUserService _currentUserService;

    public Handler(HikeOrganiserContext hikeOrganiserContext, ICurrentUserService currentUserService)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _currentUserService = currentUserService;
    }
    
    public async Task Handle(Request request, CancellationToken cancellationToken)
    {
        User user = await _currentUserService.GetAsync(cancellationToken);
        
        AttendeeInformation info = await _hikeOrganiserContext.AttendeeInformation
                                       .TagWithCallSite()
                                       .AsTracking()
                                       .FirstOrDefaultAsync(x => x.EventId == request.EventId && x.UserId == user.Id, cancellationToken) 
                                   ?? new AttendeeInformation() { EventId = request.EventId, UserId = user.Id };

        info.Status = request.Status;
        
        await _hikeOrganiserContext.SaveChangesAsync(cancellationToken);
    }
}