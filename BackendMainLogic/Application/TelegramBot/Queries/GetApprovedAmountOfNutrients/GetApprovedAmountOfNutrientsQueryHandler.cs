using Application.Common.Constants;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TelegramBot.Queries.GetApprovedAmountOfNutrients;

public class GetApprovedAmountOfNutrientsQueryHandler : IRequestHandler<GetApprovedAmountOfNutrientsQuery, string>
{
    private readonly UserManager<AppUser> _userManager;

    public GetApprovedAmountOfNutrientsQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> Handle(GetApprovedAmountOfNutrientsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user.Weight <= 0 || user.Age <= 0 || user.Height <= 0 || (user.Sex.ToLower() != "man" && user.Sex.ToLower() 
                != "women"))
        {
            return "Please, set values of weight, sex, age and height before getting data.";
        }

        var recommendedAmountOfCalories = user.Sex.Equals("Male")
            ? TelegramBotRecommendedAmountOfNutrients.WeightCoefficientInBmr * user.Weight 
            + TelegramBotRecommendedAmountOfNutrients.HeightCoefficientInBmr * user.Height 
            - TelegramBotRecommendedAmountOfNutrients.AgeCoefficientInBmr * user.Age + 5
            : TelegramBotRecommendedAmountOfNutrients.WeightCoefficientInBmr * user.Weight 
            + TelegramBotRecommendedAmountOfNutrients.HeightCoefficientInBmr * user.Height 
            - TelegramBotRecommendedAmountOfNutrients.AgeCoefficientInBmr * user.Age - 161; // Mifflin-St Jeor Equation
        
        // All of the percentages below have been taken from USDA's Dietary Guidelines for Americans
        
        var percentageOfProtein = TelegramBotRecommendedAmountOfNutrients.PercentageOfProteinFromCalories; // 10% - 35%

        var percentageOfCarbohydrates = TelegramBotRecommendedAmountOfNutrients.PercentageOfCarbohydratesFromCalories; // 45 - 65 %
        
        var percentageOfFat = TelegramBotRecommendedAmountOfNutrients.PercentageOfFatFromCalories;

        return $"Minimum needed amount of calories is {recommendedAmountOfCalories} kcal, " +
               $"protein({Math.Round(percentageOfProtein,2)}%) = {Math.Floor(recommendedAmountOfCalories * percentageOfProtein)} g, " +
               $"carbohydrates({Math.Round(percentageOfCarbohydrates,2)}%) = " +
               $"{Math.Floor(recommendedAmountOfCalories * percentageOfCarbohydrates)} g, " +
               $"fat({Math.Round(percentageOfFat,2)}%) = {Math.Floor(recommendedAmountOfCalories * percentageOfFat)} g."; 
        // basal metabolic rate = minimum amount of energy so body functions normally 
    }
}
