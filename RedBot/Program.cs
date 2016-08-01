using Discord;
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
			string token = "MjA4MzMzMTk4MjczNDEzMTIw.CnwMfg.n0qRQeEqKl05A4pI5H3A_Thi-lQ"; //insert your token here if you want to use this bot!

			if (args.Length == 2)
			{
				token = args[1];
			}

			RedBotClient bot = new RedBotClient(token);
		}
	}
}
