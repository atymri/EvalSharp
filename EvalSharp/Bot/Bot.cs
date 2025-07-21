using EvalSharp.Bot;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace EvalSharp.Bot
{
    public class Bot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CancellationTokenSource _cts;

        public Bot(string token)
        {
            _botClient = new TelegramBotClient(token);
            _cts = new CancellationTokenSource();
        }

        public async Task StartAsync()
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message }
            };

            _botClient.StartReceiving(
                new UpdateHandler(_botClient).HandleUpdateAsync,
                new ErrorHandler().HandleErrorAsync,
                receiverOptions,
                _cts.Token
            );

            var me = await _botClient.GetMeAsync();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Bot is ONLINE: {me.Username}");
            Console.ResetColor();

        }

        public Task StopAsync()
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }
}
