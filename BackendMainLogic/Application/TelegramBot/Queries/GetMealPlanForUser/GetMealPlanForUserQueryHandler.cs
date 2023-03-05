using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetMealPlanForUser;

public class GetMealPlanForUserQueryHandler : IRequestHandler<GetMealPlanForUserQuery, Tuple<List<Recipe>, NutrientViewDto>>
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetMealPlanForUserQueryHandler(HttpClient httpClient, IMapper mapper, 
        ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<Tuple<List<Recipe>, NutrientViewDto>> Handle(GetMealPlanForUserQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var caloriesNutrientId = _ctx.Nutrients.FirstOrDefault(n => n.Name.Equals("Calories"))!.Id;
        
        var userCaloriesPreferences = await _ctx.NutrientUsers
            .FirstOrDefaultAsync(nu => nu.AppUserId == userInfo.Id 
                                       && nu.NutrientId == caloriesNutrientId, cancellationToken: cancellationToken);

        if (userCaloriesPreferences is null)
        {
            return new Tuple<List<Recipe>, NutrientViewDto>(new List<Recipe>(),new NutrientViewDto());
        }

        var targetCalories = (userCaloriesPreferences!.MaxValue + userCaloriesPreferences.MinValue) / 2;

        var userRecipeFiltersIds = await _ctx.RecipeFiltersUsers
            .Where(rfu => rfu.AppUserId == userInfo.Id && rfu.IsTurnedIn).Select(rfu => rfu.RecipeFiltersId).ToListAsync(cancellationToken);
        
        var dietFilters = await _ctx.RecipeFilters
            .Where(rf => userRecipeFiltersIds.Contains(rf.Id)).ToListAsync(cancellationToken);

        var diet = string.Join(',',dietFilters.Select(dietFilter => dietFilter.Name));
        
        var mealPlanHttpResponseMessage = await _httpClient
            .GetAsync(string.Format(TelegramBotRecipesHttpPaths.GetMealPlan, targetCalories, diet), cancellationToken);

        var response = new Tuple<List<Recipe>, NutrientViewDto>(new List<Recipe>(), new NutrientViewDto());
        
        if (mealPlanHttpResponseMessage.IsSuccessStatusCode)
        {
            var mealsTmp = await mealPlanHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var meals = JsonSerializer.Deserialize<MealsList>(mealsTmp);
            
            var nutrientsTmp = await mealPlanHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var nutrients =
                JsonSerializer.Deserialize<NutrientsList>(nutrientsTmp);

            foreach (var meal in meals!.Meals)
            {
                if (_ctx.Recipes.FirstOrDefault(r => r.Id == meal.Id) is null)
                {
                    var recipeByIdHttpMessage = await _httpClient
                        .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById
                            .Replace("id", meal.Id.ToString()), cancellationToken);

                    var t = await recipeByIdHttpMessage.Content.ReadAsStringAsync(cancellationToken);
                    
                    var recipe = JsonSerializer.Deserialize<Recipe>(t);

                    if (recipe!.SourceName is null)
                    {
                        recipe.SourceName = "";
                    }

                    await _ctx.Recipes.AddAsync(recipe, cancellationToken);
                    await _ctx.SaveChangesAsync(cancellationToken);
                }
                
                var tmp = await _ctx.Recipes.FirstOrDefaultAsync(r => r.Id == meal.Id, 
                    cancellationToken: cancellationToken);
                
                response.Item1.Add(tmp!);
            }

            response.Item2.Calories = nutrients!.Nutrients.Calories;
            response.Item2.Fat = nutrients.Nutrients.Fat;
            response.Item2.Carbohydrates = nutrients.Nutrients.Carbohydrates;
            response.Item2.Protein = nutrients.Nutrients.Protein;
        }

        return response;
    }
}
