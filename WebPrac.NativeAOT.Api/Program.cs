using System.Diagnostics;
using WebPrac.NativeAOT.Api.Endpoints;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerTemperaturesContext.Default);
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Minimal APIs
app.AddTemperatureGroup();

app.Use(async (_, next) =>
{
    var time = Stopwatch.StartNew();
    await next();
    time.Stop();
    Console.WriteLine($"Execution time: {time.ElapsedMilliseconds} ms");
});

app.Run();