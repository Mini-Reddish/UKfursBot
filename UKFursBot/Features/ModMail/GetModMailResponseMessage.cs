using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.ModMail;


[CommandName("get_modmail_response")]
[CommandDescription("Gets the response given to a user when they send in a modmail message")]
public class GetModMailResponseMessage  : BaseCommand<NoCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;

    public GetModMailResponseMessage(UKFursBotDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, NoCommandParameters commandParameters)
    {
        var botConfig = _dbContext.BotConfigurations.First();
        
        var content = new RichTextBuilder()
            .AddHeading1("Modmail received Response")
            .AddText(botConfig.ModMailResponseMessage);
        
        var embed = new EmbedBuilder()
        {
            Color = Color.Blue,
            Description = content.Build(),
        }.Build();

        await FollowupAsync(embed);
    }
}