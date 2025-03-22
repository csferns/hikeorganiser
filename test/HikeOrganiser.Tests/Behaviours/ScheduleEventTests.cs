using Azure.Messaging.ServiceBus;
using HikeOrganiser.Core.Constants;
using HikeOrganiser.Core.Interfaces;
using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Enums;
using HikeOrganiser.Data.Persistence;
using MockQueryable;
using MockQueryable.Moq;
using Schedule = HikeOrganiser.Core.Behaviours.Events.Schedule;

namespace HikeOrganiser.Tests.Behaviours;

public class ScheduleEventTests
{
    private Mock<Context> _context = null!;
    private Mock<ICurrentUserService> _currentUserService = null!;
    private Mock<ServiceBusClient> _serviceBusClient = null!;
    private Mock<ServiceBusSender> _serviceBusSender = null!;
    
    private User _currentUser = null!;
    
    private Schedule.Handler _handler = null!;

    [SetUp]
    public void Setup()
    {
        User user = new() { Id = new("3de9f712-4f59-412c-8560-0bc2add62eb4") };
        
        Mock<Context> context = new();
        Mock<ICurrentUserService> currentUserService = new();
        currentUserService.Setup(mock => mock.GetAsync(CancellationToken.None))
            .ReturnsAsync(user);
        
        Mock<ServiceBusSender> sender = new();
        
        Mock<ServiceBusClient> serviceBusClient = new();
        serviceBusClient.Setup(mock => mock.CreateSender(ServiceBusQueues.EventCreated))
            .Returns(sender.Object);
        
        _handler = new(context.Object, currentUserService.Object, serviceBusClient.Object);
        
        _context = context;
        _currentUserService = currentUserService;
        _serviceBusClient = serviceBusClient;
        _serviceBusSender = sender;
        
        _currentUser = user;
    }

    [Test]
    public async Task Schedule_ShouldCreate()
    {
        List<Event> events = [];

        _context.IncludeDbSetAsync(static x => x.Events, events);
        
        Schedule.Request request = new()
        {
            Title = "Test Event",
            Description = "Test Event",
            Location = "Test Location",
            MeetingLocation = "Test Meeting Location",
            DateType = DateType.Manual,
            MeetingTime = TimeOnly.Parse("12:00:00"),
            StartDate = DateTime.Today.AddDays(2),
            EndDate = DateTime.Today.AddDays(3),
        };
        
        Schedule.Model model = await _handler.Handle(request, CancellationToken.None);
        
        model.Success.Should().BeTrue();
        
        events.Should().HaveCount(1);
        
        Event expectedEvent = new()
        {
            Title = "Test Event",
            Description = "Test Event",
            Location = "Test Location",
            MeetingLocation = "Test Meeting Location",
            DateType = DateType.Manual,
            MeetingTime = TimeOnly.Parse("12:00:00"),
            StartDate = DateTime.Today.AddDays(2),
            EndDate = DateTime.Today.AddDays(3),
            
            BucketListId = null,
            OrganiserId = new("3de9f712-4f59-412c-8560-0bc2add62eb4"),
            Attendees =
            {
                new() { User = _currentUser, Status = ResponseStatus.Going }
            }
        };
        
        events[0].Should().BeEquivalentTo(expectedEvent);
        
        _context.Verify(mock => mock.SaveChangesAsync(CancellationToken.None), Times.Once);
        
        _serviceBusSender.Verify(mock => mock.SendMessageAsync(It.IsAny<ServiceBusMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}