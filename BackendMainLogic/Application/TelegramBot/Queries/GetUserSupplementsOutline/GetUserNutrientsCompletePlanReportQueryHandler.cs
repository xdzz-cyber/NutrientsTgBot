using System.Text;
using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Queries.Dtos.RecipesNutrition;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetUserSupplementsOutline;

public class GetUserSupplementsOutlineQueryHandler : IRequestHandler<GetUserSupplementsOutlineQuery, string>
{
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly HttpClient _httpClient;

    public GetUserSupplementsOutlineQueryHandler(ITelegramBotDbContext ctx, 
        UserManager<AppUser> userManager, HttpClient httpClient)
    {
        _ctx = ctx;
        _userManager = userManager;
        _httpClient = httpClient;
    }
    
    public async Task<string> Handle(GetUserSupplementsOutlineQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
        
        var userMealsIds = await _ctx.RecipesUsers
            .Where(ru => ru.IsPartOfTheMeal && ru.AppUserId == userInfo.Id)
            .Select(ru => ru.RecipeId).ToListAsync(cancellationToken);

        if (!userMealsIds.Any())
        {
            return "You have no meals.";
        }

        var recipesNutritionHttpMessage = await _httpClient
            .GetAsync(string.Format(TelegramBotRecipesHttpPaths.GetRecipesWithNutrition, string.Join(",", userMealsIds)), cancellationToken);

        var recipesNutritionData = await recipesNutritionHttpMessage.Content.ReadAsStringAsync(cancellationToken);

        var recipes = JsonSerializer.Deserialize<List<RecipesNutrition>>(recipesNutritionData);
        
        var recipeNutrients = new List<RecipeNutrientViewDto>();
        
        var response = new StringBuilder();
        var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);
        
        foreach (var recipe in recipes!)
        {
            var neededNutrients = recipe.Nutrition.Nutrients
                .Where(rn => nutrients.FirstOrDefault(n => n.Name.Equals(rn.Name)) != null).ToList();

            foreach (var neededNutrient in neededNutrients)
            {
                if (recipeNutrients.Any(rn => rn.Name.Equals(neededNutrient.Name)))
                {
                    var nutrientThatAlreadyBeenAdded = recipeNutrients
                        .FirstOrDefault(rn => rn.Name.Equals(neededNutrient.Name));

                    nutrientThatAlreadyBeenAdded!.Amount += neededNutrient.Amount;
                }
                else
                {
                    recipeNutrients.Add(neededNutrient);
                }
            }
        }
        
        foreach (var recipeNutrient in recipeNutrients)
        {
            var nutrientByName = nutrients.FirstOrDefault(n => n.Name.Equals(recipeNutrient.Name));

            var nutrientMeasurementUnit = nutrientByName!.Name.Equals("Calories") ? "k" : "g";
            
            var userPreferenceForCurrentNutrient = await _ctx.NutrientUsers
                .FirstOrDefaultAsync(nu => nu.NutrientId == nutrientByName!.Id, 
                    cancellationToken: cancellationToken);

            if (userPreferenceForCurrentNutrient is null)
            {
                return "Cannot create nutrients report because of absence of needed nutrient(s).";
            }
        
            var responseMessageForCurrentNutrient = "";
        
            if (recipeNutrient.Amount <= userPreferenceForCurrentNutrient.MaxValue
                && recipeNutrient.Amount >= userPreferenceForCurrentNutrient.MinValue)
            {
                responseMessageForCurrentNutrient = $"<strong>{recipeNutrient.Name}</strong>: " +
                                                    $"{Math.Abs(recipeNutrient.Amount - userPreferenceForCurrentNutrient.MaxValue)}{nutrientMeasurementUnit} still need to be consumed.";
            } else if (recipeNutrient.Amount < userPreferenceForCurrentNutrient.MinValue)
            {
                responseMessageForCurrentNutrient = $"<strong>{recipeNutrient.Name}</strong> is below your preferences. " +
                                                    $"You have to consume {Math.Abs(userPreferenceForCurrentNutrient.MinValue - recipeNutrient.Amount)}{nutrientMeasurementUnit} more.";
            }
            else if (recipeNutrient.Amount > userPreferenceForCurrentNutrient.MaxValue)
            {
                responseMessageForCurrentNutrient =
                    $"<strong>{recipeNutrient.Name}</strong> is above your preferences." +
                    $" You have to consume {Math.Abs(recipeNutrient.Amount - userPreferenceForCurrentNutrient.MaxValue)}{nutrientMeasurementUnit} less.";
            }
        
            response.AppendLine(responseMessageForCurrentNutrient);
        }

        return response.ToString();
        // 1) get fat,carbo,protein and calories of each recipe
        // 2) compare them with data from nutrientsUsers table by user id and each nutrient's one (id)
        // 3) make a response for each type of nutri
        // 4) return response
    }
}
