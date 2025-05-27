using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to Weather Station");
        Console.WriteLine("This is a simple console application to consume weather data from an API.");
        Console.WriteLine("#########################################\n");
        Console.WriteLine("1- Get List of Cities");
        Console.WriteLine("2- Add New City");
        Console.WriteLine("3- Delete City");
        Console.WriteLine("4- Update City");
        Console.WriteLine("5- Get City By ID\n");

        Console.Write("Please enter your choice: ");
        string choice = Console.ReadLine();
        string url = "https://localhost:7230/api/Weathers";

        switch (choice)
        {
            case "1":
                await GetCities(url);
                break;

            case "2":
                await AddCity(url);
                break;

            case "3":
                await DeleteCity(url);
                break;

            case "4":
                await UpdateCity(url);
                break;

            case "5":
                await GetCityById(url);
                break;

            default:
                Console.WriteLine("Invalid choice, please try again.");
                break;
        }

        Console.WriteLine("#########################################");
        Console.ReadLine();
    }

    static async Task GetCities(string url)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = null;

        // Simple retry logic (max 3 attempts)
        for (int attempt = 1; attempt <= 3; attempt++)
        {
            response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
                break;

            Console.WriteLine($"Attempt {attempt} failed. Retrying...");
            await Task.Delay(1000);
        }

        if (response != null && response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray jArray = JArray.Parse(responseBody);
            foreach (var item in jArray)
            {
                string cityName = item["cityName"]?.ToString();
                Console.WriteLine($"City: {cityName}");
            }
        }
        else
        {
            Console.WriteLine("Failed to fetch cities from API.");
        }
    }

    static async Task AddCity(string url)
    {
        Console.Write("Enter City Name: ");
        string newCity = Console.ReadLine();

        Console.Write("Enter Temperature: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newTemperature))
        {
            Console.WriteLine("Invalid temperature.");
            return;
        }

        Console.Write("Enter Country: ");
        string newCountry = Console.ReadLine();

        Console.Write("Enter Weather Description: ");
        string detail = Console.ReadLine();

        var cityData = new
        {
            CityName = newCity,
            Country = newCountry,
            Temperature = newTemperature,
            WeatherDescription = detail
        };

        using HttpClient httpClient = new HttpClient();
        string json = JsonConvert.SerializeObject(cityData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
            Console.WriteLine("City added successfully.");
        else
            Console.WriteLine("Failed to add city.");
    }

    static async Task DeleteCity(string url)
    {
        Console.Write("Enter City ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        using HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.DeleteAsync($"{url}/{id}");

        if (response.IsSuccessStatusCode)
            Console.WriteLine("City deleted successfully.");
        else
            Console.WriteLine("Error deleting city.");
    }

    static async Task UpdateCity(string url)
    {
        Console.Write("Enter City ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        Console.Write("Enter New City Name: ");
        string newCity = Console.ReadLine();

        Console.Write("Enter New Country: ");
        string newCountry = Console.ReadLine();

        Console.Write("Enter New Temperature: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal newTemperature))
        {
            Console.WriteLine("Invalid temperature.");
            return;
        }

        Console.Write("Enter New Weather Description: ");
        string detail = Console.ReadLine();

        var cityData = new
        {
            CityName = newCity,
            Country = newCountry,
            Temperature = newTemperature,
            WeatherDescription = detail
        };

        using HttpClient httpClient = new HttpClient();
        string json = JsonConvert.SerializeObject(cityData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PutAsync($"{url}/{id}", content);
        if (response.IsSuccessStatusCode)
            Console.WriteLine("City updated successfully.");
        else
            Console.WriteLine("Error updating city.");
    }

    static async Task GetCityById(string url)
    {
        Console.Write("Enter City ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        using HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync($"{url}/{id}");

        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject cityData = JObject.Parse(responseBody);
            Console.WriteLine($"City: {cityData["cityName"]}, Country: {cityData["country"]}, Temperature: {cityData["temperature"]}, Description: {cityData["weatherDescription"]}");
        }
        else
        {
            Console.WriteLine("Error fetching city data.");
        }
    }
}
