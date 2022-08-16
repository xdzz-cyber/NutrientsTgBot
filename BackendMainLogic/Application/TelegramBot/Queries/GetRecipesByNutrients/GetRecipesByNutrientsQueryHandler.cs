using System.Text;
using Application.Common.Constants;
using Application.Interfaces;
using Application.TelegramBot.Queries.Dtos;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.TelegramBot.Queries.GetRecipesByNutrients;

public class GetRecipesByNutrientsQueryHandler : IRequestHandler<GetRecipesByNutrientsQuery, string>
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    
    public GetRecipesByNutrientsQueryHandler(HttpClient httpClient, IMapper mapper, 
        ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetRecipesByNutrientsQuery request, CancellationToken cancellationToken)
    {
        // if (request.Nutrients.Split(",").Select(ing => ing.Trim()).ToArray().Length < 2)
        // {
        //     return "Please, enter nutrients with comma as separator";
        // }
        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }

        if (!_ctx.NutrientUsers.Any(nu => nu.AppUserId == userInfo.Id))
        {
            return "Please, set your nutrients plan.";
        }

        var userNutrients = await _ctx.NutrientUsers
            .Where(nu => nu.AppUserId == userInfo.Id).ToListAsync(cancellationToken);

        var nutrients = await _ctx.Nutrients.ToListAsync(cancellationToken);

        var userNutrientsValues = new List<UserNutrientPlanDto>();

        foreach (var nutrient in nutrients)
        {
            var currentUserNutrientData =
                userNutrients.FirstOrDefault(userNutrient => userNutrient.NutrientId == nutrient.Id);
            
            userNutrientsValues.Add(new UserNutrientPlanDto
            {
                Name = nutrient.Name,
                MinValue = currentUserNutrientData!.MinValue,
                MaxValue = currentUserNutrientData.MaxValue
            });
        }

        var userNutrientsCarbs = userNutrientsValues.First(x => x.Name.Equals("Carbohydrates"));
        var userNutrientsProtein = userNutrientsValues.First(x => x.Name.Equals("Protein"));
        var userNutrientsFat = userNutrientsValues.First(x => x.Name.Equals("Fat"));
        var userNutrientsCalories = userNutrientsValues.First(x => x.Name.Equals("Calories"));

        var tmpString = string.Format(TelegramBotRecipesHttpPaths.GetRecipesByNutrients,
            userNutrientsCarbs.MinValue, userNutrientsCarbs.MaxValue, userNutrientsProtein.MinValue,
            userNutrientsProtein.MaxValue, userNutrientsFat.MinValue,
            userNutrientsFat.MaxValue, userNutrientsCalories.MinValue, userNutrientsCalories.MaxValue);
        
        var recipes =
           await _httpClient.GetAsync(tmpString, cancellationToken);

        
        var content = new List<RecipeViewDto>();
        var response = new StringBuilder();
        var msgResponse = "";
        var recipesIds = new List<string>();
        if (recipes.IsSuccessStatusCode)
        {
            var r = await recipes.Content.ReadAsStringAsync(cancellationToken);
            content = JsonSerializer.Deserialize<List<RecipeViewDto>>(r);
            //var recipeFilters = _ctx.RecipeFiltersUsers.Where(rfu => rf.)
            var finalRecipes = content;
            foreach (var recipe in finalRecipes)
            {
                var currentRecipeDiets = new Recipe();

                if (_ctx.Recipes.FirstOrDefault(r => r.Id == recipe.Id) is null)
                {
                    var currentRecipeDietsRecipe = await _httpClient.GetAsync(TelegramBotRecipesHttpPaths.GetRecipeById.Replace("id", recipe.Id.ToString()), cancellationToken);
                    var currentRecipeDietsRecipeContent = JsonSerializer
                        .Deserialize<RecipeViewDto>(await currentRecipeDietsRecipe.Content.ReadAsStringAsync(cancellationToken));

                    currentRecipeDiets.Vegetarian = currentRecipeDietsRecipeContent!.Vegetarian;
                    currentRecipeDiets.GlutenFree = currentRecipeDietsRecipeContent.GlutenFree;
                    currentRecipeDiets.PricePerServing = currentRecipeDietsRecipeContent.PricePerServing;
                    currentRecipeDiets.SpoonacularSourceUrl = currentRecipeDietsRecipeContent.SpoonacularSourceUrl;
                }
                else
                {
                    currentRecipeDiets = _ctx.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                }
                

                recipe.Vegetarian = currentRecipeDiets!.Vegetarian;
                recipe.GlutenFree = currentRecipeDiets.GlutenFree;
                recipe.PricePerServing = currentRecipeDiets.PricePerServing;
                recipe.SpoonacularSourceUrl = currentRecipeDiets.SpoonacularSourceUrl;
                
                var recipeFilters = await _ctx.RecipeFilters
                    .Where(rf => rf.RecipeFiltersUsers.Any(rfu => rfu.RecipeFiltersId == rf.Id && rfu.IsTurnedIn))
                    .ToListAsync(cancellationToken);
                
                var satisfiesFiltersCount = 0;
                foreach (var recipeFilter in recipeFilters)
                {
                    if ((nameof(recipe.Vegetarian).Equals(recipeFilter.Name) && recipe.Vegetarian)
                        || (nameof(recipe.GlutenFree).Equals(recipeFilter.Name) && recipe.GlutenFree))
                    {
                        satisfiesFiltersCount += 1;
                    }
                }

                var doesRecipeFitUserFilters = satisfiesFiltersCount == recipeFilters.Count;

                if (doesRecipeFitUserFilters)
                {
                    var tmp = _mapper.Map<RecipeViewDto>(recipe);
                    var msg = $"<strong>Title: {tmp.Title}, Vegetarian: {tmp.Vegetarian}, GlutenFree: {tmp.GlutenFree}," +
                              $" PricePerServing: {tmp.PricePerServing}, " +
                              $"Link: {tmp.SpoonacularSourceUrl} \nSave recipe(/AddRecipeToUser_{tmp.Id})</strong>\n";
                    response.AppendLine(msg);
                    msgResponse += msg;
                    recipesIds.Add(recipe.Id.ToString());
                }
            }

            if (string.IsNullOrEmpty(response.ToString()))
            {
                return "No recipes found.";
            }
            
            response.AppendLine("Save all(/AddRecipeToUser_All)");

            if (!StateManagement.TempData.ContainsKey("RecipesIds"))
            {
                StateManagement.TempData.Add("RecipesIds", string.Join(",", recipesIds));
            }
            else
            {
                StateManagement.TempData["RecipesIds"] = string.Join(",", recipesIds);
            }
            
        }

        return response.ToString();
    }
}
