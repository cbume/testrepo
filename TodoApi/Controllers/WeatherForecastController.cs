using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace TodoApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        /*
        // 🚨 VULNERABLE ENDPOINT: Reads file contents from an unsafe user-supplied path
        [HttpGet("readfile")]
        public IActionResult GetFileContents([FromQuery] string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return BadRequest("Path parameter is required.");
            }

            try
            {
                // 🚨 BAD: Directly reading user-supplied path
                string fileContents = System.IO.File.ReadAllText(path);
                return Ok(fileContents);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading file: {ex.Message}");
            }
        }
*/
    }
}
