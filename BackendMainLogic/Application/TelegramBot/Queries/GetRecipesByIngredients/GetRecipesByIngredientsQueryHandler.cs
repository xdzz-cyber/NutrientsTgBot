using System.Text;
using System.Text.RegularExpressions;
using Application.Common.Constants;
using Application.Common.Mappings;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.TelegramBot.Queries.GetRecipesByIngredients;

public class GetRecipesByIngredientsQueryHandler : IRequestHandler<GetRecipesByIngredientsQuery, string>
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ITelegramBotDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;

    public GetRecipesByIngredientsQueryHandler(HttpClient httpClient, IMapper mapper, 
        ITelegramBotDbContext ctx, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _httpClient = httpClient;
        _ctx = ctx;
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetRecipesByIngredientsQuery request, CancellationToken cancellationToken)
    {
        if (request.Ingredients.Split(",").Select(ing => ing.Trim()).ToArray().Length < 2)
        {
            return "Please, enter an ingredients with comma as separator";
        }

        var matchPattern = TelegramBotRecipeCommandsNQueriesDataPatterns.IngredientsCommaSeparated;

        var ingredients = string.Join("",Regex.Matches(request.Ingredients, matchPattern)
            .Select(m => m.Value)
            .ToArray());
        
        var recipes =
           await _httpClient.GetAsync($"{TelegramBotRecipesHttpPaths.GetRecipes}{ingredients}", 
               cancellationToken: cancellationToken);

        var userInfo = await _userManager.FindByNameAsync(request.Username);
        
        if (userInfo is null)
        {
            return "Please, authorize to be able to make actions.";
        }
        
        RecipesList? content;
        
        var response = new StringBuilder();
        
        var msgResponse = "";
        
        var recipesIds = new List<string>();
        
        if (recipes.IsSuccessStatusCode)
        {
            var _ = await recipes.Content.ReadAsStringAsync(cancellationToken);
            
            content = JsonSerializer.Deserialize<RecipesList>(_);
            
            var finalRecipes = content!.Recipes.ToList();
            
            foreach (var recipe in finalRecipes)
            {

                var recipeFilters = await _ctx.RecipeFilters
                    .Where(rf => rf.RecipeFiltersUsers
                        .Any(rfu => rfu.RecipeFiltersId == rf.Id && rfu.IsTurnedIn))
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
                    var mappedRecipe = _mapper.Map<RecipeViewDto>(recipe);
                    
                    var msg = $"<strong>Title: {mappedRecipe.Title}, Vegetarian: {mappedRecipe.Vegetarian}, GlutenFree: {mappedRecipe.GlutenFree}, " +
                              $"PricePerServing: {mappedRecipe.PricePerServing}, Link: {mappedRecipe.SpoonacularSourceUrl} " +
                              $"Save recipe(/AddRecipeToUser_{mappedRecipe.Id})</strong>\n";
                    
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
