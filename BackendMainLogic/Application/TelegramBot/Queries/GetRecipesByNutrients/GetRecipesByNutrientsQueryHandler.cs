using Application.Common.Constants;
using Application.Common.Mappings;
using Application.Interfaces;
using AutoMapper;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.TelegramBot.Queries.GetRecipesByNutrients;

public class GetRecipesByNutrientsQueryHandler : IRequestHandler<GetRecipesByNutrientsQuery, List<RecipeViewDto>>
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
    
    public async Task<List<RecipeViewDto>> Handle(GetRecipesByNutrientsQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _userManager.FindByNameAsync(request.Username);

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

        if (recipes.IsSuccessStatusCode)
        {
            var _ = await recipes.Content.ReadAsStringAsync(cancellationToken);

            var listOfRecipes = JsonSerializer.Deserialize<List<RecipeViewDto>>(_);

            var getRecipesInformationBulkRequestString = string
                .Format(TelegramBotRecipesHttpPaths.GetRecipesWithNutrition, string.Join(",", listOfRecipes.Select(r => r.Id)));

            var getRecipesInformationBulkHttpMessage =
                await _httpClient.GetAsync(getRecipesInformationBulkRequestString, cancellationToken);

            if (getRecipesInformationBulkHttpMessage.IsSuccessStatusCode)
            {
                var recipesInformationBulkResponse = JsonSerializer
                    .Deserialize<List<RecipeViewDto>>(
                        await getRecipesInformationBulkHttpMessage.Content.ReadAsStringAsync(cancellationToken));
                
                var recipesSpoonacularSourceUrlHttpMessageResponse = await _httpClient
                    .GetAsync($"{TelegramBotRecipesHttpPaths.GetFullRecipesData}{string.Join(',', recipesInformationBulkResponse!.Select(c => c.Id).ToList())}", 
                        cancellationToken: cancellationToken);

                var recipesSpoonacularSourceUrl = JsonSerializer.Deserialize<List<RecipeViewDto>>(await recipesSpoonacularSourceUrlHttpMessageResponse.Content.ReadAsStringAsync(cancellationToken));
                
                recipesInformationBulkResponse!.ForEach(r => r.SpoonacularSourceUrl = recipesSpoonacularSourceUrl!.First(rs => rs.Id == r.Id).SpoonacularSourceUrl);
                
                return recipesInformationBulkResponse!;
            }
        }
        return new List<RecipeViewDto>();
    }
}
