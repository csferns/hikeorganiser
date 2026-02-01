using AutoMapper.QueryableExtensions;
using HikeOrganiser.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HikeOrganiser.Core.Behaviours.BucketList.Get;

public sealed class Handler : IRequestHandler<Request, Model>
{
    private readonly HikeOrganiserContext _hikeOrganiserContext;
    private readonly IMapper _mapper;

    public Handler(HikeOrganiserContext hikeOrganiserContext, IMapper mapper)
    {
        _hikeOrganiserContext = hikeOrganiserContext;
        _mapper = mapper;
    }

    public async Task<Model> Handle(Request request, CancellationToken cancellationToken)
    {
        BucketListModel? model = await _hikeOrganiserContext.BucketLists
            .TagWithCallSite()
            .AsNoTracking()
            .ProjectTo<BucketListModel>(_mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
        
        return new()
        {
            BucketList = model,
        };
    }
}