using Application.Common.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetUserDetails;

public class UserDetailsVm : IMapWith<IdentityUser>
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<IdentityUser, UserDetailsVm>().ForMember(x => x.Id, 
            opt => opt.MapFrom(x => Guid.Parse(x.Id)))
            .ForMember(x => x.Username, 
                opt => opt.MapFrom(x => x.UserName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.PhoneNumber));
    }
}
