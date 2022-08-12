namespace Application.Common.Constants;

public static class TelegramBotRecipeCommandsNQueriesDataPatterns
{
    public static readonly string InputDataPatternForSingleId = "\\d+";
    
    public static readonly string InputDataPatternForRecipeFilterSingleId = "\\d";
    
    public static readonly string InputDataPatternForIds = "_All";
    
    public static readonly string IngredientsCommaSeparated = @"[a-z,A-Z,]";
}
