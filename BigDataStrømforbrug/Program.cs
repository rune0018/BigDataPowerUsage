// See https://aka.ms/new-console-template for more information
using BigDataStrømforbrug;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

string endpoint = Environment.GetEnvironmentVariable("Endopint") ?? "https://localhost:7215";

Power power = new() {Town = Environment.GetEnvironmentVariable("Town") ?? Guid.NewGuid().ToString() };
HttpClient client = new()
{
    Timeout = TimeSpan.FromMilliseconds(500)
};
while (true)
{
    //reuses the created threads based on the pc running it
    //
    //output Time can be unreliable due to this
    ThreadPool.QueueUserWorkItem(CheckOutputOfPower,power ?? new() { Town="Null",Usage=1000});
    Thread.Sleep(100);
}
//thread pool helper to make sure that the "State(in this case "power")"(class) is preserved
void CheckOutputOfPower(object? power)
{
    
    Power power1 = (Power?)power ?? new() { Town = "Null", Usage = 1000 };
    power1.Update();
    Console.WriteLine(power1);
    StringContent data = new(JsonSerializer.Serialize(power1), Encoding.UTF8, "application/json");
    try
    {
        HttpResponseMessage aMessege = client.PostAsync(endpoint + "/api/Power", data).Result;
        Console.WriteLine(aMessege.Content.ReadAsStringAsync().Result);
        Console.WriteLine(aMessege.StatusCode);
    }
    catch (Exception e)
    {
        Console.WriteLine("Look at http error:\n " + e.Message);
    }
}