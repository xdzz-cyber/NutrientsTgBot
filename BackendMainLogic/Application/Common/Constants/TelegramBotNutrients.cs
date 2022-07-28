namespace Application.Common.Constants;

public static class TelegramBotNutrients
{
    public static readonly List<string> Nutrients;

    static TelegramBotNutrients() => Nutrients = new List<string>(){"Fat", "Carbohydrates", "Protein", "Calories"};

}
