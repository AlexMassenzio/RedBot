using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using RedBot.Commands.Utility;

namespace RedBot.Commands
{
	class UtilityCommands
	{
		public UtilityCommands(CommandService cmd)
		{
			Console.WriteLine("Utility");
			try
			{
				InitCommands(cmd);
				Console.WriteLine("|-> [ OK ] Generic");
			}
			catch
			{
				Console.WriteLine("|-> [FAIL] Generic");
			}

			try
			{
				new Settings(cmd);
				Console.WriteLine("|-> [ OK ] Settings");
			}
			catch
			{
				Console.WriteLine("|-> [FAIL] Settings");
			}
			Console.WriteLine();
		}

		private void InitCommands(CommandService cmd)
		{
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
	}
}
