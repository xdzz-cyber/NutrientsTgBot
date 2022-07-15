using System.Reflection;
using Application;
using Application.Common.Mappings;
using Application.Interfaces;
using Persistence;
using TelegramBotUI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(ITelegramBotDbContext).Assembly));
});

builder.Services.AddHttpClient();
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
   
        try
        {
            var scope = app.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<TelegramBotDbContext>();
            var cts = new CancellationTokenSource();
            DbInitializer.Initialize(ctx);
            await new DbSeed(new HttpClient(), ctx).Seed(cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
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
