using Microsoft.Extensions.Logging;

namespace Logger
{

    public class Logger : ILogger
    {
        private readonly string _categoryName;
        private readonly StreamWriter _logWriter;
        public void Log<TState>(LogLevel logLevel,EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Записываем только логи инф. уровня или выше.
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var message = formatter(state, exception);
            // Записываем информацию в файл.
            _logWriter.WriteLine($"[{logLevel}] [{_categoryName}] {message}");
            _logWriter.Flush();
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public Logger(string categoryName, StreamWriter logFileWriter)
        {
            _categoryName = categoryName;
            _logWriter = logFileWriter;
        }
        public Logger() { }
    }
}