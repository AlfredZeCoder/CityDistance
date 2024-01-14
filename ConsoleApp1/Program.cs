using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Welcome to distance checker!");
        
        Console.WriteLine("Enter the name of the first city :");
        City firstCity = await GetCoord(
            new(Console.ReadLine()!, 0, 0)
            );

        Console.WriteLine("Enter the name of the second city :");
        City secondCity = await GetCoord(
            new(Console.ReadLine()!, 0, 0)
            );


        double distance = CalculDistance(firstCity, secondCity);

        Console.WriteLine($"The distance is {distance} km.");

        return;
    }

    public async static Task<City> GetCoord(City city)
    {
        string apiKey = "AIzaSyBGjuDVC0d9d93YINy6l1iXcvlQN4IMhfg";
        string apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(city.Name)}&key={apiKey}";

        
        HttpClient client = new();

        JObject resultObject = JObject.Parse(await client.GetStringAsync(apiUrl));
        JToken location = resultObject.SelectToken("results[0].geometry.location")
            ?? throw new Exception("Api did not passed.");

        double lat = (double)location.SelectToken("lat")!;
        double lng = (double)location.SelectToken("lng")!;
        
        return new City(city.Name, lat, lng);

    }

    public static double CalculDistance(City firstCity, City secondCity)
    {
        int R = 6371;

        double dLat = Deg2Rad(secondCity.Lat - firstCity.Lat);
        double dLon = Deg2Rad(secondCity.Lat - firstCity.Lat);
        double a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(Deg2Rad(firstCity.Lat)) * Math.Cos(Deg2Rad(secondCity.Lat)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double d = R * c;

        return d;
    }

    public static double Deg2Rad(double deg)
    {
        return deg * (Math.PI / 180);
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