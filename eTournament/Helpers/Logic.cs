using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace eTournament.Helpers
{
    public class Logic
    {
        private readonly TournamentAPI _api = new();
        private HttpClient _client = new();
        private StringContent _data;
        private string _json;
        private HttpResponseMessage _responseMessage = new();
        private Task<HttpResponseMessage> _responseMessageNoAsync;

        public async Task<HttpResponseMessage> GetPostHttpClientAsync(
            bool isTokenAuth,
            bool isGetAync,
            string requestUri,
            object param = null,
            string token = null)
        {
            _client = _api.Initial();
            switch (isGetAync)
            {
                case true:

                    if (isTokenAuth)
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    _responseMessage = await _client.GetAsync(requestUri);
                    break;
                default:
                    if (isTokenAuth)
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    _json = JsonConvert.SerializeObject(param);
                    _data = new StringContent(_json, Encoding.UTF8, "application/json");
                    _responseMessage = await _client.PostAsync(requestUri, _data);
                    break;
            }

            return _responseMessage;
        }

        public Task<HttpResponseMessage> GetPostHttpClient(
            bool isTokenAuth,
            bool isGetAync,
            string requestUri,
            object param = null,
            string token = null)
        {
            _client = _api.Initial();
            switch (isGetAync)
            {
                case true:

                    if (isTokenAuth)
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    _responseMessageNoAsync = _client.GetAsync(requestUri);
                    break;
                default:
                    if (isTokenAuth)
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    _json = JsonConvert.SerializeObject(param);
                    _data = new StringContent(_json, Encoding.UTF8, "application/json");
                    _responseMessageNoAsync = _client.PostAsync(requestUri, _data);
                    break;
            }

            return _responseMessageNoAsync;
        }
    }
}