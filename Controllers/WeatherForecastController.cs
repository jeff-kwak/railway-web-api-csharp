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
    private readonly IWeatherForecastService weatherService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IWeatherForecastService weatherService, ILogger<WeatherForecastController> logger)
    {
      this.weatherService = weatherService;
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      var rng = new Random();
      return Enumerable.Range(1, 5)
        .Select(_ => weatherService.CalculateWeatherForecastForTemperature(rng.Next(weatherService.MinAllowedTemperature, weatherService.MaxAllowedTemperature)))
        .Select(either => either.Match<WeatherForecast>(error => null, forecast => forecast))
        .Where(forecast => forecast != null)
        .Select((forecast, index) => new WeatherForecast { Date = forecast.Date.AddDays(index), TemperatureC = forecast.TemperatureC, Summary = forecast.Summary })
      .ToArray();
    }

    [HttpGet("{tempC}")]
    public ActionResult<WeatherForecast> GetWeatherForecastByTemperature(int tempC)
    {
      return weatherService.CalculateWeatherForecastForTemperature(tempC)
        .Match<ActionResult<WeatherForecast>>(
          error => error switch
          {
            TemperatureTooHigh tooHigh => BadRequest(tooHigh),
            TemperatureTooLow tooLow => BadRequest(tooLow),
            _ => Problem($"Unknown error trying to caculate a weather forecast for {tempC}C")
          },
          forecast => forecast
        );
    }
  }
}
