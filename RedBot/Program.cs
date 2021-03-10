﻿using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot
{
	class Program
	{
		static void Main(string[] args)
		{
			if (Properties.Settings.Default.discordToken == "" && Properties.Settings.Default.adminChannel == "")
			{
				Console.WriteLine("I can see this is the first time your using me! To start, I'll need two things from you:\n  1) Your Discord bot token");
				Properties.Settings.Default.discordToken = Console.ReadLine().Trim();

				Console.WriteLine(" 2) The channel where you could to control me from (do not include the '#')");
				Properties.Settings.Default.adminChannel = Console.ReadLine().Trim();

				Properties.Settings.Default.Save();
			}
			
			new Program().MainAsync().GetAwaiter().GetResult();
		}
		public async Task MainAsync()
		{
			RedBotClient bot = new RedBotClient();
			await bot.StartupAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
	}
}
