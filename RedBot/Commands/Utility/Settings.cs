using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using RedBot.Commands;

namespace RedBot.Commands.Utility
{
	[Group("settings")]
	class Settings : ModuleBase<SocketCommandContext>
	{
		[Command("directory")]
		[Alias("dir")]
		[Summary("Displays the commands you can use for settings.")]
		public async Task DirectoryAsync()
        {
			Console.WriteLine("Hit Directory");
			if (Context.Channel.ToString() == Properties.Settings.Default.adminChannel)
			{
				await Context.Channel.SendMessageAsync("1) Change admin channel.\n2) Change challonge token.\n3) Change the challonge account.");
			}
			else
			{
				await Context.Channel.SendMessageAsync("Wong channel accessed.");
			}
		}

		[Command("changeAdminChannel")]
		[Summary("Changes the channel in which this settings panel comes up.")]
		public async Task ChangeAdminChannelAsync(string newChannel)
        {
			if (Context.Channel.ToString() == Properties.Settings.Default.adminChannel)
			{
				Properties.Settings.Default.adminChannel = newChannel;
				Properties.Settings.Default.Save();
				await Context.Channel.SendMessageAsync(String.Format("Changed admin channel to {0}", newChannel));
			}
			else
			{
				await Context.Channel.SendMessageAsync("Wong channel accessed.");
			}
		}

		[Command("changeChallongeToken")]
		[Summary("Changes the challonge token")]
		public async Task ChangeChallongeTokenAsync(string newToken)
        {
			if(Context.Channel.ToString() == Properties.Settings.Default.adminChannel)
							{
				Properties.Settings.Default.challongeToken = newToken;
				Properties.Settings.Default.Save();
				await Context.Channel.SendMessageAsync(String.Format("Changed the token to {0}", newToken));
			}
			else
			{
				await Context.Channel.SendMessageAsync("Wong channel accessed.");
			}
		}

		[Command("changeChallongeAccount")]
		[Summary("Changes the challonge account")]
		public async Task ChangeChallongeAccountAsync(string newAccount)
        {
			if (Context.Channel.ToString() == Properties.Settings.Default.adminChannel)
			{
				Properties.Settings.Default.challongeAccount = newAccount;
				Properties.Settings.Default.Save();
				await Context.Channel.SendMessageAsync(String.Format("Changed the account to {0}", newAccount));
			}
			else
			{
				await Context.Channel.SendMessageAsync("Wong channel accessed.");
			}
		}
	}
}
