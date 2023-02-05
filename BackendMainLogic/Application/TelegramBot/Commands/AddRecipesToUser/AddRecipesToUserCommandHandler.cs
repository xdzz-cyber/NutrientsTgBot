using System.Globalization;
using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Commands.AddRecipeToUser;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Commands.AddRecipesToUser;

public class AddRecipesToUserCommandHandler : IRequestHandler<AddRecipesToUserCommand, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;

    public AddRecipesToUserCommandHandler(ITelegramBotDbContext ctx, UserManager<AppUser> userManager,
        HttpClient httpClient)
    {
        _ctx = ctx;
        _userManager = userManager;
        _httpClient = httpClient; 
    }
    public async Task<string> Handle(AddRecipesToUserCommand request, CancellationToken cancellationToken)
    {

        try
        {

            var user = await _userManager.FindByNameAsync(request.Username);

            if (_ctx.RecipesUsers.Count(ru => ru.AppUserId == user.Id) == TelegramBotRecipesPerUserAmount
                    .MaxRecipesPerUser)
            {
                return $"Max limit of saved recipes is exceeded({TelegramBotRecipesPerUserAmount.MaxRecipesPerUser} at most).";
            }

            foreach (var recipeId in request.RecipeIds)
            {
                if (_ctx.RecipesUsers.Count() >= TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
                {
                    return "Not all recipes have been saved because max limit has been reached.";
                }
                
                await _ctx.RecipesUsers.AddAsync(
                    new RecipesUsers
                    {
                        AppUserId = user.Id,
                        RecipeId = int.Parse(recipeId, NumberStyles.Integer)
                    }, cancellationToken);
            }

            // if (_ctx.RecipesUsers.Any(recipesUsers => recipesUsers.RecipeId.ToString() ==  request.RecipeId 
            //                                           && recipesUsers.AppUserId == user.Id))
            // {
            //     return "The chosen recipe has already been added.";
            // }

            

           
            // else if (!string.IsNullOrEmpty(request.RecipeId))
            // {
            //     var recipesIds = StateManagement.TempData["RecipesIds"].Split(',');
            //
            //     var addedDataToRecipeUsersCounter = 0;
            //
            //     foreach (var id in recipesIds)
            //     {
            //         if (!_ctx.Recipes.Any(r => r.Id.ToString() == id))
            //         {
            //             var recipeToBeAddedHttpMessage = await _httpClient
            //                 .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id", id),cancellationToken);
            //             var recipeToBeAdded = JsonSerializer.Deserialize<Recipe>(
            //                 await recipeToBeAddedHttpMessage.Content.ReadAsStreamAsync(cancellationToken));
            //             
            //             if (string.IsNullOrEmpty(recipeToBeAdded!.SourceName))
            //             {
            //                 recipeToBeAdded.SourceName = "";
            //             }
            //             
            //             await _ctx.Recipes.AddAsync(recipeToBeAdded, cancellationToken);
            //             await _ctx.SaveChangesAsync(cancellationToken);// might be shit
            //         }
            //         
            //         if (!_ctx.RecipesUsers.Any(ru => ru.RecipeId.ToString() == id) 
            //             && _ctx.RecipesUsers.Count() + addedDataToRecipeUsersCounter < TelegramBotRecipesPerUserAmount.MaxRecipesPerUser)
            //         {
            //             await _ctx.RecipesUsers.AddAsync(new RecipesUsers
            //             {
            //                 AppUserId = user.Id,
            //                 RecipeId = Convert.ToInt32(id)
            //             }, cancellationToken);
            //             addedDataToRecipeUsersCounter += 1;
            //         }
            //     }
            //     
            // }
            await _ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return "Inner server error.";
        }

        return "New recipes have been added successfully";
    }
}
