using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddRecipeFiltersToUser;

public class AddRecipeFiltersToUserCommandHandler : IRequestHandler<AddRecipeFiltersToUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public AddRecipeFiltersToUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(AddRecipeFiltersToUserCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        var filters = request.Filters.Split(',');

        if (!filters.All(f => TelegramBotRecipeFilters.RecipeFilters.Contains(f)))
        {
            return "Please, enter one filter or many with comma as a separator.";
        }
        var recipeFiltersChosenByUser = await _ctx.RecipeFilters
            .Where(rf => filters.Contains(rf.Name)).ToListAsync(cancellationToken);

        var recipeFiltersUsers = new List<RecipeFiltersUsers>();
        
        foreach (var recipeFilter in recipeFiltersChosenByUser)
        {
            if (_ctx.RecipeFiltersUsers.Any(rfu => rfu.AppUserId == userInfo.Id
                                                   && rfu.RecipeFiltersId == recipeFilter.Id))
            {
                var rf = await _ctx.RecipeFiltersUsers.FirstAsync(rfu => rfu.AppUserId == userInfo.Id
                                                            && rfu.RecipeFiltersId == recipeFilter.Id, cancellationToken: cancellationToken);
                rf.IsTurnedIn = true;
            }
            else
            {
                recipeFiltersUsers.Add(new RecipeFiltersUsers
                {
                    AppUserId = userInfo.Id,
                    RecipeFiltersId = _ctx.RecipeFilters.First(f => f.Name == recipeFilter.Name).Id,
                    IsTurnedIn = true
                });
            }
        }
        

        await _ctx.RecipeFiltersUsers.AddRangeAsync(recipeFiltersUsers, cancellationToken);
        
        await _ctx.SaveChangesAsync(cancellationToken);

        return "Filters have been successfully saved.";
    }
}
