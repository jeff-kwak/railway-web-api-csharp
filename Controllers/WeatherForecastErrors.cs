namespace WeatherApi
{
  public class WeatherForecastError
  {
    public string Message { get; private set; }
    protected WeatherForecastError(string message)
    {
      Message = message;
    }
  }

  public class TemperatureTooHigh : WeatherForecastError
  {
    public TemperatureTooHigh(int actual, int max) :
      base($"The actual temperature {actual}C is higher than the allowed max of {max}C.")
    {

    }
  }

  public class TemperatureTooLow : WeatherForecastError
  {
    public TemperatureTooLow(int actual, int min) :
      base($"The actual temperature {actual}C is lower than the allowed min of {min}C.")
    {

    }
  }
}