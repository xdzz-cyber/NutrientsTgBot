using System.Text;
using Application.Common.Constants;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.TelegramBot.Queries.GetRecipesByIngredients;

public class GetRecipesByIngredientsQueryHandler : IRequestHandler<GetRecipesByIngredientsQuery, string>
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;

    public GetRecipesByIngredientsQueryHandler(HttpClient httpClient, IMapper mapper)
    {
        _mapper = mapper;
        _httpClient = httpClient;
    }
    
    public async Task<string> Handle(GetRecipesByIngredientsQuery request, CancellationToken cancellationToken)
    {
        if (request.Ingredients.Split(",").Select(ing => ing.Trim()).ToArray().Length < 2)
        {
            return "Please, enter an ingredients with comma as separator";
        }
        
        var recipes =
           await _httpClient.GetAsync($"{TelegramBotRecipesHttpPaths.GetRecipes}{request.Ingredients}", cancellationToken: cancellationToken);

        var content = new RecipesList();
        var response = new StringBuilder();
        var msgResponse = "";
        var recipesIds = new List<string>();
        if (recipes.IsSuccessStatusCode)
        {
            var r = await recipes.Content.ReadAsStringAsync(cancellationToken);
            content = JsonSerializer.Deserialize<RecipesList>(r);
            foreach (var recipe in content.Recipes)
            {
                var tmp = _mapper.Map<RecipeViewDto>(recipe);
                var msg = $"<strong>Title: {tmp.Title}, Vegetarian: {tmp.Vegetarian}, GlutenFree: {tmp.GlutenFree}, PricePerServing: {tmp.PricePerServing}, Link: {tmp.SpoonacularSourceUrl} Save recipe(/AddRecipeToUser_{tmp.Id})</strong>";
                response.AppendLine(msg);
                msgResponse += msg;
                recipesIds.Add(recipe.Id.ToString());
                // response.AppendLine("das");
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
