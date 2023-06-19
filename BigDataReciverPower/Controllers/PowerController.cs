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
        [HttpGet("Json/{name}")]
        public IActionResult Get(string name)
        {
            return Ok( _power[name]);
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
                createWithCntext(power2, power.Amount);
            }
            //_power.TryAdd(DateTime.Now, power);
            
            return Ok(_power);
        }

        // PUT api/<PowerController>/5
        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] Power power,int amount,int seed)
        {
            if (!_power.ContainsKey(name)) return NotFound(name + " is not a valid entry");
            _power.Remove(name);
            Power temp = new(seed)
            {
                House = name,
                Time = DateTime.Now,
                Workat = power.Workat,
                Homeat = power.Homeat,
                Sleepat = power.Sleepat,
                UpAt = power.UpAt
            };
            createWithCntext(temp, amount);
            return Ok(_power[name]);
        }

        // DELETE api/<PowerController>/5
        [HttpDelete("name/{name}")]
        public void Delete(string name)
        {
            _power.Remove(name);
        }

        [HttpDelete("Clear")]
        public void Clear()
        {
            _power.Clear();
        }

        [NonAction]
        public void createWithCntext(Power power ,int amount)
        {
            _power.Add(power.House, new List<Power>());
            for (int i = 0; i < amount; i++)
            {
                power.Update();
                _power[power.House].Add(new Power() { Usage = power.Usage, House = power.House, Time = new DateTime(power.Time.Year, power.Time.Month, power.Time.Day).AddHours(power.Time.Hour) });
            }
        }
    }
}
