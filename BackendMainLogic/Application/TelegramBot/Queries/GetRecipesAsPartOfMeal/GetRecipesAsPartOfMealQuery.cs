﻿using Application.Interfaces;
using Domain.TelegramBotEntities;
using MediatR;

namespace Application.TelegramBot.Queries.GetRecipesAsPartOfMeal;

public class GetRecipesAsPartOfMealQuery : IRequest<List<Recipe>>,IQuery
{
    public string Username { get; set; }
    
    public long ChatId { get; set; }

    public GetRecipesAsPartOfMealQuery(string username, long chatId = 0) => (Username, ChatId) = (username, chatId);
}
