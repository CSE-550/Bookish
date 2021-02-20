using Bookish.Server.Controllers;
using Bookish.Shared;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Server.Tests
{
    public class Tests
    {
        [Test]
        public void ExampleTest()
        {
            WeatherForecastController controller = new WeatherForecastController(null);
            List<WeatherForecast> forecasts = controller.Get().ToList();
            Assert.AreEqual(5, forecasts.Count());
        }
    }
}