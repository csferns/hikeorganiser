using AutoMapper.QueryableExtensions;
using HikeOrganiser.Core.Extensions;
using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.BucketList.GetAll;

public sealed class Handler : IRequestHandler<Request, PagedModel<BucketListModel>>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly IMapper _mapper;

    public Handler(HikeOrganiserContext hikeOrganiserContext, IMapper mapper)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _mapper = mapper;
    }

    public async Task<PagedModel<BucketListModel>> Handle(Request request, CancellationToken cancellationToken)
    {
        IQueryable<BucketListModel> query = _hikeOrganiserContext.BucketLists
            .TagWithCallSite()
            .AsNoTracking()
            .Where(x => !request.Unplanned.HasValue || x.PlannedEventId.HasValue == !request.Unplanned)
            .OrderBy(x => x.FriendlyName)
            .ProjectTo<BucketListModel>(_mapper.ConfigurationProvider);

        (List<BucketListModel> results, int count) = await query.PageAsync(request.Filter, cancellationToken);
        
        return new()
        {
            FullCount = count,
            Items = results
        };
    }
}