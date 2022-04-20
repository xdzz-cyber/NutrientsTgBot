using Application.Common.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetUserDetails;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsVm>
{
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public GetUserDetailsQueryHandler(IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public async Task<UserDetailsVm> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            throw new NotFoundException(nameof(IdentityUser), request.Id);
        }

        return _mapper.Map<UserDetailsVm>(user);
    }
}
