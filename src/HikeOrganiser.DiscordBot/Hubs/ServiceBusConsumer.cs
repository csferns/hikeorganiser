using System.Collections.Concurrent;
using Azure.Messaging.ServiceBus;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using HikeOrganiser.Core.Constants;
using HikeOrganiser.Core.Interfaces;
using HikeOrganiser.Core.Model.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HikeOrganiser.DiscordBot.Hubs;

public sealed class ServiceBusConsumer : IHostedService
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly ILogger<ServiceBusProcessor> _logger;

    private readonly List<ServiceBusProcessor> _processors = [];

    private ConcurrentDictionary<int, ulong> _events = [];
    
    public ServiceBusConsumer(ServiceBusClient serviceBusClient, DiscordSocketClient discordSocketClient, ILogger<ServiceBusProcessor> logger)
    {
        _serviceBusClient = serviceBusClient;
        _discordSocketClient = discordSocketClient;
        _logger = logger;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Handle<EventCreatedArgs>(ServiceBusQueues.EventCreated, async (obj, token) =>
        {
            if (_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                return;
            }
            
            SocketGuild? guild = _discordSocketClient.Guilds.First(x => x.Id == obj.DiscordGuildId);
            SocketTextChannel channel = guild.Channels.OfType<SocketTextChannel>().First(x => x.Id == obj.DiscordChannelId);

            Embed embed = CreateEmbed(obj, guild);

            RestUserMessage response = await channel.SendMessageAsync(embed: embed);

            _events.TryAdd(obj.EventId, response.Id);

        }, cancellationToken);
        
        Handle<UserJoinArgs>(ServiceBusQueues.EventJoined, async (obj, token) =>
        {
            if (_discordSocketClient.ConnectionState != ConnectionState.Connected)
            {
                return;
            }
            
            SocketGuild? guild = _discordSocketClient.Guilds.First(x => x.Id == obj.DiscordGuildId);
            SocketTextChannel channel = guild.Channels.OfType<SocketTextChannel>().First(x => x.Id == obj.DiscordChannelId);

            if (!_events.TryGetValue(obj.EventId, out ulong messageId))
            {
                return;
            }
            
            Embed embed = CreateEmbed(obj, guild);
            
            await channel.ModifyMessageAsync(messageId, properties =>
            {
                properties.Embed = embed;
            });
            
        }, cancellationToken);

        return Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
    }

    private static Embed CreateEmbed(IDiscordTargetableEvent obj, SocketGuild guild)
    {
        EmbedBuilder builder = new()
        {
            Title = "",
        };

        if (obj.EventCreatedByDiscordId.HasValue)
        {
            SocketGuildUser? user = guild.Users.First(x => x.Id == obj.EventCreatedByDiscordId.Value);

            builder = builder.WithAuthor(user);
        }

        return builder.Build();
    }

    private void Handle<T>(string queueName, Func<T, CancellationToken, Task> action, CancellationToken cancellationToken)
    {
        ServiceBusProcessor processor = _serviceBusClient.CreateProcessor(queueName);
        
        processor.ProcessMessageAsync += async args =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                await args.AbandonMessageAsync(args.Message, cancellationToken: cancellationToken);
            }

            await action(args.Message.Body.ToObjectFromJson<T>()!, cancellationToken);
            
            await args.CompleteMessageAsync(args.Message, cancellationToken);
        };
        
        processor.ProcessErrorAsync += args =>
        {
            _logger.LogError(args.Exception, "Error encountered while processing message for queue {queueName}", queueName);
            
            return Task.CompletedTask;
        };

        _processors.Add(processor);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (ServiceBusProcessor processor in _processors)
        {
            if (processor.IsClosed)
            {
                continue;
            }
            
            await processor.CloseAsync(cancellationToken);
        }
        
        _processors.Clear();
    }
}