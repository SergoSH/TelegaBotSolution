using Telegram.Bot.Polling;
using TelegaBotCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        
        TelegaBotClient.startReceivingMessages(
            receiverOptions,
            cancellationToken
        );
        Console.ReadLine();
    }
}

