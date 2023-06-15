using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BigDataReciverPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private static Dictionary<string, List<Power>> _power = new();
        private object _powerLock = new object();
        // GET: api/<PowerController>
        [HttpGet]
        public IActionResult Get()
        {
            string metricResult = "# HELP power_usage A custom metric I want to expose \n# TYPE power_usage counter\n";
            //{"{"}label=\"{_power[i].Town}\"{"}"}
            lock (_power)
            {
                if(_power.Count < 0)
                    return Ok();
                for (int i = 0; i < _power.Count; i++)
                {
                    if (_power["Generated"][i] is not null)
                        metricResult += $"power_usage{"{"}label=\"{_power["Generated"][i].House}\",Time=\"{_power["Generated"][i].Time}\"{"}"} {_power["Generated"][i].Usage}\n";
                }
                //foreach (var power in _power.AsQueryable())
                //{
                //    if (power is not null)
                //        metricResult += $"power_usage{"{"}label=\"{power.Town}\"{"}"} {power.Usage}\n";
                //}

                Task.Run(() => _power.Clear());
            }

            return Content(metricResult, "text/plain; version=0.0.4");

        }

        [HttpGet("Json")]
        public IActionResult GetJson()
        {
            return Ok(_power);
        }
        // GET api/<PowerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("Generate/{amount}/seed/{seed}")]
        public IActionResult GenerateSeeded(int amount,int seed,int startworkat,int homeat,int bedtime,int upat)
        {
            var idk = new Power(seed,startworkat,homeat,bedtime,upat)
            {
                Time = DateTime.Now,
                House = "Generated"
            };
            _power.Add("Generated", new List<Power>());
            for (int i = 0; i < amount; i++)
            {
                idk.Update();
                _power["Generated"].Add(new Power() { Usage=idk.Usage,House=idk.House,Time= new DateTime(idk.Time.Year, idk.Time.Month, idk.Time.Day).AddHours(idk.Time.Hour) });
            }
            return Ok();
        }
        // POST api/<PowerController>
        [HttpPost("Generate")]
        public IActionResult Post(PowerConfig power)
        {
            foreach(Power power1 in power.Power) 
            {
                Power power2 = new Power(power.Seed)
                {
                    House = power1.House,
                    Time = DateTime.Now,
                    Workat = power1.Workat,
                    Homeat = power1.Homeat,
                    Sleepat = power1.Sleepat,
                    UpAt = power1.UpAt
                };
                _power.Add(power1.House, new List<Power>());
                for(int i = 0; i<power.Amount; i++)
                {
                    power2.Update();
                    _power[power1.House].Add(new Power() { Usage=power2.Usage,House=power2.House,Time=new DateTime(power2.Time.Year, power2.Time.Month, power2.Time.Day).AddHours(power2.Time.Hour) });
                }
            }
            //_power.TryAdd(DateTime.Now, power);
            
            return Ok(power);
        }

        // PUT api/<PowerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PowerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
