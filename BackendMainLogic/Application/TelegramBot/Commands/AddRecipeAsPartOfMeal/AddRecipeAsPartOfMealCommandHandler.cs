using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Commands.AddRecipeAsPartOfMeal;

public class AddRecipeAsPartOfMealCommandHandler : IRequestHandler<AddRecipeAsPartOfMealCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;

    public AddRecipeAsPartOfMealCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager, IMediator mediator)
    {
        _ctx = ctx;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<string> Handle(AddRecipeAsPartOfMealCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        if (await _ctx.RecipesUsers
                .FirstOrDefaultAsync(ru => ru.RecipeId.ToString() == request.RecipeId 
                                           && ru.AppUserId == userInfo.Id, cancellationToken) is null && _ctx.RecipesUsers.Count() 
            < TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
        {
           await _mediator.Send(new AddRecipeToUserCommand(request.Username, request.RecipeId), cancellationToken);
        }
        // else if (recipe is not null && recipe.IsPartOfTheMeal)
        // {
        //     return "Recipe has already been added as part of the meal.";
        // }

        var recipe = await _ctx.RecipesUsers.FirstAsync(ru =>
            ru.RecipeId.ToString() == request.RecipeId && ru.AppUserId == userInfo.Id,
            cancellationToken: cancellationToken);
        
        recipe.IsPartOfTheMeal = true;
        
        await _ctx.SaveChangesAsync(cancellationToken);
        
        return "Recipe has been successfully added as part of the meal";
    }
}
