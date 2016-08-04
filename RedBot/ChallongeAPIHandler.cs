using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBot
{
	class ChallongeAPIHandler
	{
		private RestClient client;
		public string SelectedTournament { get; set; }

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

		public async Task<System.Net.HttpStatusCode> ListTournamentstAsync()
		{
			var request = new RestRequest(Method.GET);
			request.Resource = "/tournaments.xml";

			IRestResponse response = await client.ExecuteTaskAsync(request);

			return response.StatusCode;
		}

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
	}
}
