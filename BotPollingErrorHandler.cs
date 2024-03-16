using Telegram.Bot.Exceptions;

namespace RockPaperScissorsBot;

public class BotPollingErrorHandler
{
    public Task HandlePollingErrorAsync(Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException 
                => $"Telegram Bot API Error {apiRequestException.ErrorCode}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        
        return Task.CompletedTask;
    }
}