using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.WS4NetCompatibility; // Needs this namespace on win7
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyBot_v2
{
    public class Program
    {
        static void Main(string[] args) => new Program().Run().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandService commands;

        public async Task Run()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Debug,
                WebSocketProvider = () => new WS4NetProvider()
            });

            client.Log += Log;
            
            commands = new CommandService();

            string token = "";

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, token);
            await client.ConnectAsync();

            var map = new DependencyMap();
            map.Add(client);

            client.UserJoined += async (user) =>
            {
                SocketRole welcomeRole = user.Guild.GetRole(248434734240104459);
                await user.AddRolesAsync(welcomeRole);
                Discord.Rest.RestDMChannel dmchannel = await user.CreateDMChannelAsync();
                await dmchannel.SendMessageAsync($"Welcome to {user.Guild.ToString()}, {user.Mention.ToString()}");
            };

            

            await Task.Delay(-1);
        }

        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;
            await commands.AddModules(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            int argPos = 0;

            if (message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var context = new CommandContext(client, message);

                var result = await commands.Execute(context, argPos);
                if (!result.IsSuccess)
                    await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
