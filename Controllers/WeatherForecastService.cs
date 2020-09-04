using System;

namespace WeatherApi
{
  public interface IWeatherForecastService
  {
    int MinAllowedTemperature { get; }
    int MaxAllowedTemperature { get; }
    Either<WeatherForecastError, WeatherForecast> CalculateWeatherForecastForTemperature(int tempC);
  }

  public class WeatherForecastService : IWeatherForecastService
  {
    private static readonly string[] Summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public int MinAllowedTemperature => -20;

    public int MaxAllowedTemperature => 55;

    private static int Interpolate((int x, int y) p0, (int x, int y) p1, int x) => (p0.y * (p1.x - x) + p1.y * (x - p0.x)) / (p1.x - p0.x);

    // private const int MinAllowedTemperature = -20;
    // private const int MaxAllowedTemperature = 55;
    public Either<WeatherForecastError, WeatherForecast> CalculateWeatherForecastForTemperature(int tempC)
    {
      if (tempC > MaxAllowedTemperature) return new TemperatureTooHigh(tempC, MaxAllowedTemperature);
      if (tempC < MinAllowedTemperature) return new TemperatureTooLow(tempC, MinAllowedTemperature);

      var summary = Summaries[Interpolate((MinAllowedTemperature, 0), (MaxAllowedTemperature, Summaries.Length - 1), tempC)];
      return new WeatherForecast
      {
        Date = DateTime.Now,
        Summary = summary,
        TemperatureC = tempC
      };
    }
  }
}