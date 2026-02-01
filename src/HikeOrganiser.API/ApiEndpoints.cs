using HikeOrganiser.Core.Model;
using HikeOrganiser.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using EV = HikeOrganiser.Core.Behaviours.Events;
using BL = HikeOrganiser.Core.Behaviours.BucketList;

namespace HikeOrganiser.API;

public sealed class ApiEndpoints
{
    public static class Events
    {
        public static async Task<IResult> GetAll(
            [AsParameters] FilterModel filter, 
            [FromServices] IMediator mediator, 
            CancellationToken token = default)
        {
            PagedModel<EV.GetAll.EventModel> response = await mediator.Send(new EV.GetAll.Request() { Filter = filter }, token);
            
            return TypedResults.Ok(response);
        }
        
        public static async Task<IResult> Get(
            int id, 
            [FromServices] IMediator mediator, 
            CancellationToken token = default)
        {
            EV.Get.Model response = await mediator.Send(new EV.Get.Request { Id = id }, token);
            
            return TypedResults.Ok(response);
        }

        public static async Task<IResult> Delete(
            int id, 
            [FromServices] IMediator mediator,
            CancellationToken token = default)
        {
            await mediator.Send(new EV.Delete.Request { Id = id }, token);
            
            return TypedResults.Ok();
        }

        public static async Task<IResult> Schedule(
            [FromBody] EV.Schedule.Request request, 
            [FromServices] IMediator mediator,
            CancellationToken token = default)
        {
            EV.Schedule.Model response = await mediator.Send(request, token);

            return TypedResults.Ok(response.EventId);
        }

        public static async Task<IResult> RSVP(
            int id,
            [FromBody] RSVPModel model, 
            [FromServices] IMediator mediator,
            CancellationToken token = default)
        {
            await mediator.Send(new EV.RSVP.Request { EventId = id, Status = model.Status }, token);
            
            return TypedResults.Ok();
        }

        public sealed class RSVPModel
        {
            public ResponseStatus Status { get; set; }
        }
    }
    
    public static class BucketList
    {
        public static async Task<IResult> GetAll(
            [AsParameters] FilterModel filter, 
            [FromQuery] bool? unplanned, 
            [FromServices] IMediator mediator, 
            CancellationToken token = default)
        {
            PagedModel<BL.GetAll.BucketListModel> response = await mediator.Send(new BL.GetAll.Request() { Filter = filter, Unplanned = unplanned }, token);
            
            return TypedResults.Ok(response);
        }

        public static async Task<IResult> Get(
            int id, 
            [FromServices] IMediator mediator,
            CancellationToken token = default)
        {
            BL.Get.Model response = await mediator.Send(new BL.Get.Request { Id = id }, token);
            
            return TypedResults.Ok(response);
        }
        
        public static async Task<IResult> Delete(
            int id, 
            [FromServices] IMediator mediator,
            CancellationToken token = default)
        {
            await mediator.Send(new BL.Delete.Request { Id = id }, token);
            
            return TypedResults.NoContent();
        }
    }
}