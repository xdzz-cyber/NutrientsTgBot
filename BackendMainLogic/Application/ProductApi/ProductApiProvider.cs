using System.Text.Json;
using Application.ProductApi.DataTypes;
using Microsoft.Extensions.Configuration;
using FileProvider;

namespace Application.ProductApi
{
    public class ProductApiProvider
    {
        public static async Task<IEnumerable<FoodItem>> GetFoodsListAsync()
        {
            var configManager = new ConfigurationBuilder().AddJsonFile("config.json").Build();
            var filename = configManager.GetSection("FoodsFileName").Value;
            var fileManager = new FileSystemProvider();
            if (fileManager.Exists(filename))
            {
                return JsonSerializer.Deserialize<IEnumerable<FoodItem>>(fileManager.Read(filename))!;
            }
            var endpoint = configManager.GetSection("Endpoint").Value;
            var httpClient = new HttpClient();
            var apiResponseMessage = await httpClient.GetAsync(endpoint);
            var jsonResult = await apiResponseMessage.Content.ReadAsStreamAsync();
            var unformattedList = await JsonSerializer.DeserializeAsync<List<AbridgedFoodApiItem>>(jsonResult);
            var formattedList = unformattedList!.Select(x => new FoodItem()
            {
                Amount = 100,
                Name = x.Description,
                Unit = "Grams",
                Nutrients = x.Nutrients.Where(n => n.Name
                        is "Total lipid (fat)"
                        or "Protein"
                        or "Carbohydrate, by summation"
                        or "Carbohydrate, by difference")
                    .Select(n => new FoodNutrient()
                    {
                        Amount = n.Amount,
                        Name = n.Name,
                        UnitName = n.UnitName
                    })
            });
            var foodsListAsync = formattedList.ToList();
            _ = fileManager.WriteAsync(filename, new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(foodsListAsync)));
            return foodsListAsync;
        }
    }
}
