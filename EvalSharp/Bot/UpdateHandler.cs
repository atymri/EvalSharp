using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EvalSharp.Bot
{
    public class UpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly CodeExecution.CodeRunner _codeRunner;

        public UpdateHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
            _codeRunner = new CodeExecution.CodeRunner();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
                return;

            var message = update.Message;
            if (message?.Text == null)
                return;

            if (message.Text == "/start")
            {
                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "خوش اومدی, کد های سی شارپت رو با دستور /exec بفرست, این ربات واست اجراشون میکنه.",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
                return;
            }

            var textParts = message.Text.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (textParts.Length == 0)
                return;

            var commandWithMention = textParts[0]; // مثلا "/exec@BotName" یا "/exec"
            var commandPart = commandWithMention.Split('@')[0];
            const string commandPrefix = "/exec";

            if (!commandPart.Contains(commandPrefix))
            {
                await _botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "دستور نامعتبر است. برای اجرای کد از /exec استفاده کنید.",
                    replyToMessageId: message.MessageId,
                    cancellationToken: cancellationToken);
                return;
            }

            bool isOnlyCommand = textParts.Length == 1 || string.IsNullOrWhiteSpace(textParts[1]);

            if (isOnlyCommand)
            {

                if (message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "برای اجرای کد، بعد از دستور /exec کد C# خود را وارد کنید.\n\nمثال:\n/exec Console.WriteLine(\"سلام دنیا\");",
                        replyToMessageId: message.MessageId,
                        cancellationToken: cancellationToken);
                    return;
                }

                if (message.Chat.Type == ChatType.Private)
                {
                    await _botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "برای اجرای کد، بعد از دستور /exec کد C# خود را وارد کنید.\n\nمثال:\n/exec Console.WriteLine(\"سلام دنیا\");",
                        replyToMessageId: message.MessageId,
                        cancellationToken: cancellationToken);
                    return;
                }
            }

            string code = textParts.Length > 1 ? textParts[1].Trim() : "";

            if (code.StartsWith("```") && code.EndsWith("```"))
            {
                code = code[3..^3].Trim();
            }

            string response;
            var sw = Stopwatch.StartNew();
            try
            {
                response = await _codeRunner.ExecuteCSharpCodeAsync(code);
            }
            catch (Exception ex)
            {
                response = $"خطا در اجرای کد: {ex.Message}";
            }
            sw.Stop();

            string escapedResponse = EscapeMarkdownV2(response);
            string elapsed = sw.ElapsedMilliseconds < 1000
                ? $"{sw.ElapsedMilliseconds} میلی‌ثانیه"
                : $"{sw.Elapsed.TotalSeconds:F2} ثانیه";

            string finalMessage = $"```\n{escapedResponse}\n```\n⏱️ مدت زمان اجرا: {elapsed}";

            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: finalMessage,
                parseMode: ParseMode.MarkdownV2,
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken);
        }

        private string EscapeMarkdownV2(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var escapeChars = new[] { "_", "*", "[", "]", "(", ")", "~", "`", ">", "#", "+", "-", "=", "|", "{", "}", ".", "!" };

            foreach (var ch in escapeChars)
                text = text.Replace(ch, "\\" + ch);

            return text;
        }
    }
}
