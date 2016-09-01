using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot.Commands.Challonge
{
	class Match
	{
		public int Id { get; set; }
		public string Identifier { get; set; }
		public int Player1_id { get; set; }
		public int Player2_id { get; set; }
		public string State { get; set; }
	}
}
