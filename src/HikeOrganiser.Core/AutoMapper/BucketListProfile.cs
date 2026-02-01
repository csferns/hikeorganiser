using HikeOrganiser.Data.Entities;
using BucketList = HikeOrganiser.Core.Behaviours.BucketList;

namespace HikeOrganiser.Core.AutoMapper;

public class BucketListProfile : Profile
{
    public BucketListProfile()
    {
        CreateMap<Data.Entities.BucketList, BucketList.GetAll.BucketListModel>()
            .ForMember(dest => dest.UserDisplayName, opt => opt.MapFrom(src => src.SuggestedBy!.DisplayName));
        
        CreateMap<Data.Entities.BucketList, BucketList.Get.BucketListModel>();
    }
}