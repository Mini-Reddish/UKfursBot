﻿using Discord;
using Discord.WebSocket;
using UKFursBot.Commands;
using UKFursBot.Context;
using UKFursBot.Entities;

namespace UKFursBot.Features.UserNotes;
public class AddUserNoteCommand(
    UKFursBotDbContext dbContext,
    DiscordSocketClient client,
    SocketMessageChannelManager socketMessageChannelManager)
    : BaseCommand<AddUserNoteCommandParameters>(socketMessageChannelManager)
{
    public override string CommandName => "add_note";
    public override string CommandDescription => "Add a note to the specified user";

    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, AddUserNoteCommandParameters commandParameters)
    {
        var settings = dbContext.BotConfigurations.FirstOrDefault();
        if (settings == null) 
            return;
        
        if (settings.ModerationLoggingChannel == 0)
        {
            await FollowupAsync("WARNING: The logging channel has not been assigned.  Please assign one using the /admin_set_channel_for ModerationLog command");
            return;
        }
        
        var userNote = new UserNote()
        {
            UserId = commandParameters.User.Id,
            ModeratorId = socketSlashCommand.User.Id,
            Reason = commandParameters.Note,
            DateAdded = DateTime.UtcNow,
        };
        
        dbContext.UserNotes.Add(userNote);

        var response = new RichTextBuilder()
            .AddHeading2("User Note Added")
            .AddText(commandParameters.Note)
            .AddHeading3("Moderator")
            .AddText(socketSlashCommand.User.Username);

        var embed = new EmbedBuilder()
        {
            Color = Color.Orange,
            Description = response.Build()
        }.Build();
        
        var channel = await client.GetChannelAsync(settings.ModerationLoggingChannel);
        if (channel is SocketTextChannel textChannel)
        {
            await textChannel.SendMessageAsync(embed: embed);
        }
        await FollowupAsync("Note added");
    }
}

public class AddUserNoteCommandParameters
{
    [CommandParameterRequired]
    public required SocketGuildUser User { get; set; }
    
    [CommandParameterRequired]
    public required string Note { get; set; }
}