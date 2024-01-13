using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to distance checker!");
        
        Console.WriteLine("Enter the name of the first city :");
        City firstCity = new(Console.ReadLine()!, 0 ,0);
        Console.WriteLine("Enter the name of the second city :");
        City secondCity = new(Console.ReadLine()!, 0, 0);


        var fReceived = GetCoord(firstCity);
        var sReceived = GetCoord(secondCity);

        Console.WriteLine(fReceived.Lat);
        Console.WriteLine(sReceived.Lat);
    }

    public async static Task<City> GetCoord(City city)
    {
        string apiKey = "AIzaSyBGjuDVC0d9d93YINy6l1iXcvlQN4IMhfg";
        string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(city.Name)}&key={apiKey}";

        double lat = 0;
        double lng = 0;
        
        try
        {
            string jsonResult;
            using (HttpClient client = new())
            {
                jsonResult = await client.GetStringAsync(apiUrl);
            }

            JObject resultObject = JObject.Parse(jsonResult);
            JToken location = resultObject.SelectToken("results[0].geometry.location")!;

            lat = (double)location.SelectToken("lat")!;
            lng = (double)location.SelectToken("lng")!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        City iCity = new(city.Name, lat, lng);

        return iCity;
    }
}

class City
{ 
    public string Name { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public City(string name, double lat, double lon)
    {
        Name = name;
        Lat = lat;
        Lon = lon;
    }
}