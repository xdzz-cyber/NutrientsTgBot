using Application.Common.Exceptions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserList;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListVm>
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public GetUserListQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<UserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        if (users == null)
        {
            throw new NotFoundException(nameof(AppUser), request.GetHashCode());
        }

        return new UserListVm() {Users = users};
    }
}
