using Discord;
using System;
using Discord.Commands;
using Discord.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Policy;
using System.Threading;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using OverwatchAPI;

namespace CaduceusBot
{
    class Caduceus
    {
        DiscordClient discord;
        CommandService commands;

        static Dictionary<ulong, int> cookie = new Dictionary<ulong, int>();

        Random rnd;

        public ulong AdminID = 138007067695775744;

        string[] D6answers;
        string[] SelfCookie;
        string[] CoinflipResult;
        string[] eightBallResponse;
        string[] Bye;
        
        bool loop = false;

        public Caduceus()
        {
            rnd = new Random();

            //Arrays
            D6answers = new string[]
            {
                "Haha! Nur ne 1! Du Noob!",
                "Naja... Immerhin ne 2!",
                "Ne solide 3 würd ich sagen...",
                "Ne 4... Das geht noch besser!",
                "Not bad: ne 5!",
                "Nice! Full house oder so... Egal is ne 6!"
            };
            SelfCookie = new string[]
            {
                "Das ist schon bitter...",
                "Hast du keine Freunde oder was?",
                "Das lass ich jetzt mal so stehen...",
                "...",
                "At least you tried",
                "Nice try",
                "Nein. Einfach nein.",
                "Wow. Das ist armselig"
            };
            CoinflipResult = new string[]
            {
                "Kopf",
                "Kopf",
                "Kopf",
                "Kopf",
                "Kopf",
                "Auf der Seite.... Srsly? Wer hat den shit hier Programmiert?",
                "Kopf",
                "Kopf",
                "Kopf",
                "Kopf",
                "Zahl",
                "Zahl",
                "Zahl",
                "Digga! Die Münze is weg!",
                "Zahl",
                "Zahl",
                "Zahl",
                "Zahl",
                "Zahl",
                "Zahl",
            };
            eightBallResponse = new string[]
            {
                "Ja",
                "Nein",
                "Vielleicht...",
                "Probier es aus!",
                "Is'n versuch wert!",
                "Digga..."
            };
            Bye = new string[]
            {
                "Oke...",
                "Shutting down :frowning:",
                "Bye...",
                "Initiation self destruction...",
                "Bis später!",
                "Wenn du das sagst..."
            };

            var commandService = new CommandService(new CommandServiceConfigBuilder
            {
                AllowMentionPrefix = false,
                CustomPrefixHandler = m => 0,
                HelpMode = HelpMode.Private
            });

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '-';
                x.AllowMentionPrefix = true;
            });

            discord.UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
            });

            commands = discord.GetService<CommandService>();

            
            //Die Commands
            D6_cmd();
            UserID_cmd();
            Cookie_cmd();
            UselessButton_cmd();
            doge_cmd();
            ping_cmd();
            coinflip_cmd();
            d_cmd();
            credits_cmd();
            JoinDate_cmd();
            eightBall_cmd();
            disconect_cmd();
            loop_cmd();
            loopStart_cmd();
            loopStop_cmd();
            joinVoice_cmd();
            cookieLeaderboard_cmd();
            split_cmd();
            overwatch_cmds();

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("kein-random-string-mehr-damit-jannik-zufrieden-ist", TokenType.Bot);
            });

        }


        //Cmd Method
        private void D6_cmd()
        {
            commands.CreateCommand("d6")
                    .Description("Würfelt mit einem normalen Würfel und kommentiert das Ergebniss")
                            .Do(async e =>
                            {
                                int randomD6 = rnd.Next(D6answers.Length);
                                string D6post = D6answers[randomD6];
                                await e.Channel.SendMessage(D6post);
                            });
        }
        private void UserID_cmd()
        {
            commands.CreateCommand("UserID")
                .Alias(new string[] { "UserID", "userID", "userid", "Userid" })
                    .Description("Gibt die User ID des angegebenen Users aus.")
                        .Parameter("user")
                            .Do(async e =>
                            {
                                User u = e.Message.MentionedUsers.First();
                                await e.Channel.SendMessage("This User's ID is " + u.Id + ".");
                            });
        }
        private void Cookie_cmd()
        {
            commands.CreateCommand("Cookie")
                .Alias(new string[] { "Cookie", "cookie" })
                    .Description("Gibt dem angegebenen User einen Keks!")
                        .Parameter("user")
                            .Do(async e =>
                            {
                                User u = e.Message.MentionedUsers.First();
                                User a = e.Message.User;
                                if (u.Name == "Caduceus")
                                {
                                    await e.Channel.SendMessage("Danke :3");
                                }
                                else if (a.Name != u.Name)
                                {
                                    await e.Channel.SendMessage(a.Name + " hat " + u.Mention + " einen :cookie: gegeben.");
                                }
                                else
                                {
                                    int randomSelfCookie = rnd.Next(SelfCookie.Length);
                                    string SelfCookiePost = SelfCookie[randomSelfCookie];
                                    await e.Channel.SendMessage(SelfCookiePost);
                                }
                            });
        }
        private void UselessButton_cmd()
        {
            commands.CreateCommand("UselessButton")
                    .Description("Nothing Happened")
                             .Do(async e =>
                             {
                                 await e.Channel.SendMessage("* Nothing happens *");
                             });
        }
        private void doge_cmd()
        {
            commands.CreateCommand("doge")
                    .Description("Zeigt ein Doge-Bild...")
                            .Do(async e =>
                            {
                                await e.Channel.SendFile("pictures/doge.png");
                            });
        }
        private void ping_cmd()
        {
            commands.CreateCommand("ping")
                    .Description("Pong!")
                            .Do(async e =>
                            {
                                await e.Channel.SendMessage("Pong!");
                            });
        }
        private void coinflip_cmd()
        {
            commands.CreateCommand("coinflip")
                .Alias(new string[] { "coinflip", "d2" })
                    .Description("Wirft eine Münze")
                            .Do(async e =>
                            {
                                int randomCoinflip = rnd.Next(CoinflipResult.Length);
                                string CoinflipPost = CoinflipResult[randomCoinflip];
                                await e.Channel.SendMessage(CoinflipPost);
                            });
        }
        private void d_cmd()
        {
            commands.CreateCommand("d")
                        .Parameter("dice")
                            .Do(async e =>
                            {
                                int numVal = -1;
                                var amount = e.Message.RawText;
                                amount = amount.Remove(0, 3);
                                numVal = Convert.ToInt32(amount);
                                int Dice = new int();
                                Dice = rnd.Next(1, numVal);
                                await e.Channel.SendMessage("Du hast ne " + Dice + " gewürfelt");
                            });
        }
        private void credits_cmd()
        {
            commands.CreateCommand("credits")
                            .Do(async e =>
                            {
                                await e.Channel.SendMessage("__**SchluenzumJr v2.1**__\r\n \r\nVersion **2.1**\r\nProgrammed by **SchluenzumJr**");
                            });
        }
        private void JoinDate_cmd()
        {
            commands.CreateCommand("joindate")
                        .Parameter("user")
                            .Do(async e =>
                            {
                                User u = e.Message.MentionedUsers.First();
                                await e.Channel.SendMessage(u.Name + "ist seit dem" + u.JoinedAt + "auf diesem Server");
                            });
        }
        private void eightBall_cmd()
        {
            commands.CreateCommand("8ball")
                        .Parameter("question", ParameterType.Unparsed)
                            .Do(async e =>
                            {
                                int randomEightBall = rnd.Next(eightBallResponse.Length);
                                string eightBallPost = eightBallResponse[randomEightBall];
                                await e.Channel.SendMessage(eightBallPost);
                            });
        }
        private void disconect_cmd()
        {
            commands.CreateCommand("bye")
                .Do(async e =>
                {
                    if (e.Message.User.Id == AdminID)
                    {
                        int randomBye = rnd.Next(Bye.Length);
                        string ByePost = Bye[randomBye];

                        await e.Channel.SendMessage(ByePost);
                        Thread.Sleep(250);
                        await discord.Disconnect();
                    }
                });
        }
        private void loop_cmd()
        {
            commands.CreateCommand("loop")
                .Do(async e =>
                {
                    if (loop == true)
                    {
                        await e.Channel.SendMessage("#loop");
                    }
                    else
                    {
                        await e.Channel.SendMessage("STOP SPAM NOOBS!");
                    }
                });
        }
        private void loopStart_cmd()
        {
            commands.CreateCommand("loopStart")
                .Do(async e =>
                {
                    if (e.Message.User.Id == AdminID)
                    {
                        loop = true;
                        await e.Channel.SendMessage("Im now allowing Loops with another bot...");
                    }
                    else
                    {
                        await e.Channel.SendMessage("Du hast mir garnichts zu sagen!");
                    }
                });
        }
        private void loopStop_cmd()
        {
            commands.CreateCommand("loopStop")
                .Do(async e =>
                {
                    if (e.Message.User.Id == AdminID)
                    {
                        loop = false;
                        await e.Channel.SendMessage("Im no longer allowing Loops");
                    }
                    else
                    {
                        await e.Channel.SendMessage("Du hast mir garnichts zu sagen!");
                    }
                });
        }
        public void joinVoice_cmd()
        {
            commands.CreateCommand("join")
                   .Do(async e =>
                   {
                       await e.Channel.SendMessage("Right beside you");
                       var voiceChannel = discord.FindServers("GIA1A").FirstOrDefault().VoiceChannels.FirstOrDefault();
                       var _vClient = discord.GetService<AudioService>()
                           .Join(voiceChannel);
                   });
        }
        public void cookieLeaderboard_cmd()
        {
            commands.CreateCommand("leaderboard")
                   .Do(async e =>
                   {
                       await e.Channel.SendMessage("Neeeee");
                   });
        }
        private void split_cmd()
        {
            commands.CreateCommand("split")
                .Do(async e =>
                {
                    string rawText = e.Message.RawText;
                    rawText = rawText.Remove(0, 7);
                    string[] splitted_string = rawText.Split(' ');
                    string Num1 = splitted_string[0];
                    string Num2 = splitted_string[1];
                    await e.Channel.SendMessage("Splitted string to: " + Num1 + " and: " + Num2);
                });
        }
//      private void owAllStats_cmd()
//      {
//          commands.CreateCommand("overwatch")
//              .Parameter("battleTag")
//                  .Do(async e =>
//                  {
//                      string rawText = e.Message.RawText;
//                      string battleTag = rawText.Remove(0, 11);
//                      await e.Channel.SendMessage("Calculating Stats...");
//                      OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
//                      await player.DetectRegionPC();
//                      await player.UpdateStats();
//                      await e.Channel.SendMessage("Everything is given in seconds:");
//                      foreach (var item in player.CasualStats.GetHero("AllHeroes").GetCategory("Game"))
//                      {
//                          await e.Channel.SendMessage(item.Name + ": " + item.Value);
//                      }
//                  });
//      }
        private void overwatch_cmds()
        {
            discord.GetService<CommandService>().CreateGroup("overwatched", ow =>
            {
                ow.CreateCommand("casual")
                    .Parameter("battleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 20);
                            await e.Channel.SendMessage("Calculating stats...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            foreach (var item in player.CasualStats.GetHero("AllHeroes").GetCategory("Game"))
                            {
                                await e.Channel.SendMessage(item.Name + ": " + item.Value);
                            }
                            await e.Channel.SendMessage("(Time in seconds)");
                        });
                ow.CreateCommand("competitive")
                    .Parameter("batttleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 25);
                            await e.Channel.SendMessage("Calculating stats...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            foreach (var item in player.CompetitiveStats.GetHero("AllHeroes").GetCategory("Game"))
                            {
                                await e.Channel.SendMessage(item.Name + ": " + item.Value);
                            }
                            await e.Channel.SendMessage("(Time in seconds)");
                        });
                ow.CreateCommand("achievements")
                    .Parameter("batttleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 26);
                            await e.Channel.SendMessage("Calculating achievements...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            foreach (var item in player.Achievements.GetCategory("General"))
                            {
                                await e.Channel.SendMessage(item.Name + ": " + item.IsUnlocked);
                            }
                            await e.Channel.SendMessage("(True = owned)\n(False = not owned yet)");
                        });
                ow.CreateCommand("profile")
                    .Parameter("battleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 21);
                            await e.Channel.SendMessage("Searching URL...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            await e.Channel.SendMessage("Profile: " + battleTag + "\nURL: " + player.ProfilePortraitURL);
                        });
                ow.CreateCommand("profile picture")
                    .Parameter("batleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 29);
                            await e.Channel.SendMessage("Searching profile picture...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            await e.Channel.SendMessage("Profile: " + battleTag + "\nURL: " + player.ProfilePortraitURL);
                        });
                ow.CreateCommand("info")
                    .Parameter("battleTag")
                        .Do(async e =>
                        {
                            string rawText = e.Message.RawText;
                            string battleTag = rawText.Remove(0, 18);
                            await e.Channel.SendMessage("Searching general information...");
                            OverwatchPlayer player = new OverwatchPlayer(battleTag, Platform.pc);
                            await player.DetectRegionPC();
                            await player.UpdateStats();
                            await e.Channel.SendMessage("Profile: " + battleTag + "\nProfile URL: " + player.ProfileURL + "\nPlatform: " + player.Platform + "\nRegion: " + player.Region + "\nLevel: " + player.PlayerLevel + "\nCompetitive rank: " + player.CompetitiveRank);
                        });
            });
        }
        


        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
