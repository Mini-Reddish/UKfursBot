﻿using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UKFursBot.Commands;
using UKFursBot.Commands.CommandClassAttributes;
using UKFursBot.Context;
using UKFursBot.UserJoined;

namespace UKFursBot;

class Program
{
    private static IServiceProvider _services = null!;

    public static int Main(string[] args)
    {
        _services = ServiceProvider.Instance;

        InitialiseDatabase();

        new Program().MainAsync().GetAwaiter().GetResult();
        return 0;
    }

    private static void InitialiseDatabase()
    {
        var dbContext = _services.GetRequiredService<UKFursBotDbContext>();
        dbContext.Database.Migrate();
    }

    private async Task MainAsync()
    {
        var config = _services.GetRequiredService<IConfiguration>();
        var client = _services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;
        try
        {
            await client.LoginAsync(TokenType.Bot, config["AuthToken"]);
            await client.StartAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        client.GuildAvailable += ClientOnGuildAvailable;
        client.SlashCommandExecuted += ClientOnSlashCommandExecuted;
        client.ModalSubmitted += ClientOnModalSubmitted;
        client.UserJoined += ClientOnUserJoined;
        
        await Task.Delay(-1);
    }

    private Task LogAsync(LogMessage arg)
    {
        Console.WriteLine(arg.Message);
        return Task.CompletedTask;
    }

    private async Task ClientOnUserJoined(SocketGuildUser arg)
    {
        var onUserJoinedHandlers = _services.GetServices<IUserJoinedHandler>();
        
        foreach (var userJoinedHandler in onUserJoinedHandlers)
        {
            await userJoinedHandler.HandleUserJoined(arg);
        }
    }

    private async Task ClientOnModalSubmitted(SocketModal arg)
    {
        Console.WriteLine("modal submitted: " + arg.Data.CustomId);
        await arg.RespondAsync("done");
    }

    private async Task ClientOnSlashCommandExecuted(SocketSlashCommand arg)
    {
        await arg.RespondAsync("Request Received!  I'm going to do my best!");
        var command = _services.GetServices<ISlashCommand>().FirstOrDefault(x =>
        {
            var commandNameAttribute = x.GetType().GetCustomAttribute<CommandNameAttribute>();
            return string.Equals(commandNameAttribute?.Name, arg.CommandName, StringComparison.InvariantCultureIgnoreCase);
        });

        var context = _services.GetRequiredService<UKFursBotDbContext>();

        if (command == null)
        {
            //Log error that command implementation does not exist.  Should never happen by design.
            return;
        }
        command.MapSocketSlashCommandToParameters(arg);
        await command.Execute(context, arg);
        await context.SaveChangesAsync();
        await command.OnSuccessfulCommandCompletion(context, arg);

    }

    private async Task ClientOnGuildAvailable(SocketGuild guild)
    {
        await guild.DeleteApplicationCommandsAsync();
        var commands = _services.GetServices<ISlashCommand>();

        foreach (var slashCommand in commands)
        {
            try
            {
                var commandNameAttribute = slashCommand.GetType().GetCustomAttribute<CommandNameAttribute>();
                var commandDescriptionAttribute = slashCommand.GetType().GetCustomAttribute<CommandDescriptionAttribute>();

                if (commandNameAttribute == null || commandDescriptionAttribute == null)
                {
                    //TODO: Log missing command attributes.
                    continue;
                }
                var guildCommand = new SlashCommandBuilder()
                    .WithName(commandNameAttribute.Name.ToLowerInvariant())
                    .WithDescription(commandDescriptionAttribute.Description)
                    .BuildOptionsFromParameters(slashCommand)
                    .Build();

                await guild.CreateApplicationCommandAsync(guildCommand);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}