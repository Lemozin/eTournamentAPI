using System;
using System.Net.Http;

namespace eTournament.Helpers
{
    public class TournamentAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7170/");
            return client;
        }
    }
}