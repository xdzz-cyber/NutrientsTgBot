using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Users.Queries.GetUserDetails;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserList;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, UserListVm>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly IMapper _mapper;

    public GetUserListQueryHandler(ITelegramBotDbContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }
    
    public async Task<UserListVm> Handle(GetUserListQuery request, CancellationToken cancellationToken)
    {
        var users = await _ctx.Users.
            Where(x => x.Username.Equals(request.Username)).
            ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        if (users == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        return new UserListVm()
        {
            Users = users
        };
    }
}
