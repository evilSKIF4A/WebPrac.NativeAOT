using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using WebPrac.NativeAOT.Api.Models;

namespace WebPrac.NativeAOT.Api.Endpoints;

public static class MinimalApis
{
    /// <summary>
    /// Add endpoint for get temperature Celsius
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static RouteGroupBuilder GetTemperatureCelsius(this RouteGroupBuilder builder)
    {
        builder.MapGet("celsius", Results<Ok<GetTemperatureResponse>, NotFound> () =>
        {
            var timestamp = Stopwatch.GetTimestamp();
            return TypedResults.Ok(new GetTemperatureResponse
            {
                Temperature = ((timestamp ^ (timestamp >> 32)) & 0xFFFF) % 80 - 30,
                Timestamp = DateTime.UtcNow
            });
        });

        return builder;
    }

    public static WebApplication AddTemperatureGroup(this WebApplication app)
    {
        var temperature = app.MapGroup("/temperature");

        temperature.GetTemperatureCelsius();
        
        return app;
    }
}

[JsonSerializable(typeof(GetTemperatureResponse))]
internal partial class AppJsonSerializerTemperaturesContext : JsonSerializerContext
{
}