using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

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
        private readonly WeatherDbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherDbContext context)
        {
            _logger = logger;
            _context = context;
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

        [HttpPost("runsql")]
        public IActionResult RunSql([FromBody] string query)
        {
            var result = _context.Forecasts.FromSqlRaw(query);
            return Ok(result);
        }

        [HttpGet("password")]
        public IActionResult GetPassword()
        {
            const string password = "P@ssw0rd";
            var encryptedPassword = SHA1.HashData(Encoding.UTF8.GetBytes(password));

            return Ok(encryptedPassword);
        }

        [HttpGet("regex")]
        public IActionResult DoRegex([FromQuery] string pattern)
        {
            var match = Regex.Match("abc", pattern);

            return Ok(match.Success);
        }
    }
}
