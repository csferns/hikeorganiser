using AutoMapper.QueryableExtensions;
using HikeOrganiser.Core.Extensions;
using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Enums;
using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.Events.GetAll;

public sealed class Handler : IRequestHandler<Request, PagedModel<EventModel>>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly TimeProvider _timeProvider;
    private readonly ICurrentUserService _currentUserService;

    public Handler(HikeOrganiserContext hikeOrganiserContext, TimeProvider timeProvider, ICurrentUserService currentUserService)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _timeProvider = timeProvider;
        _currentUserService = currentUserService;
    }

    public async Task<PagedModel<EventModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        DateTimeOffset now = _timeProvider.GetUtcNow();

        User user = await _currentUserService.GetAsync(cancellationToken);

        IQueryable<EventModel> query = 
            (
                from e in _hikeOrganiserContext.Events
                
                join att in _hikeOrganiserContext.AttendeeInformation on e.Id equals att.EventId into attendeeInfo
                
                let currentAttendee = e.Attendees.FirstOrDefault(x => x.UserId == user.Id)
                
                where e.StartDate.Date >= (request.Filter.DateFrom ?? now).Date
                      && (!e.EndDate.HasValue || (request.Filter.DateTo ?? now).Date <= e.EndDate.Value.Date)
                      && (e.OrganiserId == user.Id || currentAttendee != null)
                orderby e.StartDate, e.Title
                select new EventModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    MeetingLocation = e.MeetingLocation,
                    Location = e.Location,
                    MeetingTime = e.MeetingTime,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    
                    CurrentUser = currentAttendee == null 
                        ? new() { Name = user.DisplayName, Status = ResponseStatus.None }
                        : new() { Name = currentAttendee.User!.DisplayName, Status = currentAttendee.Status },
                    Attendees = attendeeInfo
                        .Where(x => x.UserId != user.Id)
                        .Select(x => new AttendeeInformationModel() { Name = x.User!.DisplayName, Status = x.Status })
                        .ToList()
                }
            )
            .TagWithCallSite()
            .AsNoTracking();
 
        (List<EventModel> results, int count) = await query.PageAsync(request.Filter, cancellationToken);
        
        return new()
        {
            FullCount = count,
            Items = results
        };
    }
}