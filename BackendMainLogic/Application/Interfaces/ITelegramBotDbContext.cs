﻿using Domain.TelegramBotEntities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface ITelegramBotDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    
    // public DbSet<WaterLevelOfUser> WaterLevelOfUsers { get; set; }
    //
    // public DbSet<AppUser> AppUsers { get; set; }
}
