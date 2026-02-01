using HikeOrganiser.Data.Entities;

namespace HikeOrganiser.Core.Interfaces;

public interface ICurrentUserService
{
    Task<User> GetAsync(CancellationToken token = default);
}