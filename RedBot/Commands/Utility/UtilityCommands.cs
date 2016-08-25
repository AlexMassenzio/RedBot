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

			cmd.CreateCommand("door")
				.Description("Alerts the eboard to grab the door for you during events. You should get a private message confirming it was sent.")
				.Do(async e =>
				{
					await e.Server.TextChannels.FirstOrDefault(channel => channel.Name == "eboard")?.SendMessage(String.Format("{0}, {1} is at the door!", e.Server.Roles.FirstOrDefault(channel => channel.Name == "eboard")?.Mention, e.User.Name));
					await e.User.SendMessage("The eboard has been alerted!");
				});

            cmd.CreateCommand("roll")
                .Description("Rolls a die")
                .Parameter("modifier", ParameterType.Optional)
                .Do(async e =>
                {
                    Random rand = new Random();
                    string die = "d100";
                    if (e.Args[0] != "")
                    {
                        die = e.Args[0];
                    }
                    int nonDieBound = 0;
                    if (int.TryParse(die,out nonDieBound))
                    {
                        die = "d" + die;
                    }
                    string[] parts = die.Split('d');
                    if (parts.Length < 2)
                    {
                        await e.Channel.SendMessage("Improper dice");
                        return;
                    }
                    parts[0] = parts[0] == "" ? "1" : parts[0];
                    int rolls = 0;
                    int sides = 0;
                    if (!int.TryParse(parts[0], out rolls))
                    {
                        await e.Channel.SendMessage("Improper dice");
                        return;
                    }
                    if (!int.TryParse(parts[1], out sides))
                    {
                        await e.Channel.SendMessage("Improper dice");
                        return;
                    }
                    int sum = 0;
                    for (int i = 0; i < rolls; i++)
                    {
                        sum += rand.Next(1, sides + 1);
                    }
                    await e.Channel.SendMessage(sum.ToString());
                });
        }
	}
}
