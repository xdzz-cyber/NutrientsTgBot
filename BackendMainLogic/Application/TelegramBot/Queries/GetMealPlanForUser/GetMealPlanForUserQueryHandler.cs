using System.Text;
using System.Text.Json;
using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Queries.Dtos;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetMealPlanForUser;

public class GetMealPlanForUserQueryHandler : IRequestHandler<GetMealPlanForUserQuery, string>
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
    
    public async Task<string> Handle(GetMealPlanForUserQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var caloriesNutrientId = _ctx.Nutrients.FirstOrDefault(n => n.Name.Equals("Calories"))!.Id;
        
        var userCaloriesPreferences = await _ctx.NutrientUsers
            .FirstOrDefaultAsync(nu => nu.AppUserId == userInfo.Id && nu.NutrientId == caloriesNutrientId, cancellationToken: cancellationToken);

        var targetCalories = (userCaloriesPreferences!.MaxValue + userCaloriesPreferences.MinValue) / 2;

        var userRecipeFiltersIds = await _ctx.RecipeFiltersUsers
            .Where(rfu => rfu.AppUserId == userInfo.Id).Select(rfu => rfu.RecipeFiltersId).ToListAsync(cancellationToken);
        
        var dietFilters = await _ctx.RecipeFilters
            .Where(rf => userRecipeFiltersIds.Contains(rf.Id)).ToListAsync(cancellationToken);

        var diet = string.Join(',',dietFilters.Select(dietFilter => dietFilter.Name));
        
        var mealPlanHttpResponseMessage = await _httpClient
            .GetAsync(string.Format(TelegramBotRecipesHttpPaths.GetMealPlan, targetCalories, diet), cancellationToken);

        var response = new StringBuilder();
        var mealsIds = new List<string>();
        if (mealPlanHttpResponseMessage.IsSuccessStatusCode)
        {
            var mealsTmp = await mealPlanHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var meals = JsonSerializer.Deserialize<MealsList>(mealsTmp);
            
            var nutrientsTmp = await mealPlanHttpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            var nutrients =
                JsonSerializer.Deserialize<NutrientsList>(nutrientsTmp);

            foreach (var meal in meals.Meals)
            {
                if (_ctx.Recipes.FirstOrDefault(r => r.Id == meal.Id) is null)
                {
                    var recipeByIdHttpMessage = await _httpClient
                        .GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id", meal.Id.ToString()), cancellationToken);

                    var t = await recipeByIdHttpMessage.Content.ReadAsStringAsync(cancellationToken);
                    
                    var recipe = JsonSerializer.Deserialize<Recipe>(t);

                    await _ctx.Recipes.AddAsync(recipe!, cancellationToken);
                    await _ctx.SaveChangesAsync(cancellationToken);
                }
                
                var tmp = await _ctx.Recipes.FirstOrDefaultAsync(r => r.Id == meal.Id, cancellationToken: cancellationToken); //_mapper.Map<RecipeViewDto>(meal);

                var msg =
                    $"<strong>Title: {tmp!.Title}, Vegetarian: {tmp.Vegetarian}, GlutenFree: {tmp.GlutenFree}, PricePerServing: {tmp.PricePerServing}, Link: {(tmp.SpoonacularSourceUrl.Length > 0 ? tmp.SpoonacularSourceUrl : tmp.SourceUrl)} Save recipe(/AddRecipeToUser_{tmp.Id})</strong>";
                response.AppendLine(msg);
                response.AppendLine($"Save as a part of the meal(/AddRecipeAsPartOfMeal_{tmp.Id})");
                mealsIds.Add(meal.Id.ToString());
            }
            
            response.AppendLine("Add everything as part of the meal(/AddAllRecipesAsPartOfMeal)");
           
            var nutrientMessage = $"Calories = {nutrients!.Nutrients.Calories}, Fat = {nutrients.Nutrients.Fat}, " +
                                  $"Carbohydrates = {nutrients.Nutrients.Carbohydrates}, Protein = {nutrients.Nutrients.Protein}";

            response.AppendLine(nutrientMessage);
            
            if (string.IsNullOrEmpty(response.ToString()))
            {
                return "No meals found.";
            }

            if (!StateManagement.TempData.ContainsKey("MealsIds"))
            {
                StateManagement.TempData.Add("MealsIds", string.Join(",", mealsIds));
            }
            else
            {
                StateManagement.TempData["MealsIds"] = string.Join(",", mealsIds);
            }
        }

        return response.ToString();
    }
}
