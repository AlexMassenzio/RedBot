using Discord;
using Discord.Commands;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedBot.Commands;
using Discord.WebSocket;
using System.Reflection;
using RedBot.Commands.Utility;

namespace RedBot
{
	public class RedBotClient
	{
		//General
		private readonly DiscordSocketClient bot;
		private readonly CommandService cmd;

		/// <summary>
		/// Begins a new instance of the bot.
		/// </summary>
		/// <param name="token">The token string given to your bot upon creation</param>
		public RedBotClient()
		{
			bot = new DiscordSocketClient();
			bot.Log += LogAsync;

			cmd = new CommandService();

			/*
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
			catch (Discord.Net.HttpException)
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
			*/
		}

		public async Task StartupAsync()
        {
			await bot.LoginAsync(TokenType.Bot, Properties.Settings.Default.discordToken);
			await bot.StartAsync();

			bot.MessageReceived += ProcessMessageAsync;

			await cmd.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
										services: null);
			await cmd.AddModuleAsync(typeof(Settings), null);
		}

		/*/// <summary>
		/// Initializes all commands for the bot to run.
		/// </summary>
		/// <param name="cmd">The DiscordClient's CommandService</param>
		private void InitializeCommands(CommandService cmd)
		{
			//Hey! You're probably here to contribute some commands! If you feel lost, check out the RedBot wiki or email me at alexmassenzio(at)gmail.com

			new Commands.UtilityCommands(cmd);

			new Commands.ChallongeCommands(cmd);
		}*/

		private async Task ProcessMessageAsync(SocketMessage msg)
		{
			// Don't process the command if it was a system message
			var message = msg as SocketUserMessage;
			if (message == null) return;

			// Create a number to track where the prefix ends and the command begins
			int argPos = 0;

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix('~', ref argPos) ||
				message.HasMentionPrefix(bot.CurrentUser, ref argPos)) ||
				message.Author.IsBot)
				return;

			// Create a WebSocket-based command context based on the message
			var context = new SocketCommandContext(bot, message);

			// Execute the command with the command context we just
			// created, along with the service provider for precondition checks.
			await cmd.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: null);
		}

		/*/// <summary>
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
		}*/

		private Task LogAsync(LogMessage msg)
        {
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
        }
	}
}
