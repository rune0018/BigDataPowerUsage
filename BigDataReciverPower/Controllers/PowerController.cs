using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BigDataReciverPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerController : ControllerBase
    {
        private static List< Power> _power = new();
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
                    if (_power[i] is not null)
                        metricResult += $"power_usage{"{"}label=\"{_power[i].Town}\",Time=\"{_power[i].Time}\"{"}"} {_power[i].Usage}\n";
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

        // GET api/<PowerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PowerController>
        [HttpPost]
        public IActionResult Post(Power power)
        {
            lock (_power) {
                _power.Add(power);
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
