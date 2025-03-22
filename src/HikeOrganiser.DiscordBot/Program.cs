using Discord;
using Discord.WebSocket;
using HikeOrganiser.Core.Options;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        services.AddOptions<AppKeys>()
            .Bind(host.Configuration.GetSection("Keys"));

        services.AddSingleton(async x =>
        {
            DiscordSocketClient client = new(new()
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.AllUnprivileged,
                LogLevel = LogSeverity.Info
            });
            
            IOptions<AppKeys> options = x.GetRequiredService<IOptions<AppKeys>>();
            await client.LoginAsync(TokenType.Bot, options.Value.BotToken);

            return client;
        });

        services.AddLogging(opt =>
        {
            opt.AddConsole();
        });

        services.AddSignalR();

        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(host.Configuration.GetConnectionString(""));
        });
    })
    .UseConsoleLifetime()
    .Build();
    
await host.RunAsync();