using Microsoft.EntityFrameworkCore;

namespace TodoApi;

public class WeatherDbContext : DbContext
{
    public DbSet<WeatherForecast> Forecasts { get; set; }

    public WeatherDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
