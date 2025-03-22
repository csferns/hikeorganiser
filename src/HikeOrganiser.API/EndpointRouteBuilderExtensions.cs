using MediatR;
using Microsoft.AspNetCore.Mvc;
using Schedule = HikeOrganiser.Core.Behaviours.Events.Schedule;

namespace HikeOrganiser.API;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPut("/schedule", 
            ([FromBody] Schedule.Request request, [FromServices] IMediator mediator, CancellationToken token = default) => 
                mediator.Send(request, token));
        
        return builder;
    }
}