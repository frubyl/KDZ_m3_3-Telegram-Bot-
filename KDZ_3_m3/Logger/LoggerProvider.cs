using Microsoft.Extensions.Logging;

namespace Logger
{
    public class LoggerProvider : ILoggerProvider
    {
        private readonly StreamWriter _logWriter;

        public ILogger CreateLogger(string categoryName) => new Logger(categoryName, _logWriter);

        public void Dispose() => _logWriter.Dispose();

        public LoggerProvider(StreamWriter logWriter)
        {
            _logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }
        public LoggerProvider() { }

    }
}