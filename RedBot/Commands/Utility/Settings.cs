using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using RedBot.Commands;

namespace RedBot.Commands.Utility
{
	class Settings
	{
		public Settings(CommandService cmd)
		{
			InitCommands(cmd);
		}

		private void InitCommands(CommandService cmd)
		{
			cmd.CreateGroup("settings", cgb =>
			{
				cgb.CreateCommand("directory")
						.Alias("dir")
						.Description("Displays the commands you can use for settings.")
						.Do(async e =>
						{
							if (e.Channel.ToString() == Properties.Settings.Default.adminChannel)
							{
								await e.Channel.SendMessage("1) Change admin channel.\n2) Change challonge token.\n3) Change the challonge account.");
							}
							else
							{
								await e.Channel.SendMessage("Wong channel accessed.");
							}
						});

				cgb.CreateCommand("1")
						.Description("Changes the channel in which this settings panel comes up.")
						.Parameter("NewChannel", ParameterType.Required)
						.Do(async e =>
						{
							if (e.Channel.ToString() == Properties.Settings.Default.adminChannel)
							{
								Properties.Settings.Default.adminChannel = e.GetArg("NewChannel");
								Properties.Settings.Default.Save();
								await e.Channel.SendMessage(String.Format("Changed admin channel to {0}", e.GetArg("NewChannel")));
							}
							else
							{
								await e.Channel.SendMessage("Wong channel accessed.");
							}
						});

				cgb.CreateCommand("2")
						.Description("Changes the challonge token")
						.Parameter("NewToken", ParameterType.Required)
						.Do(async e =>
						{
							if (e.Channel.ToString() == Properties.Settings.Default.adminChannel)
							{
								Properties.Settings.Default.challongeToken= e.GetArg("NewToken");
								Properties.Settings.Default.Save();
								await e.Channel.SendMessage(String.Format("Changed the token to {0}", e.GetArg("NewToken")));
							}
							else
							{
								await e.Channel.SendMessage("Wong channel accessed.");
							}
						});

				cgb.CreateCommand("3")
						.Description("Changes the challonge account")
						.Parameter("NewAccount", ParameterType.Required)
						.Do(async e =>
						{
							if (e.Channel.ToString() == Properties.Settings.Default.adminChannel)
							{
								Properties.Settings.Default.challongeAccount = e.GetArg("NewAccount");
								Properties.Settings.Default.Save();
								await e.Channel.SendMessage(String.Format("Changed the account to {0}", e.GetArg("NewAccount")));
							}
							else
							{
								await e.Channel.SendMessage("Wong channel accessed.");
							}
						});
			});
		}
	}
}
