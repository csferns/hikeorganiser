using System.Security.Claims;
using HikeOrganiser.API;
using HikeOrganiser.Core;
using HikeOrganiser.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.WithOrigins("https://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
builder.Services.AddResponseCaching();
builder.Services.AddOutputCache(opt => opt.AddBasePolicy(b => b.Expire(TimeSpan.FromSeconds(30))));

builder.Services.AddCoreServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCaching();
app.UseOutputCache();

var authGroup = app.MapGroup("auth");
authGroup.MapIdentityApi<User>();
authGroup.MapGet("/manage/fullinfo",
    async ([FromServices] UserManager<User> manager, ClaimsPrincipal user) =>
    {
        var response = await manager.GetUserAsync(user);

        if (response == null)
        {
            return (IResult)TypedResults.NotFound();
        }
        
        return (IResult)TypedResults.Ok(new { response.UserName, response.DisplayName, response.Email, response.EmailConfirmed });
    })
    .RequireAuthorization();

app.MapHealthChecks("/healthcheck");
        
RouteGroupBuilder apiGroup = app.MapGroup("api");

apiGroup.MapGet("/event", ApiEndpoints.Events.GetAll);
apiGroup.MapGet("/event/{id:int}", ApiEndpoints.Events.Get).WithName("GetEventById");
apiGroup.MapDelete("/event/{id:int}", ApiEndpoints.Events.Delete);
apiGroup.MapPut("/event/schedule", ApiEndpoints.Events.Schedule);
apiGroup.MapPut("/event/{id:int}/rsvp", ApiEndpoints.Events.RSVP);
        
apiGroup.MapGet("/bucketlist", ApiEndpoints.BucketList.GetAll);
apiGroup.MapGet("/bucketlist/{id:int}", ApiEndpoints.BucketList.Get);
apiGroup.MapDelete("/bucketlist/{id:int}", ApiEndpoints.BucketList.Delete);

app.Run();