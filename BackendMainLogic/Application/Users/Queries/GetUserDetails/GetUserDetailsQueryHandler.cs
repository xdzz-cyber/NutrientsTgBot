using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly IMapper _mapper;

    public GetUserDetailsQueryHandler(ITelegramBotDbContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }
    
    public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), request.Id);
        }

        return _mapper.Map<UserDetailsVm>(user);
    }
}
