using System;
using Microsoft.AspNetCore.Mvc;

namespace APIVersioning.Web.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController(){ }

        [HttpGet("{city}")]
        public IActionResult Get([FromRoute] string city)
        {
            var weatherInfo = new WeatherInfo
            {
                Name = city,
                CurrentTime = DateTime.UtcNow,
                Temperature = 2,
                Precipitation = 0.2,
                Humidity = 0.77,
                //Gist = "Cloudy with some rainfall in the evening."

            };

            return Ok(weatherInfo);
        }
    }

    internal class WeatherInfo
    {
        public string Name { get; set; }
        public DateTime CurrentTime { get; set; }
        public int Temperature { get; set; }
        public double Precipitation { get; set; }
        public double Humidity { get; set; }
        public string Gist { get; set; }
    }
}
