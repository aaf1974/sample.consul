using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sample.consul.api.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample.consul.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Config _options;
        private readonly IConsulClient _consulClient;
        private readonly IOptionsSnapshot<Config> _optionsSnapshot;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IOptions<Config> options,
            IOptionsSnapshot<Config> optionsSnapshot
            //IConsulClient consulClient
            )
        {
            _logger = logger;
            _options = options.Value;
            //this._consulClient = consulClient;
            _optionsSnapshot = optionsSnapshot;
        }

        [HttpGet]
        public string GetWinton()
        {
            return _options.Uri + " snapshot: " + _optionsSnapshot.Value.Uri;
        }

        [HttpGet("g")]
        public async Task<string> GetAsync([FromQuery] string key)
        {
            var str = string.Empty;
            //query the value  
            var res = await _consulClient.KV.Get(key);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //convert byte[] to string  
                str = System.Text.Encoding.UTF8.GetString(res.Response.Value);
            }

            return $"value-{str}";
        }

        [HttpGet(nameof(Get33))]
        public IEnumerable<WeatherForecast> Get33()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
