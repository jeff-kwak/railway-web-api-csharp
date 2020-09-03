using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WeatherApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private static int Interpolate((int x, int y) p0, (int x, int y) p1, int x) => (p0.y * (p1.x - x) + p1.y * (x - p0.x)) / (p1.x - p0.x);

    private const int MinAllowedTemperature = -20;
    private const int MaxAllowedTemperature = 55;
    private WeatherForecast CalculateWeatherForecastForTemperature(int tempC)
    {
      if(tempC > MaxAllowedTemperature) return $"The temperature {tempC}C is too high to calculate a weather forecast. The max allowed temperature is {MaxAllowedTemperature}C";
      
      var summary = Summaries[Interpolate((MinAllowedTemperature, 0), (MaxAllowedTemperature, Summaries.Length - 1), tempC)];
      return new WeatherForecast
      {
        Date = DateTime.Now,
        Summary = summary,
        TemperatureC = tempC
      };
    }

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
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

    [HttpGet("{tempC}")]
    public WeatherForecast GetWeatherForecastByTemperature(int tempC)
    {
      return CalculateWeatherForecastForTemperature(tempC);
    }
  }
}
