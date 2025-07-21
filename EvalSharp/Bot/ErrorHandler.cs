using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace EvalSharp.Bot
{
    public class ErrorHandler
    {
        public Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
