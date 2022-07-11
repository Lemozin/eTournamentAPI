using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTournament.Data.Enums;
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
            RequestMethods method,
            bool isTokenAuth,
            bool isGetAync,
            string requestUri,
            object param = null,
            string token = null)
        {
            _client = _api.Initial();

            if (isTokenAuth)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _json = JsonConvert.SerializeObject(param);
            _data = new StringContent(_json, Encoding.UTF8, "application/json");
            switch (method)
            {
                case RequestMethods.GET:
                    _responseMessage = await _client.GetAsync(requestUri);
                    break;
                case RequestMethods.POST:
                    _responseMessage = await _client.PostAsync(requestUri, _data);
                    break;
                case RequestMethods.PUT:
                    _responseMessage = await _client.PutAsync(requestUri, _data);
                    break;
                case RequestMethods.DELETE:
                    if (param != null)
                        _responseMessage = await _client.DeleteAsync(requestUri);
                    else
                        _responseMessage = await _client.PostAsync(requestUri, _data);
                    break;
            }

            return _responseMessage;
        }

        public Task<HttpResponseMessage> GetPostHttpClient(
            RequestMethods method,
            bool isTokenAuth,
            bool isGetAync,
            string requestUri,
            object param = null,
            string token = null)
        {
            _client = _api.Initial();

            if (isTokenAuth)
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _json = JsonConvert.SerializeObject(param);
            _data = new StringContent(_json, Encoding.UTF8, "application/json");
            switch (method)
            {
                case RequestMethods.GET:
                    _responseMessageNoAsync = _client.GetAsync(requestUri);
                    break;
                case RequestMethods.POST:
                    _responseMessageNoAsync = _client.PostAsync(requestUri, _data);
                    break;
                case RequestMethods.PUT:
                    _responseMessageNoAsync = _client.PutAsync(requestUri, _data);
                    break;
                case RequestMethods.DELETE:
                    if (param != null)
                        _responseMessageNoAsync = _client.DeleteAsync(requestUri);
                    else
                        _responseMessageNoAsync = _client.PostAsync(requestUri, _data);
                    break;
            }

            return _responseMessageNoAsync;
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            var claims = securityToken.Claims;
            return claims;
        }

        public void SendCompletedOrderEmail(
            string Body,
            string ToMail,
            int Port,
            string Host,
            string Username,
            string Password)
        {
            var message = new MailMessage();
            message.From = new MailAddress("info@etournament.com");
            message.To.Add(ToMail);
            message.Subject = "New order request";
            message.IsBodyHtml = true;
            message.Body = Body;

            var smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;

            smtpClient.Host = Host;
            smtpClient.Port = Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(Username, Password);
            smtpClient.Send(message);
        }
    }
}