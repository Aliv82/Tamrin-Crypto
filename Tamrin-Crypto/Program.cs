
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;



await RunInBackground(TimeSpan.FromSeconds(5), () => InitAsync());

async Task RunInBackground(TimeSpan timeSpan, Action action)
{
    var periodicTimer = new PeriodicTimer(timeSpan);
    while (await periodicTimer.WaitForNextTickAsync())
    {
        action();
    }
}

async Task InitAsync()
{
    HttpClient client = new HttpClient();
    string API = "http://api.wallex.ir/v1/currencies/stats";

    HttpResponseMessage request = await client.GetAsync(API);

    if (request.IsSuccessStatusCode)
    {
        string response = await request.Content.ReadAsStringAsync();

        Allinformation in1 = JsonConvert.DeserializeObject<Allinformation>(response);

        List<Data> Dataitem = in1.result;

        foreach (var item in Dataitem)
        {
            Console.WriteLine($"key : {item.key}");
            Console.WriteLine($"name : {item.name_en}");
            Console.WriteLine($"rank : {item.rank}");
            Console.WriteLine($"price : {item.price}");

            // price chenges for a second = + 0.0000005
            double prediction = item.price + 0.0036;
            Console.WriteLine("price prediction for 2h later is : " + prediction);
            Console.WriteLine("please insert ENTER if you want to see another coin!!");
            Console.ReadKey();

        }

    }
}



public class Allinformation
{
    public List<Data> result { get; set; }
}

public class Data
{
    public string key { get; set; }
    public string name_en { get; set; }
    public int? rank { get; set; }
    public float price { get; set; }
}