using System.Data.SqlTypes;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;

Console.WriteLine("Welcome to Weather Station");
Console.WriteLine("This is a simple console application to consume weather data from an API.");
Console.WriteLine("#########################################");
Console.WriteLine();
Console.WriteLine("1-Get List of Cities:");
Console.WriteLine("2-Add New City:");
Console.WriteLine("3-Delete City:");
Console.WriteLine("4-Update City:");
Console.WriteLine("5- Get City By ID:");
Console.WriteLine();


string number;
Console.Write("Please enter your choice: ");
number = Console.ReadLine();

if(number == "1")
{
    Console.WriteLine("List of Cities:");
    string url = "https://localhost:7230/api/Weathers";
    using(HttpClient client = new HttpClient())
    {
        HttpResponseMessage response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        JArray jArray = JArray.Parse(responseBody);
        foreach (var item in jArray)
        {
            string cityName = item["cityName"].ToString();
            Console.WriteLine($"City: {cityName}");


        }

        //if (response.IsSuccessStatusCode)
        //{
        //    var cities = response.Content.ReadAsStringAsync().Result;
        //    Console.WriteLine(cities);
        //}
        //else
        //{
        //    Console.WriteLine("Error fetching data from API.");
        //}
    }
}

if (number == "2")
{
    string url = "https://localhost:7230/api/Weathers";

    string newCity = Console.ReadLine();
    decimal newTemperature = decimal.Parse(Console.ReadLine());
    string newCountry = Console.ReadLine();
    string detail = Console.ReadLine();

    var cityData = new
    {
        CityName = newCity,
        Country = newCountry,
        Temperature = newTemperature,
        WeatherDescription = detail
    };

    using (HttpClient httpClient = new HttpClient())
    {
        string json = JsonConvert.SerializeObject(cityData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        Console.WriteLine("City added successfully.");

    }
}

if (number == "3")
{
    string url = "https://localhost:7230/api/Weathers";
    int id = int.Parse(Console.ReadLine());

    using(HttpClient httpClient = new HttpClient())
    {
        HttpResponseMessage response = await httpClient.DeleteAsync($"{url}/{id}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("City deleted successfully.");
        }
        else
        {
            Console.WriteLine("Error deleting city.");
        }
    }


}

if (number == "4")
{
    string url = "https://localhost:7230/api/Weathers";
    int id = int.Parse(Console.ReadLine());
    string newCity = Console.ReadLine();
    string newCountry = Console.ReadLine();
    decimal newTemperature = decimal.Parse(Console.ReadLine());
    string detail = Console.ReadLine();
    var cityData = new
    {
        CityName = newCity,
        Country = newCountry,
        Temperature = newTemperature,
        WeatherDescription = detail
    };
    using (HttpClient httpClient = new HttpClient())
    {
        string json = JsonConvert.SerializeObject(cityData);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await httpClient.PutAsync($"{url}/{id}", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("City updated successfully.");
        }
        else
        {
            Console.WriteLine("Error updating city.");
        }
    }
}

if (number == "5")
{
    string url = "https://localhost:7230/api/Weathers";
    int id = int.Parse(Console.ReadLine());
    string newCity = Console.ReadLine();
    string newCountry = Console.ReadLine();
    decimal newTemperature = decimal.Parse(Console.ReadLine());
    string detail = Console.ReadLine();
    using (HttpClient httpClient = new HttpClient())
    {
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

else { Console.WriteLine("Invalid choice, please try again."); }
Console.WriteLine("#########################################");


Console.Read();