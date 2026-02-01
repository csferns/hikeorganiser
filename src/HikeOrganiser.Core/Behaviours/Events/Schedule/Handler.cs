using System.Text.Json;
using Azure.Messaging.ServiceBus;
using HikeOrganiser.Core.Constants;
using HikeOrganiser.Core.Model.Events;
using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Enums;
using HikeOrganiser.Data.Persistence;

namespace HikeOrganiser.Core.Behaviours.Events.Schedule;

public sealed class Handler : IRequestHandler<Request, Model>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ServiceBusClient _serviceBusClient;

    public Handler(HikeOrganiserContext hikeOrganiserContext, ICurrentUserService currentUserService, ServiceBusClient serviceBusClient)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _currentUserService = currentUserService;
        _serviceBusClient = serviceBusClient;
    }

    public async Task<Model> Handle(Request request, CancellationToken cancellationToken)
    {
        User user = await _currentUserService.GetAsync(cancellationToken);

        Event scheduledEvent = new()
        {
            Title = request.Title,
            Description = request.Description,
            MeetingLocation = request.MeetingLocation,
            Location = request.Location,
            MeetingTime = request.MeetingTime, 
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            
            BucketListId = request.BucketListId,
            OrganiserId = user.Id,
            Attendees =
            {
                new AttendeeInformation() { User = user, Status = ResponseStatus.Going }
            }
        };
        
        _hikeOrganiserContext.Events.Add(scheduledEvent);
        
        await _hikeOrganiserContext.SaveChangesAsync(cancellationToken);
        
        ServiceBusSender? sender = _serviceBusClient.CreateSender(ServiceBusQueues.EventCreated);

        EventCreatedArgs obj = new()
        {
            EventId = scheduledEvent.Id,
            Location = request.Location,
            
            EventCreatedByDiscordId = user.DiscordUserId
        };
        
        await sender.SendMessageAsync(new(JsonSerializer.Serialize(obj)), cancellationToken);        
        
        return new()
        {
            EventId = scheduledEvent.Id
        };
    }
}