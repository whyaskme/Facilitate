using Facilitate.Libraries.Models;
using MongoDB.Bson;

namespace AdminBlazor.Data {
    public class QuoteService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateOnly startDate) {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new WeatherForecast {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public Task<Quote[]> GetQuotesAsync()
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new Quote
            {
                _id = ObjectId.Empty.ToString()
            }).ToArray());
        }
    }
}