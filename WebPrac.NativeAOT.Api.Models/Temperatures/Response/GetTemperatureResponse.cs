namespace WebPrac.NativeAOT.Api.Models;

public class GetTemperatureResponse
{
    /// <summary>
    /// Значение температуры в градусах Цельсия
    /// </summary>
    public required double Temperature { get; init; }

    /// <summary>
    /// Временная метка измерения температуры (UTC)
    /// </summary>
    public required DateTime Timestamp { get; init; }

    /// <summary>
    /// Единица измерения (default °C)
    /// </summary>
    public string Unit { get; init; } = "°C";
}