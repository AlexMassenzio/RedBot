using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot.Commands.Challonge
{
	class ChallongeAPIHandler
	{
		private RestClient client;
		public string SelectedTournament { get; set; }

		public enum MatchState {Any, Open, Pending, Complete};

		public ChallongeAPIHandler(string account, string token)
		{
			if(account == "" || account == null || token == "" || token == null)
			{
				throw new ArgumentException("Parameter cannot be null or empty");
			}

			client = new RestClient();
			client.BaseUrl = new Uri("https://api.challonge.com/v1/");
			client.Authenticator = new HttpBasicAuthenticator(account, token);

			var test = ListTournamentstAsync().Result;

			if (test != System.Net.HttpStatusCode.OK)
			{
				throw new ArgumentException("Could not connect to Challonge, check your credentials and the website status.");
			}

			SelectedTournament = null;
		}

		/// <summary>
		/// Currently used as a connection test. Gets a list of all tournaments under the account.
		/// </summary>
		/// <returns></returns>
		public async Task<System.Net.HttpStatusCode> ListTournamentstAsync()
		{
			var request = new RestRequest(Method.GET);
			request.Resource = "/tournaments.xml";

			IRestResponse response = await client.ExecuteTaskAsync(request);

			return response.StatusCode;
		}

		/// <summary>
		/// Adds a name to the participant list of the Challonge bracket.
		/// </summary>
		/// <param name="name">The name entered in to the bracket.</param>
		/// <returns></returns>
		public async Task<System.Net.HttpStatusCode> AddParticipantAsync(string name)
		{
			if (SelectedTournament == null)
			{
				return System.Net.HttpStatusCode.BadRequest;
			}

			var request = new RestRequest(Method.POST);
			request.Resource = string.Format("tournaments/{0}/participants.xml", SelectedTournament);
			request.AddParameter("participant[name]", name);

			IRestResponse response = await client.ExecuteTaskAsync(request);

			return response.StatusCode;
		}

		/// <summary>
		/// STILL IN DEVELOPMENT. Gets every match in a certain bracket.
		/// </summary>
		/// <param name="m">A simple filter for the matches you want. Default is Any.</param>
		/// <returns></returns>
		public async Task<Matches> GetAllMatchInfoAsync(MatchState m)
		{
			if (SelectedTournament == null)
			{
				Console.WriteLine("No selected tournament!");
				return null;
			}

			XmlDeserializer x = new XmlDeserializer();

			var request = new RestRequest(Method.GET);
			request.Resource = string.Format("tournaments/{0}/matches.xml", SelectedTournament);

			switch (m)
			{
				case MatchState.Open:
					request.AddParameter("state", "open");
					break;

				case MatchState.Pending:
					request.AddParameter("state", "pending");
					break;

				case MatchState.Complete:
					request.AddParameter("state", "complete");
					break;
			}

			Console.WriteLine("Starting request...");
			IRestResponse response = await client.ExecuteTaskAsync(request);

			Console.WriteLine("Deserializing...");
			Matches matches = x.Deserialize<Matches>(response);

			Console.WriteLine("Finished!");
			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return matches;
			}

			return null;
		}

		/// <summary>
		/// STILL IN DEVELOPMENT, Gets the information about a specific match.
		/// </summary>
		/// <param name="id">The match's unique id.</param>
		/// <returns></returns>
		public async Task<Match> GetMatchInfoAsync(int id)
		{
			if (SelectedTournament == null)
			{
				return null;
			}
			XmlDeserializer x = new XmlDeserializer();

			var request = new RestRequest(Method.GET);
			request.Resource = string.Format("tournaments/{0}/matches/{1}.xml", SelectedTournament, id);

			IRestResponse response = await client.ExecuteTaskAsync(request);

			Match match = x.Deserialize<Match>(response);

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				return match;
			}

			return null;
		}
	}
}
