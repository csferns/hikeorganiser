using HikeOrganiser.Data.Entities;
using HikeOrganiser.Data.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.Users.UserRegister;

public sealed record Request : IRequest<Model>
{
    public string? ReferralCode { get; set; }
    
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public sealed record Model
{
    public Guid CreatedUserId { get; init; }
}

public sealed class Handler : IRequestHandler<Request, Model>
{
    private readonly HikeOrganiserContext _context;
    private readonly UserManager<User> _userManager;
    
    public Handler(HikeOrganiserContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Model> Handle(Request request, CancellationToken cancellationToken)
    {
        User user = new()
        {
            Email = request.Email
        };

        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
        
        if (!string.IsNullOrEmpty(request.ReferralCode))
        {
            Guid referralUser = await 
            (
                from u in _context.Users
                join ur in _context.UserReferralCodes on u.Id equals ur.UserId
                join r in _context.ReferralCodes on ur.ReferralCodeId equals r.Id
                where r.Code == request.ReferralCode
                select u.Id
            )
            .TagWithCallSite()
            .SingleOrDefaultAsync(cancellationToken);

            user.ReferredByUserId = referralUser;
        }
        
        IdentityResult createResult = await _userManager.CreateAsync(user);
        
        return new()
        {
            CreatedUserId = user.Id,
        };
    }
}