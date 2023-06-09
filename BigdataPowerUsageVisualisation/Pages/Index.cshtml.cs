using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BigdataPowerUsageVisualisation;
using Newtonsoft.Json;

namespace BigdataPowerUsageVisualisation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private static HttpClient HttpClient = new();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            if(HttpClient.BaseAddress is null)
            HttpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("JsonFromHere")?? "");
        }

        public void OnGet()
        {
            HttpResponseMessage datamessege = HttpClient.GetAsync("/api/Power/Json").Result;
            var messege = datamessege.Content.ReadAsStringAsync().Result;
            
            var sampledata = JsonConvert.DeserializeObject<Power[]>(messege);
            ViewData.Add("data", sampledata);
        }
    }
}