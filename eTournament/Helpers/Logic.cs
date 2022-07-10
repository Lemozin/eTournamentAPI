﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            var claims = securityToken.Claims;
            return claims;
        }
    }
}