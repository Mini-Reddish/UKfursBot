using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using UKFursBot.Commands;
using UKFursBot.Context;

namespace UKFursBot.Features.Posts;

[CommandName("edit_post")]
[CommandDescription("Opens a modal to allow editing an existing post.")]
public class EditPostCommand : BaseCommand<EditPostCommandParameters>
{
    private readonly UKFursBotDbContext _dbContext;
    private readonly SocketMessageChannelManager _socketMessageChannelManager;
    protected override bool IsEphemeral => false;

    public EditPostCommand(UKFursBotDbContext dbContext,SocketMessageChannelManager socketMessageChannelManager)
    {       
        _dbContext = dbContext;
        _socketMessageChannelManager = socketMessageChannelManager;
    }
    protected override async Task Implementation(SocketSlashCommand socketSlashCommand, EditPostCommandParameters commandParameters)
    {
        var createdPostForEditing =
            await _dbContext.CreatePosts.SingleOrDefaultAsync(x => x.MessageId == commandParameters.PostId);

        if (createdPostForEditing == null)
        {
            await FollowupAsync("Post not found in Database");
            return;
        }

        if (createdPostForEditing.State == CreatePostState.Creating || createdPostForEditing.State == CreatePostState.Editing)
        {
            await FollowupAsync("Post is either in the process of being created or already editing.");
            return;
        }

        var messageChannel = await _socketMessageChannelManager.GetTextChannelFromIdAsync(createdPostForEditing.ChannelId);
        if (messageChannel == null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"Could not find message channel with ID: {createdPostForEditing.ChannelId}.  Removing from the database...");
            _dbContext.CreatePosts.Remove(createdPostForEditing);
            return;
        }
        var existingMessage = await messageChannel.GetMessageAsync(createdPostForEditing.MessageId);

        if (existingMessage == null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync($"Could not find message with ID: {createdPostForEditing.MessageId}.  Removing from the database...");
            _dbContext.CreatePosts.Remove(createdPostForEditing);
            return;
        }

        var content = existingMessage.Content;
        if (createdPostForEditing.UseEmbed)
        {
            content = existingMessage.Embeds.First().Description;
        }
        var richTextBuilder = new RichTextBuilder();
        richTextBuilder.AddText("Starting the process for editing an existing post.  The existing message contents are");
        richTextBuilder.AddCodeBlock(content);
        richTextBuilder.AddText("Reply to this message to edit");
        var followUpMessage = await FollowupAsync(richTextBuilder.Build());
        
        if (followUpMessage == null)
        {
            await _socketMessageChannelManager.SendLoggingErrorMessageAsync("Failed to initiate the create post command.");   
            return;
        }

        createdPostForEditing.State = CreatePostState.Editing;
        createdPostForEditing.ActivelyListenForResponseUntil = DateTime.UtcNow.AddMinutes(20);
        createdPostForEditing.CreatePostResponseToken = followUpMessage.Id;


    }
}

public class EditPostCommandParameters  
{
    [CommandParameterRequired]
    [CommandParameterDescription("The ID of the message to be edited")]
    public ulong PostId { get; set; }
}