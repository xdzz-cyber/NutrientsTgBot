namespace Application.Common.Constants;

public static class TelegramBotRecommendedAmountOfNutrients
{
    public static readonly double PercentageOfProteinFromCalories = 22.5 * 0.01;
    
    public static readonly double PercentageOfCarbohydratesFromCalories = 55 * 0.01;
    
    public static readonly double PercentageOfFatFromCalories = 1 - PercentageOfCarbohydratesFromCalories 
                                                                + PercentageOfProteinFromCalories;
    
    public static readonly double WeightCoefficientInBmr = 10;
    
    public static readonly double HeightCoefficientInBmr = 6.25;
    
    public static readonly double AgeCoefficientInBmr = 5;
}
