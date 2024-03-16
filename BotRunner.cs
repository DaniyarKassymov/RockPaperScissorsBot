using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RockPaperScissorsBot;

public class BotRunner
{
    private IBotUpdateHandler _updateHandler;
    private readonly BotPollingErrorHandler _errorHandler;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TelegramBotClient _botClient;
    
    public BotRunner(string token)
    {
        _botClient = new TelegramBotClient(token);
        _updateHandler = new MessageHandler(_botClient);
        _errorHandler = new BotPollingErrorHandler();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task Start()
    {
        var me = await _botClient.GetMeAsync();
        
        Console.WriteLine($"Бот запущен {me.Username}");
        
        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync, 
            cancellationToken: _cancellationTokenSource.Token);

        Console.ReadLine();
        _cancellationTokenSource.Cancel();
    }

    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                _updateHandler = new MessageHandler(_botClient);
                await _updateHandler.HandleUpdateAsync(update, cancellationToken);
                break;
            case UpdateType.CallbackQuery:
                _updateHandler = new CallbackQueryHandler(_botClient);
                await _updateHandler.HandleUpdateAsync(update, cancellationToken);
                break;
        }
    }

    private async Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        await _errorHandler.HandlePollingErrorAsync(exception, cancellationToken);
    }
}