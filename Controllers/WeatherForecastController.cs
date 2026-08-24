using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using CSharpPromptSnippets.Localization;

namespace CSharpPromptSnippets.Controllers
{
    /// <summary>
    /// Controller for handling weather forecast-related requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly string[] Summaries;

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging information.</param>
        /// <param name="localizer">The string localizer for accessing localized resources.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _localizer = localizer;

            // Initialize summaries from localized resources
            Summaries = new[]
            {
                _localizer["Weather_Freezing"].Value,
                _localizer["Weather_Bracing"].Value,
                _localizer["Weather_Chilly"].Value,
                _localizer["Weather_Cool"].Value,
                _localizer["Weather_Mild"].Value,
                _localizer["Weather_Warm"].Value,
                _localizer["Weather_Balmy"].Value,
                _localizer["Weather_Hot"].Value,
                _localizer["Weather_Sweltering"].Value,
                _localizer["Weather_Scorching"].Value
            };
        }

        /// <summary>
        /// Retrieves a collection of weather forecasts for the next five days. Each forecast includes the date, temperature in Celsius, and a summary of the weather conditions.
        /// Example method with cognitive complexity of 34, for prompt testing purposes.
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
                    summary = _localizer["Weather_Freezing"].Value;
                    if (date.Month == 12 || date.Month == 1)
                    {
                        summary += _localizer["Weather_Suffix_Winter"].Value;
                    }
                }
                else if (temperatureC < 10)
                {
                    summary = _localizer["Weather_Chilly"].Value;
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        summary += _localizer["Weather_Suffix_StartOfWeek"].Value;
                    }
                }
                else if (temperatureC > 30)
                {
                    summary = _localizer["Weather_Hot"].Value;
                    if (temperatureC > 40)
                    {
                        summary += _localizer["Weather_Suffix_ExtremeHeat"].Value;
                        if (date.DayOfWeek == DayOfWeek.Friday)
                        {
                            summary += _localizer["Weather_Suffix_WeekendIncoming"].Value;
                        }
                    }
                }

                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    summary += _localizer["Weather_Suffix_Weekend"].Value;
                }

                if (temperatureC > 40 && summary.Contains(_localizer["Weather_Hot"].Value))
                {
                    summary += _localizer["Weather_Suffix_StayHydrated"].Value;
                }

                if (temperatureC < -10 && date.Month == 1)
                {
                    summary += _localizer["Weather_Suffix_SevereCold"].Value;
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
                _logger.LogWarning(_localizer["Log_ExtremeTemperaturesDetected"]);
                foreach (var forecast in forecasts)
                {
                    if (forecast.TemperatureC > 50)
                    {
                        _logger.LogInformation(_localizer["Log_ExtremeTemperatureOn", forecast.Date, forecast.TemperatureC]);
                    }
                }
            }

            return forecasts;
        }
    }
}
