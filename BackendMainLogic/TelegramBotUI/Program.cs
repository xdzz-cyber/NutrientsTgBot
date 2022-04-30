using System.Reflection;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using Domain.TelegramBotEntities;
using Persistence;
using Telegram.Bot;
using TelegramBotUI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(ITelegramBotDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.RoutePrefix = string.Empty;
    config.SwaggerEndpoint("swagger/v1/swagger.json", "TelegramBotUI");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    using (var serviceProvider = builder.Services.BuildServiceProvider())
    {
        try
        {
            var ctx = serviceProvider.GetRequiredService<TelegramBotDbContext>();
            DbInitializer.Initialize(ctx);
        }
        catch (Exception e)
        {
        }
    
    }
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
