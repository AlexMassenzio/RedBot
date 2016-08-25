using Discord;
using Discord.Commands;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedBot.Commands;

namespace RedBot
{
	public class RedBotClient
	{
		//General
		private DiscordClient bot;

		/// <summary>
		/// Begins a new instance of the bot.
		/// </summary>
		/// <param name="token">The token string given to your bot upon creation</param>
		public RedBotClient()
		{
			bot = new DiscordClient();

			bot.MessageReceived += ProcessMessageSpecial;

			bot.UsingCommands(x => {
				x.PrefixChar = '~';
				x.HelpMode = HelpMode.Public;
			});

			InitializeCommands(bot.GetService<CommandService>());

			try
			{
				bot.ExecuteAndWait(async () =>
				{
					await bot.Connect(Properties.Settings.Default.discordToken);
				});
			}
			catch (Discord.Net.HttpException e)
			{
				Console.WriteLine("Something went wrong when tring to connect to the Discord service. Usually has to do with an invalid token.\n" + 
					"Would you like to try a new token? (y/n)");
				if(Console.ReadLine().Trim().ToLower() == "y")
				{
					Console.WriteLine("Please enter your new token: ");
					Properties.Settings.Default.discordToken = Console.ReadLine().Trim();
					Properties.Settings.Default.Save();
				}
				Console.WriteLine("I'm turning off! (Press any key to continue)");
				Console.ReadKey();
				Environment.Exit(0);
			}
		}

		/// <summary>
		/// Initializes all commands for the bot to run.
		/// </summary>
		/// <param name="cmd">The DiscordClient's CommandService</param>
		private void InitializeCommands(CommandService cmd)
		{
			//Hey! You're probably here to contribute some commands! If you feel lost, check out the RedBot wiki or email me at alexmassenzio(at)gmail.com

			new Commands.UtilityCommands(cmd);

			new Commands.ChallongeCommands(cmd);
		}

		/// <summary>
		/// Handles any non command messages.
		/// </summary>
		/// <param name="s">Sender</param>
		/// <param name="e">Event Args</param>
		private void ProcessMessageSpecial(object s, MessageEventArgs e)
		{
			Console.WriteLine(e.Message);

			if (e.Message.IsAuthor)
				return;

			if(e.Message.Text.ToLower().Contains("dat boi"))
			{
				e.Channel.SendFile("images/memes/datboi.jpg");
			}
		}
	}
}
