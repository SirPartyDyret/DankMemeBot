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
    }

    public class RandomShitposts : ModuleBase                                                                                                   //   Our shitpost module
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
            await ReplyAsync("Work in progress");                                                                                               //  Sends text to the channel
        }

        [Command("say"), Summary("Echoes a message.")]
        public async Task SayAsync([Summary("The text to echo")] string echo){await ReplyAsync(echo);}

        [Command("info"), Alias("about", "stats"), Summary("Gets info about the bot.")]
        public async Task InformationAsync()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: {application.Owner.Username} (ID {application.Owner.Id})\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Uptime: {GetUpTime()}\n\n" +

                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
                );
        }

        [Command("uptime"), Summary("How long the bot has been online for")]
        public async Task UptimeAsync()
        {
            await ReplyAsync(
                             $"{Format.Bold("Uptime")}\n" +
                             $"{GetUpTime()}"
                             );
        }
        

        private static string GetUpTime()
            => (System.DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        private static string GetHeapSize() => System.Math.Round(System.GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
    }

    public class AdminModule : ModuleBase
    {
        [Command("kick"), Summary("Kicks a user."), RequirePermission(GuildPermission.KickMembers)]
        public async Task KickAsync([Summary("The user to kick.")]IGuildUser target, [Optional]string reason)
        {
            IDMChannel dmchannel = await target.CreateDMChannelAsync();
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;

            await dmchannel.SendMessageAsync($"You have been kicked from {target.Guild.ToString()}.");
            await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been KICKED.");


            if (!string.IsNullOrWhiteSpace(reason))
            {
                await dmchannel.SendMessageAsync($"Reason: {reason}.");
                await logchannel.SendMessageAsync($"REASON: {reason}.");
            }

            await dmchannel.SendMessageAsync($"If you would like to appeal this kick, send a message to: {Context.User.Mention}.");
            await dmchannel.SendMessageAsync(Format.Italics("this is an automated message."));


            await target.KickAsync();
            await Context.Channel.SendMessageAsync($"Kicked user: {target}.");
            await Context.Channel.SendFileAsync("Gifs/Admin_Kicks_User.gif");
        }

        [Command("ban"), Summary("Bans a user."), RequirePermission(GuildPermission.BanMembers)]
        public async Task BanAsync([Summary("The user to ban")]IGuildUser target, [Summary("The reason for one such ban")] string reason)
        {
            IDMChannel dmchannel = await target.CreateDMChannelAsync();
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;

            if (!string.IsNullOrWhiteSpace(reason))
            {
                await Context.Channel.SendMessageAsync(
                                        $"Banned user: {target}.\n" +
                                        $"Reason: {reason}");
                await dmchannel.SendMessageAsync($"You have been banned from {target.Guild.ToString()}.");
                await dmchannel.SendMessageAsync($"Reason: {reason}.");
                await dmchannel.SendMessageAsync($"If you would like to appeal this ban, send a message to: {Context.User.Mention}.");
                await dmchannel.SendMessageAsync(Format.Italics("this is an automated message."));
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been BANNED.\nREASON: {reason}.");
            }
            try
            {
                await Context.Guild.AddBanAsync(target);
            }
            catch
            {
                await Context.Channel.SendMessageAsync("Error. Most likely I don't have sufficient permissions");
            }
        }

        //  Does not work as intended, stops me from calling other functions when timing out for a number of seconds
        [Command("timeout"), Summary("Timeouts a user"), RequirePermission(GuildPermission.ManageRoles)]
        public async Task TimeoutAsync([Summary("The user to timeout")]IGuildUser target, [Summary("Amount of seconds to timeout"), Optional]int seconds)
        {
            IRole timedOutRole = Context.Guild.GetRole(248074293211037696);
            IRole welcomeRole = Context.Guild.GetRole(248434734240104459);
            IDMChannel dmchannel = await target.CreateDMChannelAsync();
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;

            seconds = seconds * 1000;

            await target.AddRolesAsync(timedOutRole);
            await target.RemoveRolesAsync(welcomeRole);

            if (seconds != 0)
            {
                await dmchannel.SendMessageAsync($"You have been timed out from {target.Guild.ToString()} for {seconds / 1000} seconds.");
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been timed out for {seconds/1000} seconds.");

                await Task.Delay(seconds).ConfigureAwait(true);

                await target.AddRolesAsync(welcomeRole);
                await target.RemoveRolesAsync(timedOutRole);

                await dmchannel.SendMessageAsync($"You are no longer timed out from {target.Guild.ToString()}.");
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} is no longer timed out.");
            }
            else
            {
                await dmchannel.SendMessageAsync($"You have been timed out from {target.Guild.ToString()} for an unspecified amount of time.");
                await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} has been timed out for an unspecified amount of time.");
            }
        }

        [Command("timein"), Summary("Timeins a user")]
        public async Task TimeinAsync([Summary("The user to timein")]IGuildUser target)
        {
            IRole timedOutRole = Context.Guild.GetRole(248074293211037696);
            IRole welcomeRole = Context.Guild.GetRole(248434734240104459);
            IDMChannel dmchannel = await target.CreateDMChannelAsync();
            ITextChannel logchannel = await Context.Guild.GetTextChannelAsync(246572485292720139) as ITextChannel;

            await target.RemoveRolesAsync(timedOutRole);
            await target.AddRolesAsync(welcomeRole);

            await dmchannel.SendMessageAsync($"You are no longer timed out from {target.Guild.ToString()}.");
            await logchannel.SendMessageAsync($"USER: {target} ID: {target.Id} is no longer timed out.");

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
