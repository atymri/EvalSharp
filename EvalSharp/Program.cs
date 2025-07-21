using System;
using EvalSharp.Bot;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string token = "YOUR_BOT_TOKEN_HERE";
        
        var bot = new Bot(token);
        await bot.StartAsync();

        Console.WriteLine("Bot started. Press Enter to exit.");
        Console.ReadLine();

        await bot.StopAsync();
    }
}
