using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace MyBot_v2
{
    class Modules
    {
    }

    //TODO: kill me, userstats 

    public class LookUps : ModuleBase                                                       //  Our Lookup Module
    {
        [Command("diablo"), Summary("A tool to look up diablo players on battle.net")]
        public async Task DiabloLookUpAsync([Summary("The user's battletag")]string battleTag, [Summary("The realm the given user is on")]string realm)
        {
            ///  Changes the '#' in the battletag to a '-' in order to work with the url syntax

            char[] battleTagWithPound = battleTag.ToCharArray();                            //  Converts the input string to a char array

            for (int i = 0; i < battleTagWithPound.Length; i++)                             //  Loops through the array finding the characters that equal '#' then changing them to '-'
                if (battleTagWithPound[i] == '#')                                           //  Checks if the char at index [i] is equal to '#'
                {
                    battleTagWithPound[i] = '-';                                            //  Changes the char at index [i] to '-' 
                }

            string newBattleTag = new string(battleTagWithPound);                           //  Instantiates a new string that is the char array

            await ReplyAsync($"http://{realm}.battle.net/d3/en/profile/{newBattleTag}/");   //  Sends the output through the bot
        }

        [Command("google"), Summary("A tool to do google searches")]
        public async Task GoogleSearchAsync([Summary("The search word(s)"), Remainder]string search)                                            //  Function, remainder is an attribute that tells the backend that the parameter can be with spaces
        {
            char[] searchToUrl = search.ToCharArray();                                                                                          //  Converts the input string to a char array

            for (int i = 0; i < searchToUrl.Length; i++)                                                                                        //  Loops through the array finding the characters that equal ' ' then changing them to '+'
                if (searchToUrl[i] == ' ')                                                                                                      //  Checks if the char at index [i] is equal to ' '
                {
                    searchToUrl[i] = '+';                                                                                                       //  Changes the char at index [i] to '+' 
                }

            search = new string(searchToUrl);                                                                                                   //  Changes the string to be the char array
            await ReplyAsync($"https://www.google.dk/search?q={search}");                                                                       //  Sends the output through the bot
        }
    }

    public class RandomShitposts : ModuleBase                                                                                                   //  Our shitpost module
    { 

        [Command("goodshit"), Summary("Posts a spicy meme(right there(right there))")]
        public async Task GoodShitAsync()
        {
            await ReplyAsync("👌👀👌👀👌👀👌👀👌👀 good shit go౦ԁ sHit👌 thats ✔ some good👌👌shit right👌👌there👌👌👌 right✔there ✔✔");        //  The bot posts text to the channel    
        }

        [Command("jews"), Summary("Random anti-semitic thing")]
        public async Task JewsAsync()
        {
            await ReplyAsync(":regional_indicator_f: :regional_indicator_u: :regional_indicator_c: :regional_indicator_k: " +                   //  The bot posts text to the channel
                             ":regional_indicator_j: :regional_indicator_e: :regional_indicator_w: :regional_indicator_s: ");
            await Context.Channel.SendFileAsync("TwitchEmotes/swastika.png");                                                                   //  The bot sends a file to the channel
        }

        [Command("tilt"), Summary("//////")]
        public async Task TiltAsync()
        {
            await ReplyAsync("/|/|orte/|/");                                                                                                    //  The bot posts text to the channel
        }

        [Command("jack"), Summary("Yndlings-asiaten")]
        public async Task JackAsync()
        {
            await Context.Channel.SendFileAsync("TwitchEmotes/MingLee.jpg");                                                                    //  The bot sends a file to the channel
        }

        [Command("becker"), Summary("VoHiYo")]
        public async Task BeckerAsync()
        {
            await Context.Channel.SendFileAsync("TwitchEmotes/VoHiYo.jpg");                                                                     //  The bot sends a file to the channel
            await Context.Channel.SendMessageAsync("Weeb");                                                                                     //  The bot posts text to the channel
            await Context.Channel.SendFileAsync("TwitchEmotes/VoHiYo.jpg");                                                                     //  The bot sends a file to the channel
        }

        [Command("alex is a stupid"), Summary("Spicy meme fusion")]
        public async Task TriHardAsync()
        {
            await Context.Channel.SendFileAsync("TwitchEmotes/TriHard.jpg");                                                                    //  The bot sends a file to the channel
        }

        [Command("coin"), Summary("Coinflip")]
        public async Task CoinflipAsync()
        {
            string[] headsTails = {"Heads", "Tails"};                                                                                           //  Creates a string array with two indexes "heads" and "tails"
            Random random = new Random();                                                                                                       //  Creates a new random
            int randomNumber = random.Next(headsTails.Length);                                                                                  //  Instantiates an int that uses the Next property of the random to store a random number between 0 and the length of our string array
            await ReplyAsync(headsTails[randomNumber]);                                                                                         //  The bot sends a string at the index equal to our randomNumber to the channel
        }
    }

    public class Info : ModuleBase
    {
        //  Doesn't work
        [Command("help"), Summary("A help command, currently WIP")]
        public async Task HelpAsync()
        {
            await ReplyAsync("");                                                                                                               //  Sends text to the channel
        }

        [Command("say"), Summary("Echoes a message.")]
        public async Task SayAsync([Summary("The text to echo")] string echo){await ReplyAsync(echo);}                                          //  Sends the input parameter 'echo' as text to the channel

        [Command("info"), Alias("about", "stats"), Summary("Gets info about the bot.")]
        public async Task InformationAsync()                                                                                                    //  We start the function
        {
            var application = await Context.Client.GetApplicationInfoAsync();                                                                   //  Stores the application info in a variable called 'application'
            await ReplyAsync(                                                                                                                   //  Initializes the ReplyAsync function which sends a string out as text to the channel
                $"{Format.Bold("Info")}\n" +                                                                                                    //  We write "Info" in bold format, then we concatenate a string onto that
                $"- Author: {application.Owner.Username} (ID {application.Owner.Id})\n" +                                                       //  We write the user name of the owner of the application and their ID, then concatenate a string onto that
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +                                                                         //  We write the version of the library, then concatenate a string onto that
                $"- Uptime: {GetUpTime()}\n\n" +                                                                                                //  We write the Uptime of the program, ie. time elapsed since startup of the bot, by calling a self-made function, then concatenate a string onto that

                $"{Format.Bold("Stats")}\n" +                                                                                                   //  We write "Stats" in bold format, then concatenate a string onto that
                $"- Heap Size: {GetHeapSize()} MB\n" +                                                                                          //  We write the heap size, ie. the amount of memory used by the program, by calling a selfmade function, then we concatenate a string onto that
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +                                                         //  We write how many guilds the bot is a part of, then concatenate a string onto that
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +                                    //  We write how many channels the bot is active in, then concatenate a string onto that
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"                                            //  We write how many users the bot services.
                );                                                                                                                              //  We end the function
        }

        [Command("uptime"), Summary("How long the bot has been online for")]
        public async Task UptimeAsync()                             
        {
            await ReplyAsync(                                                                                                   
                             $"{Format.Bold("Uptime")}\n" +                                                                                     //  We write "uptime" in bold format, then concatenate a string onto that
                             $"{GetUpTime()}"                                                                                                   //  We call the uptime function to write it.
                             );
        }
        

        private static string GetUpTime() => (System.DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");         //  Makes a function which takes the current time and subtracts the starttime of the currentprocess from that, then converts it to a string in a dd\hh\mm\ss format

        private static string GetHeapSize() => System.Math.Round(System.GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();             //  Makes a function which takes the total memory and divides it by 1024 * 1024 to get the right format, then tells it that it should have 2 decimal points, then converts it to a string
    }

    public class AdminModule : ModuleBase
    {
        [Command("kick"), Summary("Kicks a user."), RequirePermission(GuildPermission.KickMembers)]
        public async Task KickAsync([Summary("The user to kick.")]IGuildUser target, [Optional]string reason)
        {
            IDMChannel dmchannel = await target.CreateDMChannelAsync();                                                                         //  Stores a dmchannel with the target
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;                              //  Stores the logchannel

            await dmchannel.SendMessageAsync($"You have been kicked from {target.Guild.ToString()}.");                                          //  Sends a message in the dmchannel
            await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been KICKED.");                                              //  Sends text to the logchannel


            if (!string.IsNullOrWhiteSpace(reason))                                                                                             //  Checks to see if the input parameter 'reason' is null or whitespace
            {
                await dmchannel.SendMessageAsync($"Reason: {reason}.");                                                                         //  Sends the reason parameter to the dmchannel
                await logchannel.SendMessageAsync($"REASON: {reason}.");                                                                        //  Sends the reason parameter to the logchannel        
            }

            await dmchannel.SendMessageAsync($"If you would like to appeal this kick, send a message to: {Context.User.Mention}.");             //  Sends text to the dmchannel
            await dmchannel.SendMessageAsync(Format.Italics("this is an automated message."));                                                  //  Sends text to the dmchannel in the italics format


            await target.KickAsync();                                                                                                           //  Calls the KickAsync function on the target
            await Context.Channel.SendMessageAsync($"Kicked user: {target}.");                                                                  //  Sends a message to the channel to say that the user has been kicked
            await Context.Channel.SendFileAsync("Gifs/Admin_Kicks_User.gif");                                                                   //  Sends a gif to the channel
        }

        [Command("ban"), Summary("Bans a user."), RequirePermission(GuildPermission.BanMembers)]
        public async Task BanAsync([Summary("The user to ban")]IGuildUser target, [Summary("The reason for one such ban")] string reason)
        {
            IDMChannel dmchannel = await target.CreateDMChannelAsync();                                                                         //  Stores a dmchannel with the target
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;                              //  Gets the logchannel, then stores it

            if (!string.IsNullOrWhiteSpace(reason))                                                                                             //  Checks to see if the input parameter 'reason' is null or whitespace                           
            {
                await Context.Channel.SendMessageAsync(                                                                                         
                                        $"Banned user: {target}.\n" +                                                                           //  Sends the banned user to the channel
                                        $"Reason: {reason}");                                                                                   //  Sends the reason the channel
                await dmchannel.SendMessageAsync($"You have been banned from {target.Guild.ToString()}.");                                      //  Sends text to the dmchannel
                await dmchannel.SendMessageAsync($"Reason: {reason}.");                                                                         //  Sends text to the dmchannel  
                await dmchannel.SendMessageAsync($"If you would like to appeal this ban, send a message to: {Context.User.Mention}.");          //  Sends text to the dmchannel
                await dmchannel.SendMessageAsync(Format.Italics("this is an automated message."));                                              //  Sends text to the dmchannel
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been BANNED.\nREASON: {reason}.");                       //  Sends text to the logchannel
            }
            try
            {
                await Context.Guild.AddBanAsync(target);                                                                                        //  Tries to add the target to the server's ban list
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Error. Most likely I don't have sufficient permissions");                               //  If the ban fails it will send this text to the channel
            }
        }

        //  Does not work as intended, stops me from calling other functions when timing out for a number of seconds
        [Command("timeout"), Summary("Timeouts a user"), RequirePermission(GuildPermission.ManageRoles)]
        public async Task TimeoutAsync([Summary("The user to timeout")]IGuildUser target, [Summary("Amount of seconds to timeout"), Optional]int seconds)
        {
            IRole timedOutRole = Context.Guild.GetRole(248074293211037696);                                                                     //  Stores a role, that serves as the timed out role
            IRole welcomeRole = Context.Guild.GetRole(248434734240104459);                                                                      //  Stores a different role that is assigned to every newcomer in the server to get around discord's @everyone role
            IDMChannel dmchannel = await target.CreateDMChannelAsync();                                                                         //  Stores a dmchannel with the target
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;                              //  Gets the logchannel and stores it

            seconds *= 1000;                                                                                                                    //  Takes the seconds parameter and converts it to miliseconds

            await target.AddRolesAsync(timedOutRole);                                                                                           //  Assigns the timed out role to the target
            await target.RemoveRolesAsync(welcomeRole);                                                                                         //  Removes the welcome role from the target
            
            if (seconds != 0)                                                                                                                   //  Checks to see if our seconds parameter isn't 0
            {
                await dmchannel.SendMessageAsync($"You have been timed out from {target.Guild.ToString()} for {seconds / 1000} seconds.");      //  Sends a message to the dmchannel
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been timed out for {seconds/1000} seconds.");            //  Sends a message to the logchannel

                await Task.Delay(seconds).ConfigureAwait(true);                                                                                 //  Stops the task for as many miliseconds as the seconds paramater is equal to. This doesn't work because it makes me unable to call any other functions

                await target.AddRolesAsync(welcomeRole);                                                                                        //  Assigns the welcome role to the target
                await target.RemoveRolesAsync(timedOutRole);                                                                                    //  Removes the timed out role from the target

                await dmchannel.SendMessageAsync($"You are no longer timed out from {target.Guild.ToString()}.");                               //  Sends text to the dmchannel
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} is no longer timed out.");                                   //  Sends text to the l
            }
            else                                                                                                                                //  Else
            {
                await dmchannel.SendMessageAsync($"You have been timed out from {target.Guild.ToString()} for an unspecified amount of time."); //  Sends a message to the dmchannel
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been timed out for an unspecified amount of time.");     //  Sends a message to the logchannel
            }
        }

        [Command("timein"), Summary("Timeins a user")]
        public async Task TimeinAsync([Summary("The user to timein")]IGuildUser target)
        {
            IRole timedOutRole = Context.Guild.GetRole(248074293211037696);                                                                     //  Stores the timed out role
            IRole welcomeRole = Context.Guild.GetRole(248434734240104459);                                                                      //  Stores the welcome role
            IDMChannel dmchannel = await target.CreateDMChannelAsync();                                                                         //  Creates a dmchannel with the target and stores it
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;                              //  Gets the logchannel and stores it

            await target.RemoveRolesAsync(timedOutRole);                                                                                        //  Removes the timed out role from the target
            await target.AddRolesAsync(welcomeRole);                                                                                            //  Assigns the welcome role to the target

            await dmchannel.SendMessageAsync($"You are no longer timed out from {target.Guild.ToString()}.");                                   //  Sends a message to the dmchannel        
            await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} is no longer timed out.");                                       //  Sends a message to the logchannel

        }

        //  Does not work as of right now
        [Command("purge"), Summary("Purges the chat, currently WIP.")]
        public async Task PurgeAsync([Summary("The number of messages to delete")]int numberOfMessagesToDel)
        {
            //IEnumerable<IMessage> messages = Context.Channel.GetMessagesAsync(numberOfMessagesToDel) as IEnumerable<IMessage>;
            //Context.Channel.GetMessagesAsync(100);

            //await ReplyAsync();
            //await Context.Channel.DeleteMessagesAsync();
            //TODO: Log the purge
        }

         // Does not work at the minute
         [Command("unban"), Summary("Unbans a user."), RequirePermission(GuildPermission.BanMembers)]
         public async Task UnbanAsync([Summary("The user to unban")]IUser target)
         {
            IBan bannedUsers = await Context.Guild.GetBansAsync() as IBan;
            IUser banneduser = bannedUsers.User;

            if (banneduser.Equals(target))
            {
                await Context.Guild.RemoveBanAsync(target);
                await Context.Channel.SendMessageAsync($"Unbanned user: {target}.");
            }                                    
         }
    }
}
