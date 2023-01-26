using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.TelegramBot.Queries.GetAvailableRecipeFilters;

public class GetAvailableRecipeFiltersQueryHandler : IRequestHandler<GetAvailableRecipeFiltersQuery, string>
{
    private readonly ITelegramBotDbContext _telegramBotDbContext;
    
    public GetAvailableRecipeFiltersQueryHandler(ITelegramBotDbContext telegramBotDbContext)
    {
        _telegramBotDbContext = telegramBotDbContext;
    }
    
    public async Task<string> Handle(GetAvailableRecipeFiltersQuery request, CancellationToken cancellationToken)
    {
        return string.Join(",", await _telegramBotDbContext.RecipeFilters.Select(recipe => recipe.Name)
            .ToListAsync(cancellationToken));
    }
}
