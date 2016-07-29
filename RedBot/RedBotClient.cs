using Discord;
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

			bot.MessageReceived += ProcessMessage;

			bot.ExecuteAndWait(async () =>
			{
				await bot.Connect(token);
			});
		}

		/// <summary>
		/// Handles all static commands sent by the server's users.
		/// </summary>
		/// <param name="s">Sender</param>
		/// <param name="e">Event Args</param>
		private void ProcessMessage(object s, MessageEventArgs e)
		{
			if (e.Message.IsAuthor)
				return;

			Console.WriteLine(e.Message);

			if(e.Message.Text == "~ping")
			{
				e.Channel.SendMessage("pong!");
			}
			else if(e.Message.Text.ToLower().Contains("dat boi") && e.Channel.Name == "bot_test")
			{
				e.Channel.SendFile("images/memes/datboi.jpg");
			}
		}
	}
}
