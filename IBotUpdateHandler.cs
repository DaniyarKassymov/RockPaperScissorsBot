using Telegram.Bot.Types;

namespace RockPaperScissorsBot;

public interface IBotUpdateHandler
{
    Task HandleUpdateAsync(Update update, CancellationToken cancellationToken);
}