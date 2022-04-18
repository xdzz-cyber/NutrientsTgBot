using Application.Common.Mappings;
using AutoMapper;
using Domain;

namespace Application.Users.Queries.GetUserDetails;

public class UserDetailsVm : IMapWith<User>
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDetailsVm>().ForMember(x => x.Id, 
            opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Username, 
                opt => opt.MapFrom(x => x.Username));
    }
}
