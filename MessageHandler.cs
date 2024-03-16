using RockPaperScissorsBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RockPaperScissorsBot;

public class MessageHandler : IBotUpdateHandler
{
    private readonly TelegramBotClient _botClient;

    public MessageHandler(TelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message!.Chat.Id;
        var message = update.Message.Text ?? string.Empty;
        Console.WriteLine($"Получено сообщение {message} в чате {chatId}");
        
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("камень", "камень"),
                InlineKeyboardButton.WithCallbackData("бумага", "бумага"),
                InlineKeyboardButton.WithCallbackData("ножницы", "ножницы")
            }
        });

        switch (message)
        {
            case BotCommands.Start:
                await SendMessageAsync(chatId, BotCommandsMessage.Start, cancellationToken);
                break;
            case BotCommands.Help:
                await SendMessageAsync(chatId, BotCommandsMessage.Help, cancellationToken);
                break;
            case BotCommands.Game:
                await SendMessageAsync(chatId, BotCommandsMessage.Game, inlineKeyboard, cancellationToken);
                break;
            default:
                await SendMessageAsync(chatId, BotCommandsMessage.DefaultMessage, cancellationToken);
                break;
        }

    }

    private Task SendMessageAsync(
        long chatId, 
        string message, 
        CancellationToken cancellationToken)
    {
        return _botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: message, 
            cancellationToken: cancellationToken);
    }
    
    private Task SendMessageAsync(
        long chatId, 
        string message, 
        IReplyMarkup markup,
        CancellationToken cancellationToken)
    {
       return _botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: message, 
            replyMarkup: markup,
            cancellationToken: cancellationToken);
    }
}