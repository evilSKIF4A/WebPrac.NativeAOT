using System.Diagnostics;
using WebPrac.NativeAOT.Api.Endpoints;
using WebPrac.NativeAOT.Application;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerTemperaturesContext.Default);
});

builder.Services.AddLogging(config =>
{
    config.ClearProviders();

    config.AddConsoleFormatter<LogFormatter, LogFormatterOptions>();
    
    config.Services.Configure<LogFormatterOptions>(options =>
    {
        options.Template = "{Timestamp} [{Level}] {Category}: {Message}";
    });

    config.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Minimal APIs
app.AddTemperatureGroup();

var logger = app.Services.GetRequiredService<ILogger<Program>>();


app.Use(async (_, next) =>
{
    logger.LogDebug("Starting web host");
    var time = Stopwatch.StartNew();
    await next();
    time.Stop();
    Console.WriteLine($"Execution time: {time.ElapsedMilliseconds} ms");
});

app.Run();