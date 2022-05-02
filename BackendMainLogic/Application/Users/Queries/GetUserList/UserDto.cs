using Application.Common.Mappings;
using AutoMapper;
using Domain.TelegramBotEntities;

namespace Application.Users.Queries.GetUserList;

public class UserDto : IMapWith<AppUser>
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public long ChatId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AppUser, UserDto>().ForMember(x => x.Id,
                opt => opt.MapFrom(x => Guid.Parse(x.Id)))
            .ForMember(x => x.Username,
                opt => opt.MapFrom(x => x.UserName))
            .ForMember(x => x.Email, opt => opt.MapFrom(x => x.Email))
            .ForPath(x => Password, opt => opt.MapFrom(x => x.PasswordHash))
            .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.PhoneNumber))
            .ForMember(x => x.ChatId, opt => opt.MapFrom(x => x.ChatId));
    }
}
