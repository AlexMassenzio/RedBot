using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot
{
	public class RedBotClient
	{
		private DiscordClient bot;

		/// <summary>
		/// Begins a new instance of the bot.
		/// </summary>
		/// <param name="token">The token string given to your bot upon creation</param>
		public RedBotClient(string token)
		{
			bot = new DiscordClient();

			bot.UsingCommands(x => {
				x.PrefixChar = '~';
				x.HelpMode = HelpMode.Public;
			});

			InitializeCommands(bot.GetService<CommandService>());

			bot.MessageReceived += ProcessMessageSpecial;

			bot.ExecuteAndWait(async () =>
			{
				await bot.Connect(token);
			});
		}

		/// <summary>
		/// Initializes all commands for the bot to run.
		/// </summary>
		/// <param name="cmd">The DiscordClient's CommandService</param>
		private void InitializeCommands(CommandService cmd)
		{
			//Hey! You're probably here to contribute some commands! If you feel lost, check out the RedBot wiki or email me at alexmassenzio@gmail.com

			cmd.CreateCommand("ping")
				.Description("Replies with \"pong!\".")
				.Do(async e =>
				{
					await e.Channel.SendMessage("pong!");
				});

			cmd.CreateCommand("wiki")
				.Description("Provides a link to the RedBot wiki.")
				.Do(async e =>
				{
					await e.Channel.SendMessage("https://github.com/AlexMassenzio/RedBot/wiki");
				});
		}

		/// <summary>
		/// Handles any non command messages.
		/// </summary>
		/// <param name="s">Sender</param>
		/// <param name="e">Event Args</param>
		private void ProcessMessageSpecial(object s, MessageEventArgs e)
		{
			if (e.Message.IsAuthor)
				return;

			Console.WriteLine(e.Message);

			if(e.Message.Text.ToLower().Contains("dat boi"))
			{
				e.Channel.SendFile("images/memes/datboi.jpg");
			}
		}
	}
}
