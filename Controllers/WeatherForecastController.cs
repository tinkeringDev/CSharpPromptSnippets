using Microsoft.AspNetCore.Mvc;

namespace CSharpPromptSnippets.Controllers
{
    /// <summary>
    /// Controller for handling weather forecast-related requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging information.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a collection of weather forecasts.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="WeatherForecast"/> objects.</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var forecasts = new List<WeatherForecast>();

            for (int i = 1; i <= 5; i++)
            {
                var date = DateOnly.FromDateTime(DateTime.Now.AddDays(i));
                var temperatureC = Random.Shared.Next(-20, 55);
                var summary = Summaries[Random.Shared.Next(Summaries.Length)];

                if (temperatureC < 0)
                {
                    summary = "Freezing";
                    if (date.Month == 12 || date.Month == 1)
                    {
                        summary += " - Winter";
                    }
                }
                else if (temperatureC < 10)
                {
                    summary = "Chilly";
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        summary += " - Start of the Week";
                    }
                }
                else if (temperatureC > 30)
                {
                    summary = "Hot";
                    if (temperatureC > 40)
                    {
                        summary += " - Extreme Heat";
                        if (date.DayOfWeek == DayOfWeek.Friday)
                        {
                            summary += " - Weekend Incoming";
                        }
                    }
                }

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    summary += " (Weekend)";
                }

                if (temperatureC > 40 && summary.Contains("Hot"))
                {
                    summary += " - Stay Hydrated";
                }

                if (temperatureC < -10 && date.Month == 1)
                {
                    summary += " - Severe Cold";
                }

                forecasts.Add(new WeatherForecast
                {
                    Date = date,
                    TemperatureC = temperatureC,
                    Summary = summary
                });
            }

            if (forecasts.Any(f => f.TemperatureC > 50))
            {
                _logger.LogWarning("Extreme temperatures detected in the forecast.");
                foreach (var forecast in forecasts)
                {
                    if (forecast.TemperatureC > 50)
                    {
                        _logger.LogInformation($"Extreme temperature on {forecast.Date}: {forecast.TemperatureC}C");
                    }
                }
            }

            return forecasts;
        }
    }
}
