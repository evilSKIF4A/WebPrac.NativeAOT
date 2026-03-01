using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace WebPrac.NativeAOT.Application;

public sealed class LogFormatter : ConsoleFormatter
{
    private readonly LogFormatterOptions _options;

    public LogFormatter(IOptions<LogFormatterOptions> options) : base(nameof(LogFormatter))
    {
        _options = options?.Value ?? new LogFormatterOptions();
    }

    public override void Write<TState>(in LogEntry<TState> logEntry, IExternalScopeProvider? scopeProvider, TextWriter textWriter)
    {
        if (textWriter == null) return;

        var timestamp = DateTime.UtcNow.ToString("o");
        var level = logEntry.LogLevel.ToString().ToUpperInvariant();
        var category = logEntry.Category ?? "";
        var message = logEntry.Formatter?.Invoke(logEntry.State, logEntry.Exception) ?? "";

        var sb = new StringBuilder();
        sb.Append(_options.Template
            .Replace("{Timestamp}", timestamp)
            .Replace("{Level}", level)
            .Replace("{Category}", category)
            .Replace("{Message}", message));

        if (logEntry.Exception != null)
        {
            sb.Append(" | EX: ").Append(logEntry.Exception);
        }

        textWriter.WriteLine(sb.ToString());
    }
}

public sealed class LogFormatterOptions : ConsoleFormatterOptions
{
    public string Template { get; set; } = "{Timestamp} [{Level}] {Category}: {Message}";
}