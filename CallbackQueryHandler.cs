using RockPaperScissorsBot.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RockPaperScissorsBot;

public class CallbackQueryHandler : IBotUpdateHandler
{
    private readonly TelegramBotClient _botClient;

    public CallbackQueryHandler(TelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var chatId = update.CallbackQuery.Message.Chat.Id;
        var data = update.CallbackQuery.Data;
        var botHandePosition = BotHandePosition();
        
        var gameEndKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("повторить", "повторить"),
                InlineKeyboardButton.WithCallbackData("завершить", "завершить"),
            }
        });
        
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("камень", "камень"),
                InlineKeyboardButton.WithCallbackData("бумага", "бумага"),
                InlineKeyboardButton.WithCallbackData("ножницы", "ножницы")
            }
        });

        switch (data)
        {
            case "повторить":
                await SendMessageAsync(chatId, BotCommandsMessage.Game, inlineKeyboard, cancellationToken);
                return;
            case "завершить":
                await SendMessageAsync(chatId, "Спасибо за игру!", cancellationToken);
                return;
        }
        
        await SendMessageAsync(chatId,
            $"Бот: {botHandePosition}, игрок: {data}, результат: {CompareHands(botHandePosition, data)}"
            , cancellationToken);
        
        await SendMessageAsync(chatId, "Желаете сыграть снова?", gameEndKeyboard, cancellationToken);
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

    private string BotHandePosition()
    {
        var handePosition = string.Empty;
        var randomNumber = new Random();
        var positionNumber = randomNumber.Next(1, 4);

        handePosition = positionNumber switch
        {
            1 => "камень",
            2 => "ножницы",
            3 => "бумага",
            _ => handePosition
        };

        return handePosition;
    } 
    
    public static string CompareHands(string botPosition, string playerPosition)
        => (botPosition, playerPosition) switch
        {
            ("камень", "бумага") => "камень покрыт бумагой. Бумага побеждает.",
            ("камень", "ножницы") => "камень ломает ножницы. Камень побеждает.",
            ("бумага", "камень") => "бумага покрывает камень. Выигрывает бумага.",
            ("бумага", "ножницы") => "бумага разрезается ножницами. Ножницы побеждают.",
            ("ножницы", "камень") => "ножницы разбиты камнем. Камень побеждает.",
            ("ножницы", "бумага") => "ножницы режут бумагу. Ножницы выигрывают.",
            (_, _) => " ничья"
        };
}