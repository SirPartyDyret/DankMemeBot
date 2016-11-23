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
        static void Main(string[] args) => new Program().Run().GetAwaiter().GetResult();                                    //  Makes a new program called run, gets its awaiter, then checks to see whether it failed or not.

        private DiscordSocketClient client;                                                                                 //  Stores a DiscordSocketClient
        private CommandService commands;                                                                                    //  Stores a CommandService

        public async Task Run() 
        {
            client = new DiscordSocketClient(new DiscordSocketConfig()                                                      //  Sets the client variable to be a new socketconfig with certain properties
            {
                LogLevel = LogSeverity.Debug,                                                                               //  Sets the loglevel to be debug
                WebSocketProvider = () => new WS4NetProvider()                                                              //  This is done so the program works on windows 7
            });

            client.Log += Log;                                                                                              //  Makes a new event handler called Log
            
            commands = new CommandService();                                                                                //  Instantiates the commands

            string token = "";                                                                                              //  Our token gets stored

            await InstallCommands();                                                                                        //  Call the InstallCommands function

            await client.LoginAsync(TokenType.Bot, token);                                                                  //  Tells the program to login the bot
            await client.ConnectAsync();                                                                                    //  Tells the program to connect

            var map = new DependencyMap();                                                                                  //  Stores a new dependency map, that is used for our commands
            map.Add(client);                                                                                                //  adds the client to the map

            client.UserJoined += async (user) =>                                                                            //  Makes an event handler for when a user joins
            {
                SocketRole welcomeRole = user.Guild.GetRole(248434734240104459);                                            //  Stores the welcome role
                await user.AddRolesAsync(welcomeRole);                                                                      //  Assigns the welcome role to the user who joined    
                Discord.Rest.RestDMChannel dmchannel = await user.CreateDMChannelAsync();                                   //  Makes a dmchannel with the newly joined user
                await dmchannel.SendMessageAsync($"Welcome to {user.Guild.ToString()}, {user.Mention.ToString()}");         //  Writes a message in the dmchannel
            };

            await Task.Delay(-1);                                                                                           //  Delays the task for minus 1 milisecond                                                                                           
        }

        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;                                                                        //  Makes a new eventhandler for when the client receives a message
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());                                                    //  Adds the command modules
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;                                                                //  Stores a message by taking the messageParam
            if (message == null) return;                                                                                    //  Checks to see if the message is null and if it is then return

            int argPos = 0;                                                                                                 //  Stores an int called argPos and sets it to 0

            if (message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))         //  Checks to see if the message has a charprefix of '!', at the first position of the message or if the message has the bot's mention at the first position
            {
                var context = new CommandContext(client, message);                                                          //  Stores a CommandContext 

                var result = await commands.ExecuteAsync(context, argPos);                                                  //  Stores a variable result which is commands executing
                if (!result.IsSuccess)                                                                                      //  Checks to see if result isn't a success
                    await message.Channel.SendMessageAsync(result.ErrorReason);                                             //  sends an error message to the channel
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());                                                                              //  Writes the parameter msg to the console
            return Task.CompletedTask;                                                                                      //  Returns Task.CompletedTask as this is required for non-async tasks in Discord.Net
        }
    }
}
