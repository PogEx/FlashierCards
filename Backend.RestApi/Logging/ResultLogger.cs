using FluentResults;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace Backend.RestApi.Logging;

public class ResultLogger(ILogger logger): IResultLogger
{
    public void Log(string context, string content, ResultBase result, LogLevel logLevel)
    {
        if (logLevel == LogLevel.None) return;
        logger.Write(MapLogLevel(logLevel), $"Result: {result.Reasons.Select(reason => reason.Message)} {content} <{context}>");
    }

    public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
    {
        if (logLevel == LogLevel.None) return;
        logger.Write(MapLogLevel(logLevel), $"Result: {result.Reasons.Select(reason => reason.Message)} {content} <{typeof(TContext).FullName}>");
    }

    private static LogEventLevel MapLogLevel(LogLevel level)
    {
        return (LogEventLevel)level;
    }
}