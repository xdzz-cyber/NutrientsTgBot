using Application.Common.Exceptions;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public GetUserDetailsQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            throw new NotFoundException(nameof(AppUser), request.Username);
        }

        return _mapper.Map<UserDetailsVm>(user);
    }
}
