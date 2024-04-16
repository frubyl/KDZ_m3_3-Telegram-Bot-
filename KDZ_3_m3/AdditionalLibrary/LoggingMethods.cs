using Microsoft.Extensions.Logging;
using Logger;

namespace AdditionalLibrary
{ 
public static class LoggingMethods
{
    public static void Log(string fileName, string message, LogLevel level)
    {
        string path = "../../../../log/log.txt"; 
            using (StreamWriter logWriter = new StreamWriter(path, append: true))
            {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                // Добавляем форматирование при выводе в консоль.
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
                // Добавляем провайдера логгирования.
                builder.AddProvider(new LoggerProvider(logWriter));
            });
            ILogger logger = loggerFactory.CreateLogger(fileName);
            using (logger.BeginScope(""))
            {
                switch (level)
                {
                    case LogLevel.Information:
                        logger.LogInformation("{time}: {message}", DateTime.Now, message);
                        break;
                    case LogLevel.Error:
                        logger.LogError("{time}: {message}", DateTime.Now, message);
                        break;
                };

            }
        }
    }
}
    }