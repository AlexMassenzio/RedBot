using Discord;
using Discord.Commands;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot
{
	public class RedBotClient
	{
		//General
		private DiscordClient bot;

		//Challonge
		private ChallongeAPIHandler challonge;
		private bool challongeSupportEnabled;

		/// <summary>
		/// Begins a new instance of the bot.
		/// </summary>
		/// <param name="token">The token string given to your bot upon creation</param>
		public RedBotClient(string token)
		{
			bot = new DiscordClient();

			//initializing Challonge handler
			try
			{
				challonge = new ChallongeAPIHandler(Properties.Settings.Default.challongeAccount, Properties.Settings.Default.challongeToken);
				challongeSupportEnabled = true;
				Console.WriteLine("[ OK ] Challonge Support");
			}
			catch
			{
				Console.WriteLine("[FAIL] Challonge Support");
				challongeSupportEnabled = false;
			}

			bot.MessageReceived += ProcessMessageSpecial;

			bot.UsingCommands(x => {
				x.PrefixChar = '~';
				x.HelpMode = HelpMode.Public;
			});

			InitializeCommands(bot.GetService<CommandService>());

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

			if (challongeSupportEnabled)
			{
				cmd.CreateCommand("signup")
					.Description("Adds the user to the current C2GS tournament.")
					.Parameter("player_name", Discord.Commands.ParameterType.Required)
					.Do(async e =>
					{
						string player_name = e.GetArg("player_name");

						System.Net.HttpStatusCode responseCode = await challonge.AddParticipantAsync(player_name);

						Console.WriteLine(responseCode);

						if (responseCode == System.Net.HttpStatusCode.OK)
						{
							await e.Channel.SendMessage(string.Format("{0} has been added to the tournament!", player_name));
						}
						else
						{
							await e.Channel.SendMessage(string.Format("{0}, there was a problem adding you to the torunament, try again in a sec or message the TO if the problem persists.", player_name));
						}
					});
				cmd.CreateCommand("select-tourney")
					.Description("MODERATOR/EBOARD ONLY: Sets the focus to the specified tourney. id = challonge.com/tourneyID")
					.Parameter("id", Discord.Commands.ParameterType.Required)
					.Do(async e =>
					{
						challonge.SelectedTournament = e.GetArg("id");
						await e.Channel.SendMessage(string.Format("The bot's focus has been set to chalonge.com/{0}", e.GetArg("id")));
					});
			}
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
