using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.Boop;

[CommandName("Boop")]
[CommandDescription("Boops you.  UwU")]
public class BoopUserCommand : BaseCommand<BoopUserCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private static DateTime _datetimeOfLastBoop = DateTime.MinValue;
    public BoopUserCommand(UKFursBotDbContext dbContext )
    {
        _dbContext = dbContext;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, BoopUserCommandParameters commandParameters)
    {
        var cooldown = _dbContext.BotConfigurations.First().MinutesThresholdBetweenBoops;
        if (cooldown == 0)
        {
            return;
        }
        
        if (DateTime.UtcNow < _datetimeOfLastBoop.AddMinutes(cooldown))
        {
            return;
        }
        await socketSlashCommand.Channel.SendMessageAsync($"*Gently boops <@{commandParameters.User.Id}> on the nose!*");
    }
}

public class BoopUserCommandParameters 
{
    [CommandParameterRequired]
    public SocketGuildUser User { get; set; }
}