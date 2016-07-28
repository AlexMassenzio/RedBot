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

		public RedBotClient()
		{
			bot = new DiscordClient();

			bot.MessageReceived += ProcessMessage;

			bot.ExecuteAndWait(async () =>
			{
				await bot.Connect("");
			});
		}

		private void ProcessMessage(object sender, MessageEventArgs e)
		{
			if (e.Message.IsAuthor)
				return;

			if(e.Message.Text == "~ping")
			{
				e.Channel.SendMessage("pong");
			}
			else if(e.Message.Text.ToLower().Contains("dat boi") && e.Channel.Name == "bot_test")
			{
				e.Channel.SendFile("images/memes/datboi.jpg");
			}
		}
	}
}
