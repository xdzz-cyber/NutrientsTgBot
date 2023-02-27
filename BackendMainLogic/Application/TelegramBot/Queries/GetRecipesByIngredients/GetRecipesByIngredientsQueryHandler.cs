using Application.Common.Constants;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using AutoMapper;
using Domain.TelegramBotEntities.RecipesNutrition;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.TelegramBot.Queries.GetRecipesByIngredients;

public class GetRecipesByIngredientsQueryHandler : IRequestHandler<GetRecipesByIngredientsQuery, List<Recipe>>
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
    
    public async Task<List<Recipe>> Handle(GetRecipesByIngredientsQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

        var foundRecipes = new List<Recipe>();
        
        var recipes =
           await _httpClient.GetAsync($"{TelegramBotRecipesHttpPaths.GetRecipesDataByIngredients}{request.Ingredients}", 
               cancellationToken: cancellationToken);

        List<RecipesNutrition>? content;

        if (recipes.IsSuccessStatusCode)
        {
            var _ = await recipes.Content.ReadAsStringAsync(cancellationToken);
            
            content = JsonSerializer.Deserialize<List<RecipesNutrition>>(_);

            foreach (var recipe in content!)
            {

                var recipeFilters = await _ctx.RecipeFilters
                    .Where(rf => rf.RecipeFiltersUsers
                        .Any(rfu => rfu.RecipeFiltersId == rf.Id && rfu.IsTurnedIn 
                                                                 && rfu.AppUserId == userInfo.Id))
                    .ToListAsync(cancellationToken);

                var vegetarianFilter = _ctx.RecipeFilters.First(rf => rf.Name == "Vegetarian");
                
                var glutenFreeFilter = _ctx.RecipeFilters.First(rf => rf.Name == "GlutenFree");
                
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
                    foundRecipes.Add(new Recipe
                    {
                        Id = recipe.Id,
                        Title = recipe.Title,
                        Vegetarian = _ctx.RecipeFiltersUsers.First(rfu => rfu.RecipeFiltersId == vegetarianFilter.Id).IsTurnedIn,
                        GlutenFree = _ctx.RecipeFiltersUsers.First(rfu => rfu.RecipeFiltersId == glutenFreeFilter.Id).IsTurnedIn,
                        SourceUrl = recipe.Image
                    });
                }
            }
        }
        return foundRecipes;
    }
}
