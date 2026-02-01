using System.Security.Claims;
using HikeOrganiser.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HikeOrganiser.Core.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetAsync(CancellationToken token = default)
    {
        if (_httpContextAccessor.HttpContext?.User is not { Identity.IsAuthenticated: true } current)
        {
            throw new InvalidOperationException("The current user is null or invalid.");
        }

        return await _userManager.GetUserAsync(current)
            ?? throw new InvalidOperationException("Could not find the current user.");
    }
}